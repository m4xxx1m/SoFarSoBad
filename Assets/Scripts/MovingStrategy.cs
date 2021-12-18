using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingStrategy : MonoBehaviour
{
    protected Transform enemyTransform;

    public abstract void Move(Transform transform);
}
