using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private StorageUI storageUI;
    [SerializeField] private StorageUI quickStorageUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private HoldItemUI holdItemUI;

    public ItemInstance holdItem;
    public bool onItemHold = false;

    private void Start()
    {
        inventory.quickStorage.onChangeStorage += quickStorageUI.UpdateUI;
        inventory.storage.onChangeStorage += storageUI.UpdateUI;
    }

    private void Update()
    {
        if(onItemHold)
        {
            holdItemUI.transform.position = Input.mousePosition;
        }
    }

    public void OnClickSlotUI(int slotIndex, StorageType storageType)
    {
        Storage currentStorage = GetStorageByType(storageType);

        if(onItemHold)
        {
            HandleItemPlacement(currentStorage, slotIndex);
        }
        else
        {
            HandleItemPickup(currentStorage, slotIndex);
        }
    }
    
    private Storage GetStorageByType(StorageType storageType)
    {
        return storageType == StorageType.Normal ? inventory.storage : inventory.quickStorage;
    }
    
    private void HandleItemPlacement(Storage storage, int slotIndex)
    {
        if(storage.items[slotIndex] != null)
        {
            if(storage.items[slotIndex].itemInfo.key == holdItem.itemInfo.key)
            {
                TryStackItems(storage, slotIndex);
            }
            else
            {
                SwapItems(storage, slotIndex);
            }
        }
        else
        {
            PlaceItemInEmptySlot(storage, slotIndex);
        }
    }
    
    private void TryStackItems(Storage storage, int slotIndex)
    {
        if(storage.TryAddItemAt(slotIndex, holdItem))
        {
            ClearHoldItem();
        }
        else holdItemUI.UpdateHoldItem(holdItem);
    }
    
    private void SwapItems(Storage storage, int slotIndex)
    {
        storage.SwapItem(slotIndex, ref holdItem);
        holdItemUI.UpdateHoldItem(holdItem);
    }
    
    private void PlaceItemInEmptySlot(Storage storage, int slotIndex)
    {
        storage.TryAddItemAt(slotIndex, holdItem);
        ClearHoldItem();
    }
    
    private void HandleItemPickup(Storage storage, int slotIndex)
    {
        holdItem = storage.items[slotIndex];

        if(holdItem != null)
        {
            onItemHold = true;
            holdItemUI.UpdateHoldItem(holdItem);
            holdItemUI.gameObject.SetActive(true);
            storage.RemoveItemAt(slotIndex);
        }
    }
    
    private void ClearHoldItem()
    {
        holdItem = null;
        onItemHold = false;
        holdItemUI.gameObject.SetActive(false);
    }
}
