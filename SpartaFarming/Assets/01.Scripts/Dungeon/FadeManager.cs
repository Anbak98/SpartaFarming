using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField]
    private Image fadeImagePrefab;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private Ease easeType = Ease.InOutQuad;

    [Header("===Cavas===")]
    [SerializeField]
    private Canvas cavas;
    [SerializeField]
    private Image fadeImage;

    /*
    private void Awake()
    {
        // DontDestory ���� 
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

    }
    */

    public void FindCanvas() 
    {
        Debug.Log("ĵ���� ã�� ����");

        // ĵ���� ã��
        // ������ ����� 
        try
        {
            cavas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        catch (Exception e) { Debug.Log(e); }

        if (cavas == null)
        {
            cavas = GameObject.Find("Canvas (1)").GetComponent<Canvas>();
        }
        if (cavas == null)
        {
            GameObject canvasObj = new GameObject("Canvas(clone)");
            cavas = canvasObj.AddComponent<Canvas>();
        }

        // fadeImage����
        fadeImage = Instantiate(fadeImagePrefab);
        fadeImage.transform.InitTransform(cavas.transform);
        
        // ȭ�� �� ���� 
        RectTransform rectTransform = fadeImage.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
    }

    public void FadeIn() 
    {
        fadeImage.DOFade(0f, duration)
                .SetDelay(0.5f)
                .SetEase(easeType)
                .OnComplete(() => {
                    Debug.Log("���̵��� �Ϸ�!");
                    Destroy(fadeImage.gameObject);
                });
    }

    public void FadeOut()
    {
        fadeImage.gameObject.SetActive(true);

        fadeImage.DOFade(1f, duration)
                .SetDelay(0.5f)
                .SetEase(easeType)
                .OnComplete(() => {
                    Debug.Log("���̵�ƿ� �Ϸ�!");
                });
    }
}
