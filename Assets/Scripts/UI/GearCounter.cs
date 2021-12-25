using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GearCounter : MonoBehaviour
{
    [SerializeField] private int gearsToWin;
    [SerializeField] private Text textField;

    private int total;
    private int collected;

    private void Awake()
    {
        SetCount(0);
    }

    public void SetCount(int _count)
    {
        if(_count >= gearsToWin - 1)
        {
            SceneManager.LoadScene(GlobalFields.dialogueWithTronedSceneName, LoadSceneMode.Single);
        }
        Debug.Log("_count " + _count);
        textField.text = _count.ToString() + "/" + gearsToWin.ToString();
    }
}
