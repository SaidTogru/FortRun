using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
	public void ReloadScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
