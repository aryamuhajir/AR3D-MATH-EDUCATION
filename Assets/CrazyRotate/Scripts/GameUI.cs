using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour 
{
	public Text bestScoreText;

	void Update()
	{
		int bestScore = PlayerPrefs.GetInt("BestScore_Medium", 0);
		if (GameController.instance.level == Level.HARD)
		{
			bestScore = PlayerPrefs.GetInt("BestScore_Hard", 0);
		}
		bestScoreText.text = "Best " + bestScore;
	}
}
