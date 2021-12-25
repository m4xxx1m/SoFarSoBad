using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueControl : MonoBehaviour
{
    [SerializeField] private bool toTroned = false;
    private string nextSceneName;

    public enum Speaker {HERO, VRUDNI, TRONED};

    [Serializable] public struct Line
	{
	    public Speaker speaker;
	    public string  text;
	}
    
    [SerializeField] private float      speakPauseMultiplier = 2f;
    [SerializeField] private List<char> speakPauses;

    [SerializeField] private Text  dialogueText;
    [SerializeField] private Text  speakerText;
    
    [SerializeField] private float textSpeed;
    [SerializeField] private Line[] lines;

    private string  targetText;
    private string  currentText;

    private int     currentLine;
    private int     currentTextIndex;

    private float   textTimer;

    private void Awake()
    {
        if(toTroned)
            nextSceneName = GlobalFields.fightWithTronedSceneName;
        else
            nextSceneName = GlobalFields.gameplaySceneName;

        currentLine = 0;
        SetLine(lines[0]);
    }

    private void SetLine(Line _line)
    {
        currentTextIndex    = 0;
        textTimer           = textSpeed;

        dialogueText.text   = "";
        currentText         = "";
        targetText          = _line.text;

        switch(_line.speaker)
        {
            case Speaker.HERO:
                speakerText.text       = "Робот";
                speakerText.alignment  = TextAnchor.MiddleLeft;
                dialogueText.alignment = TextAnchor.UpperLeft;
                break;
            
            case Speaker.VRUDNI:
                speakerText.text       = "Врудни";
                speakerText.alignment  = TextAnchor.MiddleRight;
                dialogueText.alignment = TextAnchor.UpperRight;
                break;
                
            case Speaker.TRONED:
                speakerText.text       = "Тронед";
                speakerText.alignment  = TextAnchor.MiddleRight;
                dialogueText.alignment = TextAnchor.UpperRight;
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0))
        {
            currentText         = targetText;
            dialogueText.text   = currentText;
            currentTextIndex    = targetText.Length;

            return;
        }

        if(currentTextIndex >= targetText.Length)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                if(currentLine + 1 < lines.Length)
                {
                    currentLine += 1;
                    SetLine(lines[currentLine]);
                }
                else
                {
                    //переход к игре
                    SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
                }
            }

            return;
        }

        textTimer -= Time.deltaTime;

        if(textTimer <= 0f)
        {
            textTimer = textSpeed;

            if(speakPauses.Contains(targetText[currentTextIndex]))
                textTimer *= speakPauseMultiplier;

            currentText += targetText[currentTextIndex];
            currentTextIndex++;

            dialogueText.text = currentText;
        }
    }
}
