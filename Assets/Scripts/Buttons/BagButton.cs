using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagButton : MonoBehaviour
{
    private Bag bag;

    [SerializeField] private Sprite full, empty;

    public Bag MyBag
    {
        get => bag;
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }
}
