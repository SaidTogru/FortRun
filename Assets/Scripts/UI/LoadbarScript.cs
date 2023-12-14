using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadbarScript : MonoBehaviour
{


	Slider LoadingSlider;
	 [SerializeField] public GameObject LoadbarObject;
	Vector3 namePos;


	void Start()
    {
		LoadingSlider = LoadbarObject.GetComponent<Slider>();
	}
													
	void FixedUpdate()
	{
		namePos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
		LoadingSlider.transform.position = namePos;	 
	}													

}
