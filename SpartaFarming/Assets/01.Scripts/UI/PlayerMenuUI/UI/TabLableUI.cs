using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabLableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite hoverImage;
    [SerializeField] private int tabIndex;

    private PlayerMenuUI PlayerMenuUI;

    private void Awake()
    {
        PlayerMenuUI = GetComponentInParent<PlayerMenuUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PlayerMenuUI.isTransition || PlayerMenuUI.CurrentTab == tabIndex) return;

        image.sprite = hoverImage;
        image.SetNativeSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(PlayerMenuUI.isTransition || PlayerMenuUI.CurrentTab == tabIndex) return;

        image.sprite = defaultImage;
        image.SetNativeSize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayerMenuUI.isTransition || PlayerMenuUI.CurrentTab == tabIndex) return;

        image.sprite = defaultImage;
        image.SetNativeSize();
        PlayerMenuUI.ChangeTab(tabIndex);
    }
}
