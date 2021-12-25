using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour
{
    [SerializeField] private Text highscore;

    private void Start()
    {
        if(PlayerPrefs.HasKey("Highscore"))
            highscore.text = PlayerPrefs.GetInt("Highscore").ToString();
    }
}
