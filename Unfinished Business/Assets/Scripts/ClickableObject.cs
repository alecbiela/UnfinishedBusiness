/* ClickableObject.cs
 * This script can be added to any GameObject that will
 * execute code based on the pointer clicks (left, right, and middle)
 * This will replace the UI click handlers so that we can do right clicks
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Inventory inventory;
    private GameObject textBox;
    public int slot;

    //make sure all of the slots can find the textbox before it's turned off
    void Awake()
    {
        textBox = GameObject.FindGameObjectWithTag("ItemUIText");
    }

    //find the inventory and turn off the examine box (if not off already)
    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        if (textBox.activeSelf) textBox.SetActive(false);
    }

    //handles pointer click on this slot
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            inventory.View(this.slot);
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right)
            inventory.Select(this.slot);
    }

    //handles when the pointer hovers over this slot
    public void OnPointerEnter(PointerEventData eventData)
    {
        //get this slot data
        GameObject hoverItem = inventory.GetItem(this.slot);

        //shorthand for == null
        if (!hoverItem) return;

        Item hoverItemScript = hoverItem.GetComponent<Item>();

        textBox.SetActive(true);
        textBox.GetComponentInChildren<Text>().text = hoverItemScript.itemName + "\n" + hoverItemScript.description;  //add description when we have it in item script

        Debug.Log("Entered!" + slot);
    }

    //when we leave this selection, hide the examine box
    public void OnPointerExit(PointerEventData eventData)
    {
        if (textBox.activeSelf) textBox.SetActive(false);
    }
}
