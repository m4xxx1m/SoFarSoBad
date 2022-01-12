using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler
{
    private Transform enemyTransform { get; set; }
    private ShoutStrategy shoutStrategy { get; set; }
    private MovingStrategy movingStrategy { get; set; }
    private float speed { get; set; }

    private Rigidbody2D rigidbody;

    public EnemyControler(Transform enemyTransform)
    {
        this.enemyTransform = enemyTransform;
        rigidbody = enemyTransform.gameObject.GetComponent<Rigidbody2D>();
        //shoutStrategy = new NoShout();
        //movingStrategy = new NoMoving();
    }

    public void Shout()
    {
        shoutStrategy = new NoShout();
        shoutStrategy.Shout();
    }

    public void Shout(Vector2 targetPoint, GameObject bullet)
    {
/*        Debug.Log("Shoot!");
        Vector2 direction = targetPoint - new Vector2(enemyTransform.position.x, enemyTransform.position.y);
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject shot = Instantiate(bullet, enemyTransform.position, rotation);*/

        //shoutStrategy = new ShoutAtPoint(targetPoint);
        //shoutStrategy.Shout();
    }

    public void Move()
    {
        movingStrategy = new NoMoving();
        movingStrategy.Move(enemyTransform);
    }

    public void Move(Vector2 targetPoint, float speed)
    {
        //enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, targetPoint, speed);

        /*Vector2 newPosition = Vector2.MoveTowards(enemyTransform.position, targetPoint, Time.deltaTime * speed);
        rigidbody.MovePosition(newPosition);*/

        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, targetPoint, speed);

        //movingStrategy = new MovingAtPoint(targetPoint, 0.1f);
        //movingStrategy.Move(enemyTransform);
    }

/*    public void Move(Vector2 targetPoint1, Vector2 targetPoint2)
    {
        movingStrategy = new MovingBetweenPoints(targetPoint1, targetPoint2);
        movingStrategy.Move(enemyTransform);
    }*/
}
