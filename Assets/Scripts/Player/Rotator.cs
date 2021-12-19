using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite right;

    [SerializeField] private SpriteRenderer sr;

    private void SetDirection(Vector2 _dir)
    {
        if(_dir.y != 0)
            sr.sprite = (_dir.y > 0 ? up : down);
        else if(_dir.x != 0)
            sr.sprite = (_dir.x > 0 ? right : left);
    }
}
