using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.timeScale > 0f)
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3 perpendicular = Vector3.Cross(transform.position - mousePos, transform.forward);
            transform.rotation = Quaternion.LookRotation(transform.forward, perpendicular);
        }
    }
}
