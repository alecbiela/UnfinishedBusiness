using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour {

    private List<Item> items;
    private GameObject[] slots;
    private Color OCCUPIED_COLOR = new Color(255, 255, 255, 255);
    private Color VACANT_COLOR = new Color(255, 255, 255, 0);

    //private GameManager gm;

	// Use this for initialization
	void Start () {
        items = new List<Item>();
        slots = GameObject.FindGameObjectsWithTag("InventorySlot");

        GameObject[] slotsTmp = new GameObject[slots.Length];

        //organize slots(since we don't know how the compiler found them)
        for(int i=0; i < slots.Length; i++)
        {
            string slotNum = slots[i].name.Split(' ')[1];
            int iSlotNum = int.Parse(slotNum);
            slotsTmp[iSlotNum - 1] = slots[i];
        }

        slots = slotsTmp;
	}

    //adds an item to the inventory
    //takes an arg for the item script
    public void AddItem(Item i)
    {
        items.Add(i);
        ReSortItems();
    }

    //removes item
    public void RemoveItem(string name)
    {
        //loop through current items
        for(int i=0; i < items.Count; i++)
        {
            //if this one isn't what we're looking for, skip to next
            if (items[i].itemName != name) continue;

            //once we've found it, remove it and get out of the loop
            items.RemoveAt(i);
            break;
        }

        //re-sort to account for vacant space
        slots[items.Count - 1].GetComponent<Image>().color = VACANT_COLOR;
        ReSortItems();
    }

    //re-displays the images to account for adding/removing
    private void ReSortItems()
    {
        Image currentImage;
        for(int i=0; i < items.Count; i++)
        {
            currentImage = slots[i].GetComponent<Image>();
            currentImage.sprite = items[i].uiImage;
            currentImage.color = OCCUPIED_COLOR;
        }
    }

}
