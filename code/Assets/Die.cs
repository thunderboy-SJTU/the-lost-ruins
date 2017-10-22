using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Die : MonoBehaviour {
	public GameObject scoreText;
	public ControllerManager manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.GetComponent<Text>().text = "Your Score:" + gameManager.score.ToString();
		if (manager.leftControllerTouchDown.trigger) {
			gameManager.score = 0;
			gameManager.life = 5;
			gameManager.start = false;
			gameManager.currentProcess = 0.0f;
			Global.loadName = "Test";
			SceneManager.LoadScene("Loading");
		}
	}
}
