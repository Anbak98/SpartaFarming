using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Storage quickStorage;
    public Storage storage;

    public void Awake()
    {
        quickStorage = new Storage(12);
        storage = new Storage(24);
    }

    public bool AddItem(ItemInstance item)
    {
        return quickStorage.AddItem(item) ? true : storage.AddItem(item);
    }

    public bool AddNewItem(int key, int quantity = 1)
    {
        ItemInfo itemInfo = DataManager.ItemLoader.GetByKey(key);
        ItemInstance itemInstance = new ItemInstance(itemInfo, quantity);
        return AddItem(itemInstance);
    }

    public void RemoveItem(int key, int quantity = 1)
    {
        quickStorage.RemoveItem(key, ref quantity);
        storage.RemoveItem(key, ref quantity);
    }
}
