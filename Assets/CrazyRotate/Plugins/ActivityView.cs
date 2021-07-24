using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;

namespace IndieYP
{

	public class ActivityView : MonoBehaviour {

	public const string POST_DONE = "Done";
	public const string POS_CANCELLED = "Cancelled";

	[DllImport("__Internal")]
	private static extern void ShareOnlyText(string messageText, string subjectTitle, int[] activityArray, int arraySize);
	[DllImport("__Internal")]
	private static extern void ShareImgUseBytes(string messageText, string subjectTitle, byte[] imageBuffer, uint bufferLength, int[] activityArray, int arraySize);
	[DllImport("__Internal")]
	private static extern void ProgressBarTweakInit (float posX, float posY);
	[DllImport("__Internal")]
	private static extern void ProgressBarTweakStart ();
	[DllImport("__Internal")]
	private static extern void ProgressBarTweakStop ();
	[DllImport("__Internal")]
	private static extern void ShareMessageToService(string messageText, socialService socialService, string errorMessage, string delegateGO, string callbackMethod);
	[DllImport("__Internal")]
	private static extern void ShareImgToService(string messageText, byte[] imageBuffer, uint bufferLength, socialService socialService, string errorMessage, string delegateGO, string callbackMethod);
	[DllImport("__Internal")]
	private static extern void WriteImgToCameraRoll(byte[] imageBuffer, uint bufferLength);


	public enum UIActivityType
	{
		PostToFacebook, 
		PostToTwitter,
		PostToWeibo,
		Message,
		Mail,
		Print,
		CopyToPasteboard,
		AssignToContact,
		SaveToCameraRoll,
		AddToReadingList,
		PostToFlickr,
		PostToVimeo,
		PostToTencentWeibo,
		AirDrop
	}

	public enum socialService
	{
		Twitter,
		Facebook,
		SinaWeibo,
		TencentWeibo
	};

	public static void InitProgressBar (float posX, float posY)
	{
		ProgressBarTweakInit (posX, posY);
	}

	public static void ShowProgressBar()
	{
		ProgressBarTweakStart ();
	}

	public static void HideProgressBar()
	{
		ProgressBarTweakStop ();
	}

	public static void Share (string messageText, string subjectTitle, UIActivityType[] activityArray = null)
	{
		int[] resultArray = null;
		int arraySize = 0;
		if (activityArray != null)
		{
			resultArray = Array.ConvertAll<UIActivityType, int> (activityArray, delegate (UIActivityType value) {
				return (int) value;});
			arraySize = resultArray.Count();
		}
		ShareOnlyText (messageText, subjectTitle, resultArray, arraySize);
	}
	
	public static void Share (string messageText, string subjectTitle, byte[] imageBuffer, uint bufferLength, UIActivityType[] activityArray = null)
	{
		int[] resultArray = null;
		int arraySize = 0;
		if (activityArray != null)
		{
			resultArray = Array.ConvertAll<UIActivityType, int> (activityArray, delegate (UIActivityType value) {
			return (int) value;});
			arraySize = resultArray.Count();
		}
		ShareImgUseBytes (messageText, subjectTitle, imageBuffer, bufferLength, resultArray, arraySize);
	}


	public static void ShareToService (string messageText, socialService socialService, string errorMessage, string delegateGO = null, string callbackMethod = null)
	{
		ShareMessageToService (messageText, socialService, errorMessage, delegateGO, callbackMethod);
	}

	public static void ShareToService (string messageText, socialService socialService, byte[] imageBuffer, uint bufferLength, string errorMessage, string delegateGO = null, string callbackMethod = null)
	{
		ShareImgToService (messageText, imageBuffer, bufferLength, socialService, errorMessage, delegateGO, callbackMethod);
	}
	
	public static void SaveImgToCameraRoll(byte[] imageBuffer, uint bufferLength)
	{
		WriteImgToCameraRoll (imageBuffer, bufferLength);
	}
  

#region IO METHODS
	
	/// <summary>
	/// Gets the app streaming assets data path.
	/// </summary>
	/// <value>The app data path.</value>
	public static Uri AppDataPath
	{
		get 
		{
			UriBuilder uriBuilder = new UriBuilder();      
			uriBuilder.Scheme = "file";
#if !UNITY_EDITOR
			uriBuilder.Path = System.IO.Path.Combine(appDataPath, "Raw");
#else
			uriBuilder.Path = System.IO.Path.Combine(appDataPath, "StreamingAssets");
#endif
			return uriBuilder.Uri;
		}
	}
	
	private static string appDataPath
	{
		get 
		{               
			return Application.dataPath; 
		}
	}
	
#endregion
	
}

}