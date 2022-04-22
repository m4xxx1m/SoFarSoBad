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

    private DialogueSounds Sounds;

    private FadeInOutScene fios;

    private void Awake()
    {
        Sounds = GetComponent<DialogueSounds>();
        fios = GetComponent<FadeInOutScene>();
    }

    private void Start()
    {
        if (toTroned)
            nextSceneName = GlobalFields.fightWithTronedSceneName;
        else
            //nextSceneName = GlobalFields.gameplaySceneName;
            nextSceneName = GlobalFields.grohogCutSceneName;
        currentLine = 0;
        SetLine(lines[0]);
    }

    private void SetLine(Line _line)
    {
        Sounds.StopSound();
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
                Sounds.PlaySound(Sounds.RatSpeech2, 0.7f);
                break;
            
            case Speaker.VRUDNI:
                speakerText.text       = "Врудни";
                speakerText.alignment  = TextAnchor.MiddleRight;
                dialogueText.alignment = TextAnchor.UpperRight;
                Sounds.PlaySound(Sounds.VrudniSpeech, 0.15f);
                break;
                
            case Speaker.TRONED:
                speakerText.text       = "Тронед";
                speakerText.alignment  = TextAnchor.MiddleRight;
                dialogueText.alignment = TextAnchor.UpperRight;
                Sounds.PlaySound(Sounds.TronedSpeech, 0.6f);
                break;
        }
    }

    private void Update()
    {
        if(currentTextIndex < targetText.Length && (Input.GetKeyDown(KeyCode.LeftShift) || 
            Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            currentText         = targetText;
            dialogueText.text   = currentText;
            currentTextIndex    = targetText.Length;

            return;
        }
        if(currentTextIndex - targetText.Length >= 5)
        {
            //if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            //{
                if (currentLine + 1 < lines.Length)
                {
                    currentLine += 1;
                    SetLine(lines[currentLine]);
                }
                else
                {
                    //переход к игре
                    if (fios != null)
                        StartCoroutine(FadeToNextScene());
                    else
                        toNextScene();
                //SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
            }
            //}

            return;
        }

        textTimer -= Time.deltaTime;

        if(textTimer <= 0f)
        {
            textTimer = textSpeed;

            if (currentTextIndex < targetText.Length)
            {
                if (speakPauses.Contains(targetText[currentTextIndex]))
                    textTimer *= speakPauseMultiplier;
                currentText += targetText[currentTextIndex];
            }
            currentTextIndex++;

            dialogueText.text = currentText;
        }
    }

    /*private IEnumerator NextLine()
    {
        yield return new WaitForSeconds(1);
        if (currentLine + 1 < lines.Length)
        {
            currentLine += 1;
            SetLine(lines[currentLine]);
        }
        else
        {
            toNextScene();
        }
    }*/

    private IEnumerator FadeToNextScene()
    {
        fios.isFadeOut = true;
        yield return new WaitForSeconds(1);
        toNextScene();
    }

    private void toNextScene()
    {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    public void Skip()
    {
        StartCoroutine(FadeToNextScene());
    }
}
