using Mirror;

// =======================================================================================
// PLAYER
// =======================================================================================
public partial class Player
{
    // -----------------------------------------------------------------------------------
    // CmdSwapEquipInventory
    // -----------------------------------------------------------------------------------
    [Command]
    public void CmdSwapEquipInventory(int equipmentIndex, int inventoryIndex)
    {
        // validate: make sure that the slots actually exist in the inventory
        // and in the equipment
        if (isAlive &&
            0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
            0 <= equipmentIndex && equipmentIndex < equipment.slots.Count)
        {
            // equipment slot has to be empty (unequip) or un-equipable
            ItemSlot slot = equipment.slots[equipmentIndex];
            
            if (slot.amount == 0 ||
                slot.item.data is EquipmentItem &&
                ((EquipmentItem)slot.item.data).CanUnequip(this, inventoryIndex, equipmentIndex))
            {
                // swap them
                ItemSlot temp = inventory.slots[inventoryIndex];
                inventory.slots[inventoryIndex] = slot;
                equipment.slots[equipmentIndex] = temp;
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // OnDragAndDrop_EquipmentSlot_InventorySlot
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnDragAndDrop")]
    private void OnDragAndDrop_EquipmentSlot_InventorySlot(int[] slotIndices)
    {
        // slotIndices[0] = slotFrom; slotIndices[1] = slotTo
        CmdSwapEquipInventory(slotIndices[0], slotIndices[1]);
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================