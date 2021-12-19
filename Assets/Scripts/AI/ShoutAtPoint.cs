using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutAtPoint : MonoBehaviour, ShoutStrategy
{
    private Vector2 targetPoint;

    public ShoutAtPoint(Vector2 targetPoint)
    {
        this.targetPoint = targetPoint;
    }

    public void Shout()
    {
        throw new System.NotImplementedException();
    }
}
