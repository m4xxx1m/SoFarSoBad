using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float health = 3f;

    private void AddHealth(float _delta)
    {
        health += _delta;
    }

    private void ReduceHealth(float _delta)
    {
        health -= _delta;

        if(health <= 0)
        {
            Debug.Log("Dead");
            gameObject.SendMessage("Death", null, SendMessageOptions.DontRequireReceiver);
        }
    }
    
    private void Death()
    {
        Destroy(gameObject);
    }
}
