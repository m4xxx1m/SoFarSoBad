using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    private Rigidbody2D rb;
    private Vector2     dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void SetDirection(Vector2 _dir)
    {
        dir = _dir;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameObject shot = Instantiate(bullet, transform.position, Quaternion.identity);
            shot.SendMessage("SetDirection", dir, SendMessageOptions.DontRequireReceiver);
        }
    }
}
