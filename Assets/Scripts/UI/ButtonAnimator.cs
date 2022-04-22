using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool("Released", true);
        animator.SetBool("Pressed", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("Pressed", true);
        animator.SetBool("Released", false);
    }
}
