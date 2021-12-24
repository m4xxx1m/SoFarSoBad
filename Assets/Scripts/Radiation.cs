using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

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

    private string wallTileName = GlobalFields.wallTileName;
    private string borderTileName = GlobalFields.borderTileName;

    [SerializeField] private int BorderTilesCoeff = 3;

    [SerializeField] private bool isCoroutineStarted = false;

    private void Start()
    {
        tilemapGameObject = GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag)[0];
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
            case GlobalFields.playerTag:
                {
                    timeFromLastRadiationDamage += Time.deltaTime;
                    if (timeFromLastRadiationDamage < timeBeforeRadiationDamage) return;
                    else timeFromLastRadiationDamage = 0f;
                    GameObject playerGameObject = collision.gameObject;
                    Vector3 direction = playerGameObject.transform.position - transform.position;
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, circleColliderRadius);
                    Debug.DrawRay(transform.position, direction);
                    int hitsCount = 0; // количество пересечений со стенами
                    float distance = 0f; // дистанция до игрока
                    foreach (RaycastHit2D hit in hits)
                    {
                        Collider2D collider = hit.collider;
                        if (collider.gameObject.tag == GlobalFields.playerTag)
                        {
                            distance = hit.distance;
                            break;
                        }
                    }
                    foreach (RaycastHit2D hit in hits)
                    {
                        Collider2D collider = hit.collider;
                        Vector3 hitPosition = Vector3.zero;
                        if (collider.gameObject == tilemapGameObject && hit.distance < distance)
                        {
                            List<Vector3> foundTiles = new List<Vector3>();
                            Debug.DrawRay(hit.point, direction, Color.red, 6f);
                            for (float a = 0f; a <= 1f; a+=0.05f)
                            {
                                Vector2 vector = Vector2.Lerp(hit.point, playerGameObject.transform.position, a);
                                //hit.normal.Set(hit.normal.x + deltaX, hit.normal.y + deltaY);
                                hitPosition.x = vector.x;
                                hitPosition.y = vector.y;
                                TileBase tile = tilemap.GetTile(tilemap.WorldToCell(hitPosition));
                                Vector3 cellCenter = tilemap.GetCellCenterWorld(tilemap.WorldToCell(hitPosition));
                                if (tile == null) continue;
                                if (tile.name == wallTileName && !foundTiles.Contains(cellCenter))
                                {
                                    Debug.Log(cellCenter);
                                    foundTiles.Add(cellCenter);
                                    hitsCount++;
                                }
                                else if (tile.name == borderTileName && !foundTiles.Contains(cellCenter))
                                {
                                    Debug.Log(cellCenter);
                                    foundTiles.Add(cellCenter);
                                    hitsCount += BorderTilesCoeff;
                                }
                            }
                        }
                    }
                    Debug.Log(this.name + " " + "Distance to player: " + distance);

                    // Todo: вот сюда вставляешь все свои формулки для коэффицинтов
                    float k1 = Mathf.Pow(0.5f, hitsCount);
                    float k2 = 1f - distance / circleColliderRadius;
                    if(distance == 0) k2 = 0;
                    float radiationForPlayer = deltaRadiation * k1 * k2;
                    Debug.Log($"{hitsCount}, {distance}, {deltaRadiation}, {radiationForPlayer}");
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
            case GlobalFields.vrudniTag:
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
            case GlobalFields.vrudniTag:
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
            case GlobalFields.playerTag:
                Entity entity = collision.gameObject.GetComponent<Entity>();
                entity.isInRadiation = false;
                StartCoroutine(entity.NullRadiationAfterSomeSeconds());
                break;
        }
    }
}
