using UnityEngine;

public class EquipItemUI : MonoBehaviour
{
    public EquipItemSlot itemSlot01;
    public EquipItemSlot itemSlot02;
    public EquipItemSlot itemSlot03;
    public EquipItemSlot itemSlot04;
    public EquipItemSlot itemSlot05;
    public EquipItemSlot itemSlot06;
    public EquipItemSlot itemSlot07;
    public EquipItemSlot itemSlot08;
    public EquipItemSlot itemSlot09;
    public EquipItemSlot itemSlot10;

    public EquipItemSlot[] itemSlots;

    void Start()
    {
        itemSlots = new EquipItemSlot[10] { itemSlot01, itemSlot02, itemSlot03, itemSlot04, itemSlot05, itemSlot06, itemSlot07, itemSlot08, itemSlot09, itemSlot10 };
    }

    public void SelectOnlyOneItem()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].isSelected) itemSlots[i].isSelected = false;
        }
    }
}
