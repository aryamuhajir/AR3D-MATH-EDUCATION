using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{	
	public int rotateAngle = 90;
	public int rotateSpeed = 600;
	public bool isSwipe = false;  // use Swipe or Touch on mobile devices

	private Vector3 touchOrigin = -Vector3.one;
	private bool rotating;
	private Vector3 curEuler;

	void Start()
	{
		rotating = false;
		curEuler = transform.eulerAngles;
	}

	void Update () 
	{
		int horizontal = 0;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		horizontal = (int)Input.GetAxisRaw("Horizontal");
#endif
#if UNITY_IPHONE || UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			Touch myTouch = Input.touches[0];
			if (Camera.main.ScreenToWorldPoint(myTouch.position).y < Camera.main.orthographicSize/2)
			{
				if (isSwipe)
				{
					if (myTouch.phase == TouchPhase.Began)
					{
						touchOrigin = myTouch.position;
					}
					else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
					{
						Vector2 touchEnded = myTouch.position;
						float x = touchEnded.x - touchOrigin.x;
						touchOrigin.x = -1;
						horizontal = x > 0 ? 1 : -1;
					}
				}
				else 
				{
					if (myTouch.phase == TouchPhase.Began)
					{
						float touchX = Camera.main.ScreenToWorldPoint(myTouch.position).x;
						if (touchX > 0) { horizontal = 1; }
						if (touchX < 0) { horizontal = -1; }
					}
				}
			}
		}
#endif
		if (horizontal !=0 && !rotating && !GameController.instance.gameOver)
		{
			ToRotate(horizontal);
		}
	}

	private void ToRotate(int horizontal)
	{
		if (horizontal > 0)
		{
			StartCoroutine(SmoothRotateRight(rotateAngle));
		}
		if (horizontal < 0)
		{
			StartCoroutine(SmoothRotateLeft(rotateAngle));
		}
	}

	private IEnumerator SmoothRotateLeft(int angle)
	{
		rotating = true;
		curEuler = transform.eulerAngles;
		Vector3 newAngle = curEuler + angle * Vector3.forward;
		while (curEuler.z < newAngle.z)
		{
			curEuler.z = Mathf.MoveTowards(curEuler.z, newAngle.z, rotateSpeed * Time.deltaTime);
			transform.eulerAngles = curEuler;
			yield return null;
		}
		rotating = false;
	}

	private IEnumerator SmoothRotateRight(int angle)
	{
		rotating = true;
		curEuler = transform.eulerAngles;
		Vector3 newAngle = curEuler - angle * Vector3.forward;
		while (curEuler.z > newAngle.z)
		{
			curEuler.z = Mathf.MoveTowards(curEuler.z, newAngle.z, rotateSpeed * Time.deltaTime);
			transform.eulerAngles = curEuler;
			yield return null;
		}
		rotating = false;
	}
}