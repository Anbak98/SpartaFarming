using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Storage storage;
    public Storage quickSlots;

    public void Init()
    {
        storage = new Storage(12);
        quickSlots = new Storage(12);
    }
}
