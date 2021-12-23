using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GearCounter : MonoBehaviour
{
    [SerializeField] private Text textField;

    private int total;
    private int collected;

    private void Awake()
    {
        SetCount(0);
    }

    public void SetCount(int _count)
    {
        textField.text = _count.ToString();
    }
}
