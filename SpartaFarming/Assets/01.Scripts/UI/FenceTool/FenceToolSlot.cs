using UnityEngine;
using UnityEngine.UI;

public class FenceToolSlot : MonoBehaviour
{
    public FenceToolUI fenceToolUI;
    public bool isSelected = false;

    public Button slotBtn;
    public Outline selectedOutline;

    void Start()
    {
        slotBtn.onClick.AddListener(ChangeCurSelectedItem);
    }

    void Update()
    {

        selectedOutline.enabled = isSelected;
    }

    void ChangeCurSelectedItem()
    {
        if (!isSelected)
        {
            fenceToolUI.SelectOnlyOneItem();
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }
    }
}