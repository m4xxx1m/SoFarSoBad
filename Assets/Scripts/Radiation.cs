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

    [SerializeField] private string wallTileName = "grey_tile";
    [SerializeField] private string borderTileName = "black_tile";

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
                    int hitsCount = 0; // ���������� ����������� �� �������
                    float distance = 0f; // ��������� �� ������
                    foreach (RaycastHit2D hit in hits)
                    {
                        Collider2D collider = hit.collider;
                        if (collider.gameObject.tag == "Player")
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
                    }
                    Debug.Log(this.name + " " + "Distance to player: " + distance);
                    
                    // Todo: ��� ���� ���������� ��� ���� �������� ��� ������������
                    float k1 = hitsCount;
                    float k2 = distance;
                    float radiationForPlayer = deltaRadiation * k1 * k2;
                    Debug.Log($"{hitsCount}, {distance}, {deltaRadiation}");
                    // � �������� ����� ���� ����� �������������

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
            case "Vrudni":
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
            case "Vrudni":
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
