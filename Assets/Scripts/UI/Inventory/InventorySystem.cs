using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventory;
    [SerializeField] private List<InventorySlot> ui_slots;

    [SerializeField] private GameObject remove_button;

    [SerializeField] private GameObject inventory_button;

    [SerializeField] private GameObject slots_panel;

    [SerializeField] private Text selected_item_name;

    private void Start()
    {
        for ( int i = 0; i < ui_slots.Count;i++ )
        {
            ui_slots[i].SelectedEnabled(false);
            ui_slots[i].UpdateSlot(null, 0);
            ui_slots[i].SetID(i);
            ui_slots[i].SetInventorySystem(this);
        }

        selected_item_name.text = "";
    }

    public void AddItem(ItemConfig config, int count)
    {
        if (config.isStackble)
            for (int i = 0; i < inventory.Count; i++)
                if (inventory[i].id == config.Id)
                {
                    inventory[i].id = config.Id;
                    inventory[i].config = config;
                    inventory[i].count += count;
                    ui_slots[i].UpdateSlot(config.Icon, inventory[i].count);
                    return;
                }
        
        for (int i = 0; i < inventory.Count; i++)
            if (inventory[i].id == "")
            {
                inventory[i].id = config.Id;
                inventory[i].config = config;
                inventory[i].count += count;
                ui_slots[i].UpdateSlot(config.Icon, inventory[i].count);
                return;
            }
    }


    public void RemoveItem()
    {
        if (inventory[selected_slot_id].count == 1)
        {

            inventory[selected_slot_id].id = "";
            inventory[selected_slot_id].count = 0;
            inventory[selected_slot_id].config = null;

            ui_slots[selected_slot_id].UpdateSlot(null, 0);

            ui_slots[selected_slot_id].SelectedEnabled(false);

            selected_slot_id = -1;

            remove_button.SetActive(false);

            selected_item_name.text = "";

        }
        else
        {
            inventory[selected_slot_id].count -= 1;
            ui_slots[selected_slot_id].UpdateSlot(inventory[selected_slot_id].config.Icon, inventory[selected_slot_id].count);
        }
    }


    private int selected_slot_id = -1;
    public void SlotSelected(int id)
    {
        foreach (InventorySlot slot in ui_slots)
            slot.SelectedEnabled(false);

        ui_slots[id].SelectedEnabled(true);

        selected_slot_id = id;

        remove_button.SetActive(true);

        selected_item_name.text = inventory[selected_slot_id].config.InGameName;
    }


    public void ShowInventory()
    {
        slots_panel.SetActive(!slots_panel.activeSelf);

        selected_slot_id = -1;

        for (int i = 0; i < ui_slots.Count; i++)
            ui_slots[i].SelectedEnabled(false);

        remove_button.SetActive(false);
        selected_item_name.text = "";
    }

    public List<InventoryItem> Inventory { get { return inventory; } }


    public void LoadInventory(List<InventoryItem> inventory)
    {
        for (int x = 0; x < inventory.Count; x++)
        {
            this.inventory[x].id = inventory[x].id;
            this.inventory[x].config = inventory[x].config;
            this.inventory[x].count = inventory[x].count;

            if (this.inventory[x].config != null)
                ui_slots[x].UpdateSlot(inventory[x].config.Icon, inventory[x].count);
        }
    }
}




[System.Serializable]
public class InventoryItem
{
    public string id;

    public ItemConfig config;

    public int count;

}
