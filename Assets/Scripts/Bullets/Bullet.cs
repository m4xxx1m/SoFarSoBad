using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Tile floorTile;
    private GameObject tilemapGameObject;
    private Tilemap tilemap;

    [SerializeField] private float speed    = 10f;
    [SerializeField] private float damage   = 1f;
    
    public float lifetime = 1f;
    private float lifeTimer = 0f;

    private Rigidbody2D rb;
    
    [SerializeField] private string wallTileName = GlobalFields.wallTileName;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystem particles2;
    [SerializeField] private GameObject lighting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if(GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag).Length > 0)
        {
            tilemapGameObject = GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag)[0];
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }
    }

    private void FixedUpdate()
    {
        if(!rb.isKinematic)
        {
            rb.velocity = transform.right * speed;
            lifeTimer += Time.fixedDeltaTime;
            
            if(lifeTimer >= lifetime)
            {
                Explode();
            }
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
                Vector3Int vector = tilemap.WorldToCell(hitPosition);
                TileBase tile = tilemap.GetTile(vector);
                if (tile != null && tile.name == wallTileName)
                {
                    tilemap.SetTile(vector, floorTile);
                }
            }
        }

        Explode();
    }

    private void Explode()
    {
        rb.isKinematic = true;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        rb.velocity = Vector2.zero;

        lighting.SetActive(false);
        particles.Play();
        particles2.Play();
        Destroy(gameObject, particles.main.duration);
    }
}
