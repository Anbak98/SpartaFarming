using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Button returnToVillageButton;
    [SerializeField]
    private Button offButton;

    private void Awake()
    {
        returnToVillageButton.onClick.AddListener(ReturnToVillage);
        offButton.onClick.AddListener(OffPanel);
    }

    public void OnOffPanel() 
    {
        panel.SetActive( !panel.activeSelf );
    }

    private void ReturnToVillage()
    {
        // ���� ������ �Ѿ��
        Debug.Log("���ξ����� ���ư��ϴ�");
    }

    private void OffPanel() 
    {
        panel.SetActive(false);
    }
}
