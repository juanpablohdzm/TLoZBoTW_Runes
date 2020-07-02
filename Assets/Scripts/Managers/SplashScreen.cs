using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private SceneReference sceneToLoad;
    [SerializeField] private float minTime = 3.0f;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
       AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
       async.allowSceneActivation = false;
       float t = 0;
       while (async.isDone || t< minTime)
       {
           yield return null;
       }
       async.allowSceneActivation = true;
    }
}
