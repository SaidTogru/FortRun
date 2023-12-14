using UnityEngine;
using System.Collections;
using System;

public class CameraControll : MonoBehaviour
{

	[HideInInspector] GameObject player;
	Movement playerscript;
	Buildsystem buildsystem;
	private Vector3 offset;
	float goBack = 0.05f;
	[SerializeField] MainMenu menuscript;
	Vector3 rotation1, position1,position11;
	bool cameraReady = false;  
	float height = 17.4f;		  //17.4

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerscript = player.GetComponent<Movement>();
		buildsystem = player.GetComponent<Buildsystem>();
		//offset = player.transform.position-transform.position;
		//position1 = new Vector3(36f, 19.82f, 120f-1); rotation1 = new Vector3(7.146f-1, -50.919f + 1.9f+-3, 6.465f+2);		 //7.9
		 position1 = new Vector3(36.39f, 19.82f, 120f-4.5f+30); rotation1 = new Vector3(7.146f-0.1f, -50.919f + 1.9f+-3.8f, 6.465f+4.4f);   //13.9
		//position1 = new Vector3(11, 22.1f, 100.6f); rotation1 = new Vector3(-183.319f, 153.44f, 192.742f);		 //7.9
		StartCoroutine(CoroutineForCamera()); offset = player.transform.position - position1;
	}

	void LateUpdate()
	{
	   	if (menuscript.gamestarted) {
			if (cameraReady == false) setCameraPosition();
		if (playerscript.gameover == true)
		{	
			if (goBack < 5f)																			
			{
			   goBack += 0.05f;
			}
			if (buildsystem.lastCol==1)			//Stair
			{
				Quaternion target = Quaternion.Euler(player.transform.rotation.x-35, 0, 0);
				player.transform.rotation = Quaternion.Slerp(player.transform.rotation, target, Time.deltaTime * 500f);
			}
			else if (buildsystem.lastCol == 2)         //Stair
			{
				Quaternion target = Quaternion.Euler(player.transform.rotation.x + 25, 0, 0);
				player.transform.rotation = Quaternion.Slerp(player.transform.rotation, target, Time.deltaTime * 500f);
			}	  
			
				transform.position = new Vector3(player.transform.position.x, height , player.transform.position.z-goBack)-offset;
		}
		else {	
				transform.position = new Vector3(player.transform.position.x, height, player.transform.position.z) - offset;
		}
	}
	}

	private void setCameraPosition()
	{
		cameraReady = true;
	}
	IEnumerator CoroutineForCamera()
	{	
		
		LeanTween.rotate(gameObject, rotation1, 0.5f);
		LeanTween.move(gameObject, new Vector3(position1.x,position1.y+height,position1.z), 1f);	
		playerscript.animator.SetBool("WarmUp", true);
		//playerscript.animator.SetBool("Run", true);
		yield return new WaitForSeconds(1f);
		menuscript.gamestarted = true;
	}
}
