using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(Text))]
public class GearCounter : MonoBehaviour
{
    [SerializeField] private int gearsToWin;
    private Text textField;

    private int total;
    private int collected;

    private void Awake()
    {
        textField = GetComponent<Text>();
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
