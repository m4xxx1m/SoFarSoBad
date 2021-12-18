using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float      speed    = 10f;
    [SerializeField] private float      damage   = 1f;

    [SerializeField] private Transform  sprite;
    
    [SerializeField] private float      lifetime = 1f;
    private float lifeTimer = 0f;

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
        sprite.Rotate(new Vector3(0, 0, 10f));

        if(dir.x != 0)
            sprite.localPosition = new Vector2(0, Mathf.Sin(Time.time * 3) / 4);
        else
            sprite.localPosition = new Vector2(Mathf.Sin(Time.time * 3) / 4, 0);
    }

    private void FixedUpdate()
    {
        rb.velocity = dir * speed;
        lifeTimer += Time.fixedDeltaTime;
        if(lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        _other.gameObject.SendMessage("ReduceHealth", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
