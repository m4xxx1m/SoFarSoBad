using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Tile floorTile;
    private GameObject tilemapGameObject;
    private Tilemap tilemap;

    [SerializeField] private float      speed    = 10f;
    [SerializeField] private float      damage   = 1f;

    [SerializeField] private Transform  sprite;
    
    [SerializeField] private float      lifetime = 1f;
    private float lifeTimer = 0f;

    private Rigidbody2D rb;
    private Vector2     dir;

    private void Start()
    {
        tilemapGameObject = GameObject.FindGameObjectsWithTag("TileMap")[0];
        if (tilemapGameObject != null)
        {
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }
    }

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
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && tilemapGameObject == _other.gameObject)
        {
            foreach (ContactPoint2D hit in _other.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), floorTile);
            }
        }
        Destroy(gameObject);
    }
}
