using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

    public int stage = 1;
    static public int score = 0;
    static public int life = 5;
	static public float currentProcess = 0.0f;
    public GameObject scoreText;
    public GameObject stageText;
    public GameObject lifeText;
    public processbar bar;
    public int magicstate = 0;
    public int timecount = 500;
    public bool attacked = false;
    public int attackcount = 0;
    static public bool start = false;

    static public float  playerHeight=0;
    //public int warning = 0;
    
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
		
	static public void die()
	{
		SceneManager.LoadScene("Die");
	}

    public int getScore()
    {
        return score;
    }

    public int getStage()
    {
        return stage;
    }

    public int getlife()
    {
        return life;
    }

    public void setScore(int score)
    {
        gameManager.score = score;
    }

    public void addStage()
    {
        stage++;
    }

    public void reduceLife()
    {
        life--;
		if (life <= 0)
			die ();
    }

    public void updateLife()
    {
        lifeText.GetComponent<Text>().text = "Life:" + life.ToString();
    }

    public void updateScore()
    {
        scoreText.GetComponent<Text>().text = "Score:" + score.ToString();
    }

    public void setProcess(float process)
    {
        gameManager.currentProcess = process;
    }

    public float getProcess()
    {
        return currentProcess;
    }

    public void updateProcess()
    {
        bar.updateProcess();
    }

    public void setMagic(int magic)
    {
        magicstate = magic;
    }

    public int getMagic()
    {
        return magicstate;
    }

    public int getTimecount()
    {
        return timecount;
    }

    public void minorTimecount()
    {
        if(timecount > 0)
           timecount--;
        else
        {
            magicstate = 0;
            currentProcess = 0;
        }
    }


}
