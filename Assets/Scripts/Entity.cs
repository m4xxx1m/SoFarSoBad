using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float health = 3f;
    [SerializeField] private float radiationLevel = 0f;
    [SerializeField] private float maxRadiationLevel = 100f;
    [SerializeField] private float timeForNullRadiation = 5;
    [SerializeField] public bool isInRadiation = false;

    //public float RadiationLevel { get => radiationLevel; set => radiationLevel = value; }
    public void AddRadiation(float radLevel)
    {
        radiationLevel += radLevel;
        if (radiationLevel >= maxRadiationLevel)
        {
            Debug.Log("Dead by radiation");
            gameObject.SendMessage("Death by radiation", null, SendMessageOptions.DontRequireReceiver);
        }
    }

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

    public IEnumerator NullRadiationAfterSomeSeconds()
    {
        yield return new WaitForSeconds(timeForNullRadiation);
        if (!isInRadiation)
            this.radiationLevel = 0f;
    }
}
