using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform follow;
    
    private void LateUpdate()
    {
        this.transform.position = new Vector3(follow.position.x, follow.position.y, transform.position.z);
    }
}
