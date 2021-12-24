using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    [SerializeField] private Text textField;

    //private int total;
    //private int collected;

    private void Awake()
    {
        SetCount();
    }

    public void SetCount()
    {
        try
        {
            int count = Points.getCurrentInstance().pointCounter;
            Debug.Log("Point " + count);
            textField.text = "Счёт: " + count.ToString();
            
            if(!PlayerPrefs.HasKey("Highscore") || (PlayerPrefs.HasKey("Highscore") && PlayerPrefs.GetInt("Highscore") < Points.getCurrentInstance().pointCounter))
            {
                PlayerPrefs.SetInt("Highscore", Points.getCurrentInstance().pointCounter);
            }
        }
        catch (System.Exception)
        {
            textField.text = "Счёт: 0";
        }
    }
}
