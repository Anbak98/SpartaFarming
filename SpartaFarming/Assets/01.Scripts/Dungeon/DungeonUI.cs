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

    public void OnOffPanel(bool flag) 
    {
        panel.SetActive(flag);
    }

    private void ReturnToVillage()
    {
        // ���� ������ �Ѿ��
        SceneLoader.Instance.ChangeScene(SceneState.MainScene);
    }

    private void OffPanel() 
    {
        panel.SetActive(false);
    }
}
