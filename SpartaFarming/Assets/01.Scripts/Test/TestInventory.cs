using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private int key;
    [SerializeField] private int quantity;

    public void AddItem()
    {
        inventory.AddNewItem(key, quantity);
    }

    public void RemoveItem()
    {
        inventory.RemoveItem(key, quantity);
    }
}
