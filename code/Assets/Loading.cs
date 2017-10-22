using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {


    //异步对象    
    private AsyncOperation _asyncOperation;
    //显示tips的文本  
   // private Text _tip;
    //tips的集合  

    //更新tips的时间间隔  
    private const float _updateTime = 1f;
    //上次更新的时间  
    private float _lastUpdateTime = 0;
    //滑动条  
    public Slider _slider;
    //显示进度的文本  
    public Text _progress;
    // Use this for initialization  
    void Start()
    {
        //_tip = GameObject.Find("Text").GetComponent<Text>();
        //_progress = GameObject.Find("Progress").GetComponent<Text>();
        StartCoroutine(LoadAsync(Global.loadName));
    }

    // Update is called once per frame  
    void Update()
    {
        //首先判断是否为空，其次判断是否加载完毕  
        if (_asyncOperation != null && !_asyncOperation.allowSceneActivation)
        {
            //开始更新tips  
            if (Time.time - _lastUpdateTime >= _updateTime)
            {
                _lastUpdateTime = Time.time;
            }
        }
    }
  
    /// <summary>  
    /// 携程进行异步加载场景  
    /// </summary>  
    /// <param name="sceneName">需要加载的场景名</param>  
    /// <returns></returns>  
    IEnumerator LoadAsync(string sceneName)
    {
        //yield return new WaitForEndOfFrame();
        //当前进度  
        int currentProgress = 0;
        //目标进度  
        int targetProgress = 0;
        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //unity 加载90%  
        _asyncOperation.allowSceneActivation = false;
        while (_asyncOperation.progress < 0.9f)
        {
            targetProgress = (int)_asyncOperation.progress * 100;
            //平滑过渡  
            while (currentProgress < targetProgress)
            {
                ++currentProgress;
                _progress.text = System.String.Format("{0}{1}", currentProgress.ToString(), "%");
                _slider.value = (float)currentProgress / 100;
                yield return new WaitForEndOfFrame();
            }
        }
        //自行加载剩余的10%  
        targetProgress = 100;
        while (currentProgress < targetProgress)
        {
            ++currentProgress;
            _progress.text = System.String.Format("{0}{1}", currentProgress.ToString(), "%");
            _slider.value = (float)currentProgress / 100;
            yield return new WaitForEndOfFrame();
        }
        _asyncOperation.allowSceneActivation = true;
    }  


}
