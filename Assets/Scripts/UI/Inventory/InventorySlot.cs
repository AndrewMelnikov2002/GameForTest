using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public InventorySystem inventory;

    [SerializeField] private int slot_id;

    [SerializeField] private Image outline_image;
    [SerializeField] private Image icon;
    [SerializeField] private Text count_text;

    public void UpdateSlot(Sprite icon_sprite, int count)
    {
        if (icon_sprite != null)
        {
            icon.sprite = icon_sprite;
            icon.enabled = true;
        }
        else
            icon.enabled = false;

        if (count > 1)
        {
            count_text.text = "x" + count.ToString();
            count_text.enabled = true;
        }
        else
            count_text.enabled = false;

    }
    
    public void SetID(int id) { slot_id = id; }

    public void SetInventorySystem(InventorySystem inventory_system) { inventory = inventory_system; }

    public void SelectedEnabled(bool enabled) { outline_image.enabled = enabled; }

    public void Selected() {if (icon.enabled) inventory.SlotSelected(slot_id); }
}
