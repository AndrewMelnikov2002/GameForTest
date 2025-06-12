using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private List<InventoryItem> inventory;



    [Space(20)]
    [Header("UI")]
    [SerializeField] private List<InventorySlot> ui_slots;
    [Space(10)]

    [SerializeField] private CanvasGroupFade slots_panel_fade;
    [SerializeField] private CanvasGroupFade remove_button_fade;
    [Space(10)]
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

        CanvasGroup rmv_btn_cg = remove_button_fade.GetComponent<CanvasGroup>();

        rmv_btn_cg.alpha = 0;
        rmv_btn_cg.interactable = false;
        rmv_btn_cg.blocksRaycasts = false;

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


    private void RemoveItem(int id)
    {
        if (inventory[id].count == 1)
        {

            inventory[id].id = "";
            inventory[id].count = 0;
            inventory[id].config = null;

            ui_slots[id].UpdateSlot(null, 0);

            ui_slots[id].SelectedEnabled(false);

            selected_slot_id = -1;

            remove_button_fade.StartFadeOut(4f);

            selected_item_name.text = "";

        }
        else
        {
            inventory[id].count -= 1;
            ui_slots[id].UpdateSlot(inventory[selected_slot_id].config.Icon, inventory[id].count);
        }
    }

    public void RemoveItemButtonEvent()
    {
        if (selected_slot_id >= 0)
            RemoveItem(selected_slot_id);
    }


    private int selected_slot_id = -1;
    public void SlotSelected(int id)
    {
        if (selected_slot_id == id)
            return;

        foreach (InventorySlot slot in ui_slots)
            slot.SelectedEnabled(false);

        ui_slots[id].SelectedEnabled(true);

        selected_slot_id = id;

        remove_button_fade.StartFadeIn(4f);

        selected_item_name.text = inventory[selected_slot_id].config.InGameName;
    }

    public void ItemUsed(int id)
    {
        if (inventory[id].id == "health_potion")
        {
            RemoveItem(id);

            FindAnyObjectByType<PlayerController>().HealPlayer(50);
        }
    }




    public void ShowInventory()
    {      
        if(!slots_panel_fade.GetComponent<CanvasGroup>().interactable)
            slots_panel_fade.StartFadeIn(4f);
        else
            slots_panel_fade.StartFadeOut(4f);

        selected_slot_id = -1;

        for (int i = 0; i < ui_slots.Count; i++)
            ui_slots[i].SelectedEnabled(false);


        if(remove_button_fade.GetComponent<CanvasGroup>().interactable)
            remove_button_fade.StartFadeOut(4f);


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
