using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TronedAI : MonoBehaviour
{
    [SerializeField] private int minTimeBetweenShoutAround;
    [SerializeField] private int maxTimeBetweenShoutAround;
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float constantTimeBetweenShooting;
    [SerializeField] private float deltaTimeBetweenShooting;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject player;
    private ShoutState shoutState = ShoutState.AtPlayer;
    private float radius = 20.46f;

    private float time = 0f;

    void Start()
    {
        timeBetweenShooting = constantTimeBetweenShooting;
        StartCoroutine(ChangeStateToShoutAround());
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        switch (shoutState)
        {
            case ShoutState.AtPlayer:
                {
                    if (time > timeBetweenShooting)
                    {
                        Shout(player.transform.position);
                        time = 0f;
                        timeBetweenShooting = constantTimeBetweenShooting + Random.Range(-deltaTimeBetweenShooting,
                            deltaTimeBetweenShooting);
                    }
                    break;
                }
            case ShoutState.Around:
            {
                    for (int i = 0; i < 360; i += Random.Range(6, 24))
                {
                    float dx = radius * Mathf.Cos(i * Mathf.Deg2Rad);
                    float dy = radius * Mathf.Sin(i * Mathf.Deg2Rad);
                    Vector2 targetPoint = new Vector2(transform.position.x + dx, transform.position.y + dy);
                    Shout(targetPoint);
                }
                shoutState = ShoutState.AtPlayer;
                StartCoroutine(ChangeStateToShoutAround());
                break;
            }
        }
    }

    private IEnumerator ChangeStateToShoutAround()
    {
        int timeDelay = Random.Range(minTimeBetweenShoutAround, maxTimeBetweenShoutAround);
        yield return new WaitForSeconds(timeDelay);
        shoutState = ShoutState.Around;
    }

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

enum ShoutState
{
    AtPlayer = 0,
    Around = 1
}
