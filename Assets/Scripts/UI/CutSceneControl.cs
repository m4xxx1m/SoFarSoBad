using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneControl : MonoBehaviour
{
    [SerializeField] private int seconds;
    [SerializeField] private string nextSceneName;
    private FadeInOutScene fios;

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
            StopAllCoroutines();
            StartCoroutine(SkipCoroutine());
        }
    }

    private IEnumerator WaitAndGoNext()
    {
        yield return new WaitForSeconds(seconds);
        if (fios != null)
            fios.isFadeOut = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    private IEnumerator SkipCoroutine()
    {
        if (fios != null)
            fios.isFadeOut = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    /*public void Skip()
    {
        StopAllCoroutines();
        StartCoroutine(SkipCoroutine());
    }*/
}
