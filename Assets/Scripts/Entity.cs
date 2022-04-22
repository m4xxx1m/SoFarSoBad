using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float health = 3f;
    [SerializeField] public bool isInRadiation = false;
    
    protected UIControl uiControl;
    protected float startHealth;

    private void Awake()
    {
        uiControl = GameObject.FindGameObjectsWithTag(GlobalFields.canvasTag)[0].GetComponent<UIControl>();
    }

    public virtual void Start()
    {
        startHealth = health;
    }

    protected virtual void AddHealth(float _delta)
    {
        if (_delta < 0f)
        {
            ReduceHealth(_delta);
            return;
        }
        health += _delta;
        if (health > startHealth) health = startHealth;
    }

    protected virtual void ReduceHealth(float _delta)
    {
        if (gameObject.tag == GlobalFields.grohogTag)
            return;
        health -= _delta;
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is Dead");
            Death();
        }
    }
    
    protected virtual void Death()
    {
        if(gameObject.tag == GlobalFields.tronedTag)
        {
            uiControl.OpenWinMenu();
            Time.timeScale = 0f;
            //uiControl.GoToScoretable();
        }
        if (gameObject.tag == GlobalFields.vrudniTag)
        {
            Points.getCurrentInstance().pointCounter += (int)Mathf.Sqrt(Points.CurrentChunk) * Points.pointsForVruden;
            Points.Counter.SetCount();
        }
        if (gameObject.tag == GlobalFields.grohogTag)
        {
            Points.getCurrentInstance().pointCounter += (int)Mathf.Sqrt(Points.CurrentChunk) * Points.pointsForGrohog;
            Points.Counter.SetCount();
        }
        Destroy(gameObject);
    }
}
