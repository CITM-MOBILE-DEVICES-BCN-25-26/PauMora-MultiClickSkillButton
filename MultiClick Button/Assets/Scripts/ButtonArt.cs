using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ButtonArt : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

	[SerializeField] Color normalColor;
	[SerializeField] Color hoverColor;
	[SerializeField] Color pressedColor;

	[SerializeField] Image image;

	private void Start()
	{
		ChangeImageColor(normalColor);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ChangeImageColor(hoverColor);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ChangeImageColor(normalColor);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		ChangeImageColor(pressedColor);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		ChangeImageColor(normalColor);
	}

	private void ChangeImageColor(Color color)
	{
		image.color = color;
	}
}
