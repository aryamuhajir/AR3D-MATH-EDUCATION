using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour 
{
	void OnMouseDown()
	{
		GameController.instance.Restart();
	}
}
