using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
	public AudioClip clickSound;
	public AudioClip scoreSound;
	public AudioClip gameOverSound;

	public AudioSource mySource;

	protected override void Initialize()
	{

	}

	public void EnableSound()
	{
		PlayClickSound();
		mySource.volume = 1f;
		GameManager.GetInstance().soundOn = true;
	}

	public void DisableSound()
	{
		mySource.volume = 0f;
		GameManager.GetInstance().soundOn = false;
	}
	
	public void PlayClickSound()
	{
		mySource.clip = clickSound;
		MakeSound();
	}

	public void PlayScoreSound()
	{
		mySource.clip = scoreSound;
		mySource.Play();
	}

	public void PlayGameOverSound()
	{
		mySource.clip = gameOverSound;
		mySource.Play();
	}

	void MakeSound() 
	{
		mySource.Play();
	}
}
