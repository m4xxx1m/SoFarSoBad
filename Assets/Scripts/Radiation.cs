using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Radiation : MonoBehaviour
{
    [SerializeField] private int secondsForDisable;
    [SerializeField] private bool haveRadiation;
    [SerializeField] private float circleColliderRadius;
    [SerializeField] private float deltaRadiation;
    [SerializeField] private float secondsPerDamage;

    private GameObject tilemapGameObject;
    private Tilemap tilemap;
    private float timeFromLastRadiationDamage = 0f;

    private void Start()
    {
        tilemapGameObject = GameObject.FindGameObjectsWithTag("TileMap")[0];
        if (tilemapGameObject != null)
        {
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }
    }

    public IEnumerator DisableAfterSomeSeconds()
    {
        yield return new WaitForSeconds(secondsForDisable);
        this.haveRadiation = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!haveRadiation) return;
        switch(collision.gameObject.tag)
        {
            case "Player":
                {
                    timeFromLastRadiationDamage += Time.deltaTime;
                    if (timeFromLastRadiationDamage < secondsPerDamage) return;
                    else timeFromLastRadiationDamage = 0f;
                    GameObject playerGameObject = collision.gameObject;
                    Vector2 direction = (playerGameObject.transform.position - transform.position).normalized;
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, circleColliderRadius);
                    int hitsCount = 0; // количество пересечений со стенами
                    float distance = 0f; // дистанция до игрока
                    foreach (RaycastHit2D hit in hits)
                    {
                        Collider2D collider = hit.collider;
                        Vector3 hitPosition = Vector3.zero;
                        if (collider.gameObject == tilemapGameObject)
                        {
                            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                            TileBase tile = tilemap.GetTile(tilemap.WorldToCell(hitPosition));
                            if(tile.name == "grey_tile")
                            {
                                hitsCount++;
                            }
                        }
                        else if (collider.gameObject.tag == "Player") distance = hit.distance;
                    }
                    Debug.Log(this.name + " " + "Distance to player: " + distance);
                    
                    // Todo: вот сюда вставляешь все свои формулки для коэффицинтов
                    float k1 = hitsCount;
                    float k2 = distance;
                    float radiationForPlayer = deltaRadiation * k1 * k2;
                    // в принципе здесь твоя часть заканчивается

                    Entity entity = playerGameObject.GetComponent<Entity>();
                    entity.RadiationLevel += radiationForPlayer;
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!haveRadiation) return;
        if (!(collision is CapsuleCollider2D)) return;
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                {
                    GameObject enemyGameObject = collision.gameObject;
                    Radiation radiation = enemyGameObject.GetComponent<Radiation>();
                    if (!radiation.haveRadiation)
                    {
                        radiation.haveRadiation = true;
                        StartCoroutine(radiation.DisableAfterSomeSeconds());
                    }
                    break;
                }
        }
    }
}
