using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public AudioMixerGroup SoundsMixer;

    [SerializeField] protected Camera camera;
    [SerializeField] protected Transform leftPoint;
    [SerializeField] protected Transform rightPoint;
    [SerializeField] protected Transform endBackgroundPoint;

    protected float startBackgroundWidth;
    [SerializeField] protected RectTransform background;

    void Start()
    {
        startBackgroundWidth = background.rect.width;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public virtual void OnDrag(PointerEventData data)
    {
        Vector2 touchPosWorld = camera.ScreenToWorldPoint(new Vector3(data.position.x, data.position.y, camera.nearClipPlane));
        Vector2 targetPosition = FitScrollerPosition(touchPosWorld);
        transform.position = targetPosition;
        float targetVolume = GetTargetVolume(targetPosition);
        SoundsMixer.audioMixer.SetFloat("MasterVolume", targetVolume);

        float targetBackgroundSize = GetTargetBackgroundSize(targetPosition);
        background.sizeDelta = new Vector2(Mathf.Lerp(0, startBackgroundWidth, targetBackgroundSize), background.rect.height);
    }


    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public Vector2 FitScrollerPosition(Vector2 position)
    {
        position.y = transform.position.y;
        if (position.x < leftPoint.position.x) position.x = leftPoint.position.x;
        if (position.x > rightPoint.position.x) position.x = rightPoint.position.x;
        return position;
    }

    public float GetTargetVolume(Vector2 position)
    {
        float size = ((rightPoint.position.x - leftPoint.position.x) / 100) * 120;
        float deltaX = position.x - leftPoint.position.x;
        return (deltaX / size) * 100 - 80;
    }

    public float GetTargetBackgroundSize(Vector2 position)
    {
        float size = rightPoint.position.x - leftPoint.position.x;
        float deltaX = position.x - leftPoint.position.x;
        return deltaX / size;
    }
}
