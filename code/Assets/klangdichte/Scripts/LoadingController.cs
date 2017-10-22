using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour {

    private AsyncOperation async = null; // When assigned, load is in progress.

    private void Start()
    {
        StartCoroutine(LoadScene("main"));
    }

    private IEnumerator LoadScene(string levelName)
    {
        async = SceneManager.LoadSceneAsync(levelName);
        yield return async;
    }
}
