using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour {

    private List<GameObject> items;
    private GameObject[] slots;
    private Color OCCUPIED_COLOR = new Color(255, 255, 255, 255);
    private Color VACANT_COLOR = new Color(255, 255, 255, 0);
    private GameManager gm;

	// Use this for initialization
	void Start () {
        items = new List<GameObject>();
        slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

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

    public void View(int slotNum)
    {
        if ((slotNum > items.Count)) return;
        gm.StartViewingObject(items[slotNum - 1], true);
    }

    public void Select(int slotNum)
    {
        if ((slotNum > items.Count)) return;
        gm.SelectObject(items[slotNum - 1]);
    }

    //returns an item (used by UI to get item information
    public GameObject GetItem(int slot)
    {
        if (slot > items.Count) return null;

        return items[slot-1];
    }


    //adds an item to the inventory
    //takes an arg for the item script
    public void AddItem(GameObject i)
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
            if (items[i].GetComponent<Item>().itemName != name) continue;

            //once we've found it, remove it and get out of the loop
            items.RemoveAt(i);
            break;
        }

        //re-sort to account for vacant space
        slots[items.Count].GetComponent<Image>().color = VACANT_COLOR;
        ReSortItems();
    }

    //re-displays the images to account for adding/removing
    private void ReSortItems()
    {
        Image currentImage;
        for(int i=0; i < items.Count; i++)
        {
            currentImage = slots[i].GetComponent<Image>();
            currentImage.sprite = items[i].GetComponent<Item>().uiImage;
            currentImage.color = OCCUPIED_COLOR;
        }
    }

}
