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
            textField.text = count.ToString();
        }
        catch (System.Exception)
        {
            textField.text = "0";
        }
    }
}
