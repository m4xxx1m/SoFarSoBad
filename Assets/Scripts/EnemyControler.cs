using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler
{
    private Transform enemyTransform { get; set; }
    private ShoutStrategy shoutStrategy { get; set; }
    private MovingStrategy movingStrategy { get; set; }
    private float speed { get; set; }

    public EnemyControler(Transform enemyTransform)
    {
        this.enemyTransform = enemyTransform;
        shoutStrategy = new NoShout();
        movingStrategy = new NoMoving();
    }

    public void Shout()
    {
        shoutStrategy = new NoShout();
        shoutStrategy.Shout();
    }

    public void Shout(Vector2 targetPoint)
    {
        shoutStrategy = new ShoutAtPoint(targetPoint);
        shoutStrategy.Shout();
    }

    public void Move()
    {
        movingStrategy = new NoMoving();
        movingStrategy.Move(enemyTransform);
    }

    public void Move(Vector2 targetPoint)
    {
        movingStrategy = new MovingAtPoint(targetPoint, 0.1f);
        movingStrategy.Move(enemyTransform);
    }

/*    public void Move(Vector2 targetPoint1, Vector2 targetPoint2)
    {
        movingStrategy = new MovingBetweenPoints(targetPoint1, targetPoint2);
        movingStrategy.Move(enemyTransform);
    }*/
}
