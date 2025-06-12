using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private InventorySystem inventory;

    [Header("General")]
    [SerializeField] private int slot_id;


    [Space(20)]
    [Header("UI")]
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


    private float last_click_time = 0;
    public void Selected()
    {

        float time_from_last_click = Time.time - last_click_time;

        last_click_time = Time.time;

        if (icon.enabled)
        {
            inventory.SlotSelected(slot_id);

            if (time_from_last_click < 0.25f)
                inventory.ItemUsed(slot_id);
        }
    }
}
