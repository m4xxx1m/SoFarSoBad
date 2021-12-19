using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAtPoint : MovingStrategy
{
    private Vector2 targetPoint;
    private bool isMoving = false;
    private float speed;

    public MovingAtPoint(Vector2 targetPoint, float speed)
    {
        this.targetPoint = targetPoint;
        this.speed = speed;
    }

    public override void Move(Transform transform)
    {
        isMoving = true;
    }

    private void Update()
    {
        if(isMoving)
        {
            enemyTransform.position = Vector3.MoveTowards(transform.position, targetPoint, speed);
            if(enemyTransform.position.x == targetPoint.x && enemyTransform.position.y == targetPoint.y)
            {
                isMoving = false;
            }    
        }
    }
}
