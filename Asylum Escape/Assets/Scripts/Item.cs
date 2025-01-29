using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType
    {
        Key,
        EscapeKey,
        Battery,
        Null
    }
    public enum MaxStack
    {
        Key = 2,
        EscapeKey = 1,
        Battery = 2,
        Null = 1
        
    }
    public Item(ItemType type)
    {
        itemType = type;
        stack = 1;
    }

    public ItemType itemType;
    public int stack;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Key: return ItemAssestsManager.Instance.keySprite;
            case ItemType.EscapeKey: return ItemAssestsManager.Instance.escapeKeySprite;
            case ItemType.Battery: return ItemAssestsManager.Instance.batterySprite;
            case ItemType.Null: return ItemAssestsManager.Instance.Null;
        }
    }
}
