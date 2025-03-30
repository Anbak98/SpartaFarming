using UnityEngine;
using UnityEngine.UI;

public class EquipItemSlot : MonoBehaviour
{
    public EquipItemUI equipItemUI;
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
            equipItemUI.SelectOnlyOneItem();
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }
    }
}
