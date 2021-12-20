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

    private void Start()
    {
        if(GameObject.FindGameObjectsWithTag("TileMap").Length > 0)
        {
            tilemapGameObject = GameObject.FindGameObjectsWithTag("TileMap")[0];
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
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
