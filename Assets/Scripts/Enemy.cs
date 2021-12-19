using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float speedMin = 0.00002f;
    [SerializeField] private float speedMax = 1f;

    [SerializeField] private float damage = 1f;

    private float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(speedMin, speedMax);
    }

    private void FixedUpdate()
    {
        //rb.MovePosition(Vector2.MoveTowards(rb.position, target.position, speed));
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        _other.gameObject.SendMessage("ReduceHealth", damage, SendMessageOptions.DontRequireReceiver);
    }
}
