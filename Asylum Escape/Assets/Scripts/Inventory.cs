using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory 
{
    private List<Item> itemList;

    [SerializeField]
    static public int size = 5;

    public Inventory()
    {
        itemList = new List<Item>();
        for(int i = 0; i < size; i++)
            addItem(new Item(Item.ItemType.Null));
        
    }


    public void addItem(Item item)
    {
        itemList.Add(item);
    } 
    
    public List<Item> GetItems()
    {
        return itemList;
    }

    public void setItem(int poz , Item item)
    {
        // So it doesn't override items
        if (itemList[poz].itemType != Item.ItemType.Null)
        {
            for (int i = 0; i< size; i++)
            {
                if (itemList[i].itemType == Item.ItemType.Null)
                {
                    itemList[i] = item;
                    return;
                }
            }
        }
        itemList[poz] = item;
    }

    public void removeBattery()
    {
        for (int i = 0;i< size; i++)
        {
            if (itemList[i].itemType == Item.ItemType.Battery)
            {
                itemList[i].itemType = Item.ItemType.Null;
                return;
            }
        }
    }

    public string printInv()
    {
        string s = "";
        foreach(Item i in itemList)
        {
            s += i.itemType + "     ";
        }
        return s;
    }

    public bool hasKey(int poz)
    {
        if (itemList[poz].itemType == Item.ItemType.Key)
            return true;

        /*
        for (int i = 0; i < size; i++)
        {
            if (itemList[i].itemType == Item.ItemType.Key)
            {
                return true;
            }
        }
        */
        return false;
    }

    public bool hasEscapeKey(int poz)
    {
        if (itemList[poz].itemType == Item.ItemType.EscapeKey)
            return true;

        return false;
    }

    public void removeKey(int poz)
    {
        if (itemList[poz].itemType == Item.ItemType.Key)
        {
            itemList[poz].itemType = Item.ItemType.Null;
        }

        /*
        for (int i = 0; i < size; i++)
        {
            if (itemList[i].itemType == Item.ItemType.Key)
            {
                itemList[i].itemType = Item.ItemType.Null;
            }
        }
        */
    }

    public void removeEscapeKey(int poz)
    {
        if (itemList[poz].itemType == Item.ItemType.EscapeKey)
        {
            itemList[poz].itemType = Item.ItemType.Null;
        }
    }
}
