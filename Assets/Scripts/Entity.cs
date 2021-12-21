using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float health = 3f;
    [SerializeField] private float radiationLevel = 0f;
    [SerializeField] private float maxRadiationLevel = 20f;
    [SerializeField] private float timeForNullRadiation = 5;
    [SerializeField] public bool isInRadiation = false;
    
    private UIControl uiControl;
    private float startHealth;

    //public float RadiationLevel { get => radiationLevel; set => radiationLevel = value; }

    private void Start()
    {
        startHealth = health;
        uiControl = GameObject.FindGameObjectsWithTag("Canvas")[0].GetComponent<UIControl>();
    }

    public void AddRadiation(float radLevel)
    {
        radiationLevel += radLevel;
        if (gameObject.tag == "Player") uiControl.radiationIndicatorWidth = radiationLevel / maxRadiationLevel;
        if (radiationLevel >= maxRadiationLevel)
        {
            Debug.Log("Dead by radiation");
            gameObject.SendMessage("Death by radiation", null, SendMessageOptions.DontRequireReceiver);
            Death();
        }
    }

    private void AddHealth(float _delta)
    {
        health += _delta;
        if (gameObject.tag == "Player") uiControl.healthIndicatorWidth = health / startHealth;
    }

    private void ReduceHealth(float _delta)
    {
        health -= _delta;
        if (gameObject.tag == "Player") uiControl.healthIndicatorWidth = health / startHealth;

        if (health <= 0)
        {
            Debug.Log("Dead");
            gameObject.SendMessage("Death", null, SendMessageOptions.DontRequireReceiver);
            Death();
        }
    }
    
    private void Death()
    {
        if (gameObject.tag == "Player")
        {
            uiControl.OpenGameOverMenu();
            Time.timeScale = 0f;
        }
        Destroy(gameObject);
    }

    public IEnumerator NullRadiationAfterSomeSeconds()
    {
        yield return new WaitForSeconds(timeForNullRadiation);
        if (!isInRadiation)
        {
            this.radiationLevel = 0f;
            uiControl.radiationIndicatorWidth = 0f;
        }
    }
}
