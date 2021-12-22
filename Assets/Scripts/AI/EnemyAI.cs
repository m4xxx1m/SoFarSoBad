using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector2 targetPoint1;
    private Vector2 targetPoint2;

    private Transform playerTransform;

    private Direction direction;

    private EnemyControler controler;

    private float timeFromLastShooting = 0f;

    void Start()
    {
        controler = new EnemyControler(transform);
        targetPoint1 = transform.position;
        targetPoint2 = GenerateNewTargetPoint(targetPoint1);
        direction = Direction.ToPoint2;
    }

    void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.WalkingAround:
                {
                    if (direction == Direction.ToPoint2)
                    {
                        if (!transform.position.Equals(targetPoint2))
                        {
                            controler.Move(targetPoint2, speed);
                        }
                        else
                        {
                            StartCoroutine(ChangeDirection(Direction.ToPoint1));
                        }
                    }
                    else if (direction == Direction.ToPoint1)
                    {
                        if (!transform.position.Equals(targetPoint1))
                        {
                            controler.Move(targetPoint1, speed);
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
                    timeFromLastShooting += Time.fixedDeltaTime;
                    if(timeFromLastShooting >= secondsBetweenShooting)
                    {
                        Shout(playerTransform.position);
                        timeFromLastShooting = 0f;
                    }
                    controler.Move(playerTransform.position, speed);
                    break;
                }
        }
    }

     private IEnumerator ChangeDirection(Direction direction)
    {
        yield return new WaitForSeconds(awaitingTimeSeconds);
        this.direction = direction;
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
        if(collision.gameObject.tag == "Player")
        {
            if (isAggressive)
            {
                enemyState = EnemyState.WalkingToPlayer;
                playerTransform = collision.gameObject.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyState = EnemyState.WalkingAround;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TileMap")
        {
            enemyState = EnemyState.Chill;
        }
    }

    private void Shout(Vector2 targetPoint)
    {
        Debug.Log("Shoot!");
        Vector3 perpendicular = Vector3.Cross(transform.position - new Vector3(targetPoint.x, targetPoint.y, 0), transform.forward);
        Quaternion rotation = Quaternion.LookRotation(transform.forward, perpendicular);
        GameObject shot = Instantiate(bullet, transform.position, rotation);
    }
}

public enum EnemyState
{
    Chill = 0,
    WalkingAround = 1,
    WalkingToPlayer = 2
}

public enum Direction
{
    ToPoint1 = 0,
    ToPoint2 = 1
}