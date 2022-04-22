using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressedAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Animator animator;
    private RectTransform buttonRect;
    private Vector2 startRect;

    void Start()
    {
        animator = GetComponent<Animator>();
        buttonRect = GetComponent<RectTransform>();
        startRect = buttonRect.sizeDelta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonRect.sizeDelta = new Vector2(startRect.x, startRect.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonRect.sizeDelta = new Vector2(startRect.x * 0.95f, startRect.y * 0.95f);
    }
}