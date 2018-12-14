using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using IndieYP;

public class Sharing : MonoBehaviour 
{
	public Button shareButton;
	private string screenshotFilename = "ScreenShot.png";

	void Start()
	{
#if UNITY_IPHONE
		shareButton.onClick.AddListener(OniOSMediaSharingClick);
#endif

#if UNITY_ANDROID
		shareButton.onClick.AddListener(OnAndroidMediaSharingClick);
#endif
	}
	
	public void OnAndroidMediaSharingClick()
	{
		SoundManager.GetInstance().PlayClickSound();
#if UNITY_ANDROID
		ScreenCapture.CaptureScreenshot(screenshotFilename);
		StartCoroutine(SaveAndShare(screenshotFilename));
#endif
	}

	public void OniOSMediaSharingClick()
	{
		SoundManager.GetInstance().PlayClickSound();
#if UNITY_IPHONE
		StartCoroutine(ShareScoreProcess());
#endif
	}

	IEnumerator ShareScoreProcess()
	{
		yield return new WaitForEndOfFrame ();
#if UNITY_IPHONE
		Texture2D screenshot = new Texture2D (Screen.width, Screen.height);
		screenshot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		screenshot.Apply ();
		
		byte[] imgBytes = screenshot.EncodeToPNG();
		uint imgLength = (uint)imgBytes.Length;
		int score = GameController.instance.GetScore();
		string shareMessage = string.Format("Wow ! I've scored {0} in this game !", score);
		string subjectTitle = "Score Sharing";
		ActivityView.Share(shareMessage, subjectTitle, imgBytes, imgLength);
#endif
	}

	IEnumerator SaveAndShare(string fileName)
	{
		yield return new WaitForEndOfFrame();
#if UNITY_ANDROID

		string path = Application.persistentDataPath + "/" +fileName;
		Debug.Log(path);
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "High Score");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "High Score ");
		int score = GameController.instance.GetScore();
		string shareMessage = string.Format("Wow ! I've scored {0} in this game !", score);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");

		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path); // Set Image Path Here

		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

		//			string uriPath =  uriObject.Call<string>("getPath");
		bool fileExist = fileObject.Call<bool>("exists");
		Debug.Log("File exist : " + fileExist);
		if (fileExist)
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);
#endif
	}
}