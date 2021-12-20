using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Radiation : MonoBehaviour
{
    [SerializeField] private int timeForDisable;
    [SerializeField] private bool haveRadiation;
    [SerializeField] private float circleColliderRadius;
    [SerializeField] private float deltaRadiation;
    [SerializeField] private float timeBeforeRadiationDamage;

    private GameObject tilemapGameObject;
    private Tilemap tilemap;
    private float timeFromLastRadiationDamage = 0f;

    private string wallTileName = "grey_tile";
    private string borderTileName = "black_tile";

    [SerializeField] private int BorderTilesCoeff = 3;

    [SerializeField] private bool isCoroutineStarted = false;

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
        isCoroutineStarted = true;
        yield return new WaitForSeconds(timeForDisable);
        this.haveRadiation = false;
        isCoroutineStarted = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!haveRadiation) return;
        switch(collision.gameObject.tag)
        {
            case "Player":
                {
                    timeFromLastRadiationDamage += Time.deltaTime;
                    if (timeFromLastRadiationDamage < timeBeforeRadiationDamage) return;
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
                            if (tile.name == wallTileName)
                            {
                                hitsCount++;
                            }
                            else if (tile.name == borderTileName)
                            {
                                hitsCount += BorderTilesCoeff;
                            }
                        }
                        else if (collider.gameObject.tag == "Player")
                        {
                            distance = hit.distance;
                            break;
                            // Если луч нормально идет от объекта радиации к игроку, здесь нужен break
                        }
                    }
                    Debug.Log(this.name + " " + "Distance to player: " + distance);
                    
                    // Todo: вот сюда вставляешь все свои формулки для коэффицинтов
                    float k1 = 1f;
                    for (int i = 0; i < hitsCount; ++i)
                    {
                        k1 *= 0.8f;
                    }
                    float k2 = distance / circleColliderRadius;
                    float radiationForPlayer = deltaRadiation * k1 * k2;
                    Debug.Log($"{hitsCount}, {distance}, {deltaRadiation}");
                    // в принципе здесь твоя часть заканчивается

                    Entity entity = playerGameObject.GetComponent<Entity>();
                    //entity.RadiationLevel += radiationForPlayer;
                    entity.AddRadiation(radiationForPlayer);
                    entity.isInRadiation = true;
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
                        //StartCoroutine(radiation.DisableAfterSomeSeconds());
                    }
                    break;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!haveRadiation) return;
        //if (!(collision is CapsuleCollider2D)) return;
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                GameObject enemyGameObject = collision.gameObject;
                Radiation radiation = enemyGameObject.GetComponent<Radiation>();
                if (!radiation.haveRadiation)
                {
                    radiation.haveRadiation = true;
                }
                if (!radiation.isCoroutineStarted)
                {
                    StartCoroutine(radiation.DisableAfterSomeSeconds());
                }
                break;
            case "Player":
                Entity entity = collision.gameObject.GetComponent<Entity>();
                entity.isInRadiation = false;
                StartCoroutine(entity.NullRadiationAfterSomeSeconds());
                break;
        }
    }
}
