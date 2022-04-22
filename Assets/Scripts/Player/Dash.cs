using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Dash : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Movement person;
    [SerializeField] private float dashSpeed = 20.0f;
    [SerializeField] private float timeToWait = 1f;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!button.interactable) return;
        person.dash = true;
        person.dashTimer = 0f;
        button.interactable = false;
        StartCoroutine(EnableDashAfterSomeSeconds());
    }

    private IEnumerator EnableDashAfterSomeSeconds()
    {
        yield return new WaitForSeconds(timeToWait);
        button.interactable = true;
    }
}
