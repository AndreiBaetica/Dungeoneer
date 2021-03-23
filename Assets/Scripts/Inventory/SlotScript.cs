using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
   private Stack<Item> items = new Stack<Item>();
   
   [SerializeField]
   private Image icon;

   public bool IsEmpty
   {
      get
      {
         return items.Count == 0;
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

   public bool AddItem(Item item)
   {
      items.Push(item);
      icon.sprite = item.MyIcon;
      icon.color = Color.white;
      item.MySlot = this;
      return true; // TODO: change this later
   }

   public void RemoveItem(Item item)
   {
      if (!IsEmpty)
      {
         items.Pop();
         UIManager.MyInstance.UpdateStackSize(this);
      }
   }

   public void OnPointerClick(PointerEventData eventData)
   {
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
}
