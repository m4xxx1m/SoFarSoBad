using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private EnemyState enemyState;
    private EnemyControler controler;
    void Start()
    {
        controler = new EnemyControler(transform);
        switch (enemyState)
        {
            case EnemyState.WalkingAround:
                {
                    controler.Move(new Vector2(transform.position.x + 5f, transform.position.y + 2f));
                    break;
                }
        }
    }

    void Update()
    {

    }
}

public enum EnemyState
{
    Chill = 0,
    WalkingAround = 1,
    WalkingToPlayer = 2
}
