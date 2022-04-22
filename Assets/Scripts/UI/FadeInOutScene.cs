using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutScene : MonoBehaviour
{
    [SerializeField] private Image black;
    private Color color = Color.black;

    public bool isFadeIn;
    public bool isFadeOut;

    void Start()
    {
        black.enabled = true;
        isFadeIn = true;
        color.a = 1f;
    }

    void Update()
    {
        if (isFadeIn)
        {
            if (color.a > 0f)
            {
                color.a -= 0.01f;
                black.color = color;
            }
            else
                isFadeIn = false;
        }
        if (isFadeOut)
        {
            if (color.a < 1f)
            {
                color.a += 0.01f;
                black.color = color;
            }
            else
            {
                isFadeOut = false;
            }
        }
    }
}
