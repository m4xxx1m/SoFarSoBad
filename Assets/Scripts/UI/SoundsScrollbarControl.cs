using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundsScrollbarControl : ScrollControl
{
    public override void OnDrag(PointerEventData data)
    {
        Vector2 touchPosWorld = camera.ScreenToWorldPoint(new Vector3(data.position.x, data.position.y, camera.nearClipPlane));
        Vector2 targetPosition = FitScrollerPosition(touchPosWorld);
        transform.position = targetPosition;
        float targetVolume = GetTargetVolume(targetPosition);
        SoundsMixer.audioMixer.SetFloat("SoundsVolume", targetVolume);

        float targetBackgroundSize = GetTargetBackgroundSize(targetPosition);
        background.sizeDelta = new Vector2(Mathf.Lerp(0, startBackgroundWidth, targetBackgroundSize), background.rect.height);
    }
}
