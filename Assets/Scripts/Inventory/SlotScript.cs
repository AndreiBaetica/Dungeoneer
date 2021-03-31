using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
   private ObservableStack<Item> items = new ObservableStack<Item>();
   
   [SerializeField]
   private Image icon;

   [SerializeField] 
   private Text stackSize;

   public bool IsEmpty
   {
      get
      {
         return items.Count == 0;
      }
   }

   public bool IsFull
   {
      get
      {
         if (IsEmpty || MyCount < MyItem.MyStackSize)
         {
            return false;
         }

         return true;
      }
   }

   public Item MyItem
   {
      get
      {
         if (!IsEmpty)
         {
            return items.Peek();
         }

         return null;
      }
   }

   public Image MyIcon
   {
      get
      {
         return icon;
      }
      set
      {
         icon = value;
      }
   }

   public int MyCount
   {
      get { return items.Count; }
   }

   public Text MyStackText
   {
      get
      {
         return stackSize;
      }
   }

   private void Awake()
   {
      items.OnPop += new UpdateStackEvent(UpdateSlot);
      items.OnPush += new UpdateStackEvent(UpdateSlot);
      items.OnClear += new UpdateStackEvent(UpdateSlot);
   }

   public bool AddItem(Item item)
   {
      items.Push(item);
      icon.sprite = item.MyIcon;
      icon.color = Color.white;
      item.MySlot = this;
      return true; // TODO: change this later
   }

   public bool AddItems(ObservableStack<Item> newItems)
   {
      if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType()) // if my slot is empty or contains the same item type as the item(s) in my hand that im trying to drop
      {
         int count = newItems.Count;
         for (int i = 0; i < count; i++) // for each item in newItems
         {
            if (IsFull) // slot already full, cant add more
            {
               return false;
            }

            AddItem(newItems.Pop());
         }

         return true;
      }

      return false;
   }

   public void RemoveItem(Item item)
   {
      if (!IsEmpty)
      {
         items.Pop();
         //UIManager.MyInstance.UpdateStackSize(this);
      }
   }

   public void Clear()
   {
      if (items.Count > 0)
      {
         items.Clear();
      }
   }

   //Called when slot is clicked
   public void OnPointerClick(PointerEventData eventData)
   {
      if (eventData.button == PointerEventData.InputButton.Left) // Left click to move item
      {
         if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) // if you don't have something in your hand already and the slot is not empty
         {
            HandScript.MyInstance.TakeMoveable(MyItem as IMoveable); // take the item
            InventoryScript.MyInstance.FromSlot = this;
         }
         else if (InventoryScript.MyInstance.FromSlot != null) // the player has something in their hand
         {
            if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.items)) // keep this order : put back > merge > swap > add
            {
               HandScript.MyInstance.Drop();
               InventoryScript.MyInstance.FromSlot = null;
            }
         }
      }
      if (eventData.button == PointerEventData.InputButton.Right) // Right click to use item
      {
         UseItem();
      }
   }

   public void UseItem()
   {
      if (MyItem is IUsable)
      {
         (MyItem as IUsable).Use();
      }
   }

   public bool StackItem(Item item)
   {
      if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
      {
         items.Push(item);
         item.MySlot = this;
         return true;
      }

      return false;
   }

   private void UpdateSlot()
   {
      UIManager.MyInstance.UpdateStackSize(this);
   }

   private bool PutItemBack()
   {
      if (InventoryScript.MyInstance.FromSlot == this) // if you click back on the same slot that you picked the item from
      {
         InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white; // reset color
         return true;
      }

      return false;
   }

   private bool SwapItems(SlotScript from)
   {
      if (IsEmpty) 
      {
         return false; // no need to swap
      }

      if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize) // incompatible items types or stack sizes
      {
         ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items); // copy of the items from slot 1
         from.items.Clear(); // clear slot 1
         from.AddItems(items); // take items from slot 2 and copy them to slot 1
         items.Clear(); // clear slot 2
         AddItems(tmpFrom); // take items from copy of slot 1 to slot 2
         return true;
      }

      return false;
   }

   private bool MergeItems(SlotScript from)
   {
      if (IsEmpty)
      {
         return false; // no need to merge
      }
      if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
      {
         int free = MyItem.MyStackSize - MyCount; // free slots
         for (int i = 0; i < free; i++)
         {
            AddItem(from.items.Pop());
         }

         return true;
      }

      return false;
   }
}
