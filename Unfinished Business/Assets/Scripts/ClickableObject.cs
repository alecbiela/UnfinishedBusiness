/* ClickableObject.cs - by Unity forum user jfarias
 * This script can be added to any GameObject that will
 * execute code based on the pointer clicks (left, right, and middle)
 * This will replace the UI click handlers so that we can do right clicks
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    private Inventory player;
    public int slot;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Inventory>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            player.View(this.slot);
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right)
            player.Select(this.slot);
    }
}
