using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneControl : MonoBehaviour
{
    [SerializeField] private int seconds;
    [SerializeField] private string[] nextSceneNames = new string[2];
    private FadeInOutScene fios;

    private const int SKIP_ONE_SCENE = 1;
    private const int SKIP_ALL_SCENES = 0;

    private void Awake()
    {
        fios = GetComponent<FadeInOutScene>();
    }

    void Start()
    {
        if (seconds > 0)
            StartCoroutine(WaitAndGoNext());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Skip(SKIP_ONE_SCENE);
        }
    }

    private void Skip(int index)
    {
        StopAllCoroutines();
        StartCoroutine(SkipCoroutine(index));
    }

    private IEnumerator WaitAndGoNext()
    {
        yield return new WaitForSeconds(seconds);
        if (fios != null)
            fios.isFadeOut = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneNames[SKIP_ONE_SCENE], LoadSceneMode.Single);
    }

    private IEnumerator SkipCoroutine(int index)
    {
        if (fios != null)
            fios.isFadeOut = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneNames[index], LoadSceneMode.Single);
    }

    public void SkipAll()
    {
        Skip(SKIP_ALL_SCENES);
    }
}
