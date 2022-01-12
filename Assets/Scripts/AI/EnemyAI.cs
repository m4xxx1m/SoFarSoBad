using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] bool isAggressive;

    [SerializeField] private EnemyState enemyState;
    [SerializeField] private float speed;
    [SerializeField] private int awaitingTimeSeconds = 0;
    [SerializeField] private float generatingIntervalStart;
    [SerializeField] private float generatingIntervalEnd;
    [SerializeField] private float secondsBetweenShooting;
    [SerializeField] private GameObject bullet;

    private GameObject tilemapGameObject;
    private Tilemap tilemap;

    private Vector2 targetPoint;

    private Transform playerTransform;

    private Direction direction;

    private EnemyControler controler;

    private float timeFromLastShooting = 0f;

    private bool havePlayerAround = false;

    float circleColliderRadius = 16.54f;

    List<Vertex> pathToSecondPoint;
    int posInSecondPointPath = 0;
    List<Vertex> pathToPlayer;
    int posInPlayerPath = 0;
    bool b = true;

    Vertex playerVertex;
    Vertex enemyVertex;

    Vertex firstPointVertex;
    Vertex secondPointVertex;

    [SerializeField] float timeToUpdatePathToPlayer;
    private float time = 0f;

    void Start()
    {
        controler = new EnemyControler(transform);
        direction = Direction.ToPoint2;
        if (GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag).Length > 0)
        {
            tilemapGameObject = GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag)[0];
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }

        // Todo: getting size of circle collider
        tilemap.CompressBounds();
        GeneratePathToNewSecondPoint();
    }

    void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.WalkingAround:
                {
                    if (pathToSecondPoint[pathToSecondPoint.Count - 1].coord.Equals(new Vector3(0, 0, 0))) break;
                    if (posInSecondPointPath >= pathToSecondPoint.Count - 1) posInSecondPointPath = pathToSecondPoint.Count - 1;
                    if (posInSecondPointPath < 0) posInSecondPointPath = 0;
                    if (direction == Direction.ToPoint2)
                    {
                        if (!isApproximatelyNear(transform.position, pathToSecondPoint[pathToSecondPoint.Count-1].coord, 0.1f))
                        {
                            controler.Move(pathToSecondPoint[posInSecondPointPath].coord, speed);
                            if (isApproximatelyNear(transform.position, pathToSecondPoint[posInSecondPointPath].coord, 0.1f)) posInSecondPointPath++;
                        }
                        else
                        {
                            StartCoroutine(ChangeDirection(Direction.ToPoint1));
                        }
                    }
                    else if (direction == Direction.ToPoint1)
                    {
                        if (!isApproximatelyNear(transform.position, pathToSecondPoint[0].coord, 0.1f))
                        {
                            controler.Move(pathToSecondPoint[posInSecondPointPath].coord, speed);
                            if (isApproximatelyNear(transform.position, pathToSecondPoint[posInSecondPointPath].coord, 0.1f)) posInSecondPointPath--;
                        }
                        else
                        {
                            StartCoroutine(ChangeDirection(Direction.ToPoint2));
                        }
                    }
                    break;
                }
            case EnemyState.WalkingToPlayer:
                {
                    if (b || time >= timeToUpdatePathToPlayer)
                    {
                        List<Vertex> vertices = CollectVertices(transform.position, playerTransform.position, out enemyVertex, out playerVertex);

                        if (enemyVertex != null)
                        {
                            Flex228(enemyVertex, vertices);
                        }

                        pathToPlayer = MakePath(playerVertex, enemyVertex);
                        posInPlayerPath = 0;

                        time = 0;
                        b = false;
                    }

                    timeFromLastShooting += Time.fixedDeltaTime;
                    if (timeFromLastShooting >= secondsBetweenShooting)
                    {
                        Shout(playerTransform.position);
                        timeFromLastShooting = 0f;
                    }
                    if(pathToPlayer.Count == 0)
                    {
                        Debug.Log("Path to player is null!");
                        break;
                    }
                    if (posInPlayerPath >= pathToPlayer.Count - 1)
                    {
                        Debug.Log("Enemy is around player!");
                        break;
                    }
                    controler.Move(pathToPlayer[posInPlayerPath].coord, speed * 1.5f);
                    if(isApproximatelyNear(transform.position, pathToPlayer[posInPlayerPath].coord, 0.1f)) posInPlayerPath++;
                    break;
                }
            case EnemyState.WalkingToPoint:
                {
                    if (!transform.position.Equals(targetPoint))
                    {
                        controler.Move(targetPoint, speed);
                    }
                    else if(havePlayerAround)
                    {
                        enemyState = EnemyState.WalkingToPlayer;
                    }
                    break;
                }
        }
        time += Time.fixedDeltaTime;
    }

    private List<Vertex> CollectVertices(Vector3 startVertexPosition, Vector3 endVertexPosition, out Vertex startVertex, out Vertex endVertex)
    {
        List<Vertex> vertices = new List<Vertex>();
        List<Vector3> foundTiles = new List<Vector3>();
        List<Vertex> foundVertex = new List<Vertex>();
        List<Vector3> foundVertexCoords = new List<Vector3>();
        startVertex = new Vertex();
        endVertex = new Vertex();
        for (float i = transform.position.y + circleColliderRadius / 2; i >= transform.position.y - circleColliderRadius / 2; i -= 1f)
        {
            for (float j = transform.position.x - circleColliderRadius / 2; j <= transform.position.x + circleColliderRadius / 2; j += 1f)
            {
                Vector3Int posInt = tilemap.WorldToCell(new Vector3(j, i, 0));
                Vector3 foundTilePosition = tilemap.GetCellCenterWorld(posInt);
                TileBase tile = tilemap.GetTile(posInt);
                if (!foundTiles.Contains(foundTilePosition))
                {
                    Vertex thisVertex = new Vertex();
                    thisVertex.coord = foundTilePosition;
                    if (foundVertexCoords.Contains(foundTilePosition))
                    {
                        foreach (Vertex vertex in foundVertex)
                        {
                            if (vertex.coord.Equals(foundTilePosition))
                            {
                                thisVertex = vertex;
                                break;
                            }
                        }
                    }
                    List<Link> vertexAround = new List<Link>();
                    int[] dx = { 1, -1, 0, 0 };
                    int[] dy = { 0, 0, -1, 1 };
                    int wallsCount = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        Vector3Int newPos = tilemap.WorldToCell(posInt + new Vector3Int(dx[k], dy[k], 0));
                        Vector3 tileAroundCoord = tilemap.GetCellCenterWorld(newPos);
                        TileBase tileAround = tilemap.GetTile(newPos);
                        if (tileAround != null && tileAround.name == GlobalFields.wallTileName) wallsCount++;
                        if (tileAround == null || tileAround.name == GlobalFields.floorTileName)
                        {
                            Vertex vertexToAdd = new Vertex();
                            vertexToAdd.coord = tileAroundCoord;
                            if (foundVertexCoords.Contains(tileAroundCoord))
                            {
                                foreach (Vertex vertex in foundVertex)
                                {
                                    if (vertex.coord.Equals(tileAroundCoord))
                                    {
                                        vertexToAdd = vertex;
                                        break;
                                    }
                                }
                                // враг избегает прохода по клеткам рядом со стенами
                                vertexAround.Add(new Link(vertexToAdd, 1f + vertexToAdd.wallAroundCount));
                            }
                            else
                            {
                                vertexAround.Add(new Link(vertexToAdd, 1f));
                                foundVertex.Add(vertexToAdd);
                                foundVertexCoords.Add(tileAroundCoord);
                            }
                        }
                    }
                    thisVertex.wallAroundCount = wallsCount;
                    foundTiles.Add(foundTilePosition);
                    thisVertex.links = vertexAround;
                    vertices.Add(thisVertex);
                    if (!foundVertexCoords.Contains(foundTilePosition))
                    {
                        foundVertex.Add(thisVertex);
                        foundVertexCoords.Add(thisVertex.coord);
                    }
                    if (isApproximatelyNear(startVertexPosition, new Vector3(j, i, 0), 0.5f))
                    {
                        startVertex = thisVertex;
                    }
                    if (isApproximatelyNear(endVertexPosition, new Vector3(j, i, 0), 0.5f))
                    {
                        endVertex = thisVertex;
                    }
                }
            }
        }
        return vertices;
    }

    // о да, это алгоритм Дейкстры, того самого знаменитого ковбоя
    private void Flex228(Vertex vertex, List<Vertex> vertices)
    {
        vertex.vizited = true;
        List<Link> list = vertex.links;
        float min = -1;
        Vertex nv = null;
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i].vertex.vizited) continue;
            if(list[i].vertex.d == -1 || vertex.d + list[i].dist < list[i].vertex.d)
            {
                list[i].vertex.d = vertex.d + list[i].dist;
                list[i].vertex.last = vertex;
            }
        }
        for(int i = 0; i < vertices.Count ; i++)
        {
            if (vertices[i].vizited || vertices[i].d == -1) continue;
            if(min == -1 || vertices[i].d < min)
            {
                min = vertices[i].d;
                nv = vertices[i];
            }
        }
        if(nv != null) Flex228(nv, vertices);
    }

    private List<Vertex> MakePath(Vertex playerVertex, Vertex enemyVertex)
    {
        List<Vertex> path = new List<Vertex>();
        path.Add(playerVertex);
        if (playerVertex.last != null) Debug.DrawLine(playerVertex.coord, playerVertex.last.coord, Color.yellow, timeToUpdatePathToPlayer);
        Vertex v = playerVertex.last;
        while (v != null && v != enemyVertex)
        {
            if (v.last != null) Debug.DrawLine(v.coord, v.last.coord, Color.yellow, timeToUpdatePathToPlayer);
            path.Add(v);
            v = v.last;
        }
        path.Add(enemyVertex);
        path.Reverse();
        return path;
    }

    private bool isApproximatelyNear(Vector3 v1, Vector3 v2, float infelicity)
    {
        return v1.x + infelicity >= v2.x && v1.x - infelicity <= v2.x && v1.y + infelicity >= v2.y && v1.y - infelicity <= v2.y && v1.z + infelicity >= v2.z && v1.z - infelicity <= v2.z;
    }

    private IEnumerator ChangeDirection(Direction direction)
    {
        yield return new WaitForSeconds(awaitingTimeSeconds);
        this.direction = direction;
    }

    private void GeneratePathToNewSecondPoint()
    {
        int n = 0;
        Vector3 randSecondPoint;
        TileBase tile;
        do
        {
            randSecondPoint = new Vector3(Random.Range(transform.position.x - circleColliderRadius / 2, transform.position.x + circleColliderRadius / 2), Random.Range(transform.position.y - circleColliderRadius / 2, transform.position.y + circleColliderRadius / 2), 0);
            tile = tilemap.GetTile(tilemap.WorldToCell(randSecondPoint));
            n++;
        }
        while (tile != null && tile.name != GlobalFields.floorTileName && n < 100);
        List < Vertex > vertices = CollectVertices(transform.position, randSecondPoint, out firstPointVertex, out secondPointVertex);
        if (firstPointVertex != null)
        {
            Flex228(firstPointVertex, vertices);
        }
        pathToSecondPoint = MakePath(secondPointVertex, firstPointVertex);
        posInSecondPointPath = 0;
    }

    private Vector2 GenerateNewTargetPoint(Vector2 targetPoint)
    {
        Vector2 result;
        result = new Vector2();
        int dest = Random.Range(1, 4); // UP, RIGHT, DOWN, LEFT
        switch(dest)
        {
            case 1:
                {
                    result.y = targetPoint.y + Random.Range(generatingIntervalStart, generatingIntervalEnd);
                    result.x = targetPoint.x;
                    break;
                }
            case 2:
                {
                    result.y = targetPoint.y;
                    result.x = targetPoint.x + Random.Range(generatingIntervalStart, generatingIntervalEnd);
                    break;
                }
            case 3:
                {
                    result.y = targetPoint.y - Random.Range(generatingIntervalStart, generatingIntervalEnd);
                    result.x = targetPoint.x;
                    break;
                }
            case 4:
                {
                    result.y = targetPoint.y;
                    result.x = targetPoint.x - Random.Range(generatingIntervalStart, generatingIntervalEnd);
                    break;
                }
        }
        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == GlobalFields.playerTag)
        {
            b = true;
            havePlayerAround = true;
            if (isAggressive)
            {
                enemyState = EnemyState.WalkingToPlayer;
                playerTransform = collision.gameObject.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GlobalFields.playerTag)
        {
            havePlayerAround = false;
            enemyState = EnemyState.WalkingAround;
        }
    }

