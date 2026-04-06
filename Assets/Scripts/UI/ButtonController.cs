using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonController : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public Sprite normalSprite;
    public Sprite hoverSprite;
    public Sprite pressedSprite;

    private Image image;
    private bool isPointerInside;

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        image.sprite = normalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = isPointerInside ? hoverSprite : normalSprite;
    }
}