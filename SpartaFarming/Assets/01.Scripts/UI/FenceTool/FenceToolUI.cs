using UnityEngine;

public class FenceToolUI : MonoBehaviour
{
    public FenceToolSlot fenceSlot01;
    public FenceToolSlot fenceSlot02;
    public FenceToolSlot fenceSlot03;
    public FenceToolSlot fenceSlot04;
    public FenceToolSlot fenceSlot05;
    public FenceToolSlot fenceSlot06;
    public FenceToolSlot fenceSlot07;
    public FenceToolSlot fenceSlot08;
    public FenceToolSlot fenceSlot09;
    public FenceToolSlot fenceSlot10;

    public FenceToolSlot[] fenceSlots;

    void Start()
    {
        fenceSlots = new FenceToolSlot[10] { fenceSlot01, fenceSlot02, fenceSlot03, fenceSlot04, fenceSlot05, fenceSlot06, fenceSlot07, fenceSlot08, fenceSlot09, fenceSlot10 };
    }

    public void SelectOnlyOneItem()
    {
        for (int i = 0; i < fenceSlots.Length; i++)
        {
            if (fenceSlots[i].isSelected) fenceSlots[i].isSelected = false;
        }
    }
}