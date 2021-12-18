using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBetweenPoints : MovingStrategy
{
    private Vector2 targetPoint1;
    private Vector2 targetPoint2;

    public MovingBetweenPoints(Vector2 targetPoint1, Vector2 targetPoint2)
    {
        this.targetPoint1 = targetPoint1;
        this.targetPoint2 = targetPoint2;
    }

    public override void Move(Transform transform)
    {
        throw new System.NotImplementedException();
    }
}
