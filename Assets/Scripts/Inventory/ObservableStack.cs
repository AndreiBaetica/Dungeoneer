using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.UIElements;

public delegate void UpdateStackEvent();

//Used to raise events, and update other things such as slot number based on them
public class ObservableStack<T> : Stack<T>
{
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    public ObservableStack(ObservableStack<T> items) : base(items) // Instantiate a stack with an amount of items
    {
    }

    public ObservableStack()
    {
    }

    public new void Push(T item)
    {
        base.Push(item);

        if (OnPush != null)
        {
            OnPush();
        }
    }
    
    public new T Pop()
    {
        T item = base.Pop();

        if (OnPop != null)
        {
            OnPop();
        }

        return item;
    }
    
    public new void Clear()
    {
        base.Clear();

        if (OnClear != null)
        {
            OnClear();
        }
    }
}
