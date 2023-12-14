using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Youtube : MonoBehaviour
{
	[SerializeField] public string link;
  public void OpenLink()
	{
		Application.OpenURL(link);
	}
}