/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GlobalFields.tilemapTag)
        {
            if (tilemap != null && tilemapGameObject == collision.gameObject)
            {
                Vector3 hitPosition = Vector3.zero;
                foreach (ContactPoint2D hit in collision.contacts)
                {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                    Vector3Int vector = tilemap.WorldToCell(hitPosition);
                    Vector3 deltaVector = tilemap.GetCellCenterWorld(vector);
                    targetPoint = transform.position - (deltaVector - transform.position);
                    enemyState = EnemyState.WalkingToPoint;
                }
            }
        }
    }*/

    private void Shout(Vector2 targetPoint)
    {
        Debug.Log("Shoot!");
        Vector3 perpendicular = Vector3.Cross(transform.position - new Vector3(targetPoint.x, targetPoint.y, 0), transform.forward);
        Quaternion rotation = Quaternion.LookRotation(transform.forward, perpendicular);
        float missedBulletRotation = rotation.eulerAngles.z + Random.Range(-20f, 20f);
        rotation = Quaternion.Euler(0f, 0f, missedBulletRotation);
        GameObject shot = Instantiate(bullet, transform.position, rotation);
    }
}

class Vertex
{
    public Vector3 coord;
    public List<Link> links;
    public bool vizited;
    public float d;
    public Vertex last;
    public int wallAroundCount;

    public Vertex()
    {
        this.coord = new Vector3();
        this.links = new List<Link>();
        this.vizited = false;
        d = -1;
        wallAroundCount = 0;
    }
}

class Link
{
    public Vertex vertex;
    public float dist;

    public Link()
    {
        this.vertex = new Vertex();
        this.dist = 0f;
    }

    public Link(Vertex vertex, float dist)
    {
        this.vertex = vertex;
        this.dist = dist;
    }
}

public enum EnemyState
{
    Chill = 0,
    WalkingAround = 1,
    WalkingToPlayer = 2,
    WalkingToPoint = 3
}

public enum Direction
{
    ToPoint1 = 0,
    ToPoint2 = 1
}