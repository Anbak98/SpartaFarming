using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite hoverImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverImage;
        image.SetNativeSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = defaultImage;
    }
}
