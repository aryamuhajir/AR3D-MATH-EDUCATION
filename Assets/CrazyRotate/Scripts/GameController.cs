using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public enum Level 
{
	MEDIUM,
	HARD,
}

public class GameController : MonoBehaviour 
{
	public static GameController instance = null;
	public GameObject[] Balls;
	public GameObject[] NextBalls;
	public Vector3 SpawnPoint;
	public GameObject gameOverText;
	public GameObject restartButton;
	public bool gameOver = false;
	public Level level;

	private Text scoreText;
	private int score;
	private List<GameObject> ballsList;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else  if (instance != null)
		{
			Destroy(gameObject);
		}
		ballsList = new List<GameObject>();
		InitGame();
	}

	void InitGame()
	{
		score = 0;
		ballsList.Clear();
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		scoreText.text = "0";
		restartButton = GameObject.Find("RestartButton");
		gameOverText.SetActive(false);
		restartButton.SetActive(false);
		StartCoroutine("SpawnBall");
	}


	private IEnumerator SpawnBall()
	{
		while (!gameOver)
		{
			int randomIndex = Random.Range(0, Balls.Length);
			UpdateNextBall(randomIndex);
			GameObject ball = Instantiate(Balls[randomIndex], SpawnPoint, Quaternion.identity) as GameObject;
			ballsList.Add(ball);
			yield return new WaitForSeconds(1.5f);
		}
	}

	public void UpdateNextBall(int index)
	{
		for (int i = 0; i < NextBalls.Length; i++)
		{
			if (i != index)
			{
				NextBalls[i].SetActive(false);
			}
			else 
			{
				NextBalls[i].SetActive(true);
			}
		}
	}

	public void Score()
	{
		SoundManager.GetInstance().PlayScoreSound();
		score += 1;
		scoreText.text = "" + score;
	}

	public int GetScore()
	{
		return score;
	}

	public void GameOver()
	{
		SoundManager.GetInstance().PlayGameOverSound();
		foreach (GameObject ball in ballsList)
		{
			Destroy(ball);
		}
		ballsList.Clear();
		gameOver = true;
		restartButton.SetActive(true);
		gameOverText.SetActive(true);
		StopCoroutine("SpawnBall");
		if (level == Level.MEDIUM)
		{
			int bestScore = PlayerPrefs.GetInt("BestScore_Medium", 0);
			if (score > bestScore)
			{
				bestScore = score;
				PlayerPrefs.SetInt("BestScore_Medium", bestScore);
			}
		}
		if (level == Level.HARD)
		{
			int bestScore = PlayerPrefs.GetInt("BestScore_Hard", 0);
			if (score > bestScore)
			{
				bestScore = score;
				PlayerPrefs.SetInt("BestScore_Hard", bestScore);
			}
		}
	}

	public void Restart()
	{
		SoundManager.GetInstance().PlayClickSound();
		gameOver = false;
		InitGame();
	}

	public void Quit()
	{
		SoundManager.GetInstance().PlayClickSound();
		Time.timeScale = 1f;
		Application.LoadLevel("MainMenu");
	}

	public void Pause()
	{
		SoundManager.GetInstance().PlayClickSound();
		Time.timeScale = 0f;
	}

	public void Resume()
	{
		SoundManager.GetInstance().PlayClickSound();
		Time.timeScale = 1f;
	}
}
