using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplete;
    [SerializeField] private Flashlight flashlight;
    public int selected = 0;

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            if (scroll > 0)
                selected++;
            else
                selected--;
            if (selected < 0)
                selected = Inventory.size - 1;
            else if (selected > Inventory.size - 1)
                selected = 0;

            refreshInventory();
        }
        
    }

    public void Awake()
    {
        itemSlotContainer = transform.Find("Inventory");
        itemSlotTemplete = itemSlotContainer.Find("ItemSlotTemplate");


    }

    public void setInventory(Inventory inventory)
    {
        this.inventory = inventory;
        refreshInventory();
    }

    public void UpdateIventory(string name)
    {
        switch (name)
        {
            default:
                {
                    inventory.setItem(selected, new Item(Item.ItemType.Null));
                    break;
                }
            case "Key":
                {
                    inventory.setItem(selected, new Item(Item.ItemType.Key));
                    break;
                }
            case "Escape Key":
                {
                    inventory.setItem(selected, new Item(Item.ItemType.EscapeKey));
                    break;
                }
            case "Battery":
                {
                    inventory.setItem(selected, new Item(Item.ItemType.Battery));
                    flashlight.batteries += 1; 
                    break;
                }
            case "FlashlightItem":
                {
                    flashlight.exists = true;
                    flashlight.showBody();
                    break;
                }
        }
        refreshInventory();
    }

    public void removeBattery()
    {
        inventory.removeBattery();
        refreshInventory();
    }

    public void removeKey()
    {
        inventory.removeKey(selected);
        refreshInventory();
    }

    public void refreshInventory()
    {
        int poz = 0;
        float cellSize = 180;
        Vector2 defaultSize = new Vector2(200, 200); 
        Vector2 selectedSize = new Vector2(240, 230); 

        
        foreach (Item item in inventory.GetItems())
        {
            RectTransform itemSlotTransform;

            
            if (poz < itemSlotContainer.childCount)           
                itemSlotTransform = itemSlotContainer.GetChild(poz).GetComponent<RectTransform>();
            else
            {
                itemSlotTransform = Instantiate(itemSlotTemplete, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotTransform.gameObject.SetActive(true);
            }

            
            itemSlotTransform.anchoredPosition = new Vector2(poz * cellSize - 600f, 0);

            
            RectTransform imageTransform = itemSlotTransform.Find("Image").GetComponent<RectTransform>();
            imageTransform.sizeDelta = defaultSize;

            RectTransform backgroundTransform = itemSlotTransform.Find("Background").GetComponent<RectTransform>();
            backgroundTransform.sizeDelta = defaultSize;

            
            if (poz == selected)
            {
                imageTransform.sizeDelta = selectedSize;
                backgroundTransform.sizeDelta = selectedSize;
            }

            
            Image img = itemSlotTransform.Find("Image").GetComponent<Image>();
            img.sprite = item.GetSprite();

            poz++;
        }
    }

}
