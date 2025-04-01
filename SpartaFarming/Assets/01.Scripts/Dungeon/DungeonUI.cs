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
        // 메인 씬으로 넘어가기
        Debug.Log("메인씬으로 돌아갑니다");
    }

    private void OffPanel() 
    {
        panel.SetActive(false);
    }
}
