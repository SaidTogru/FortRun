using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public float speed; bool Increasing = true; float move;  //8 is normal start 
	[HideInInspector] public Animator animator;
	private CharacterController player;
	private float groundDistance;
	
	public Collision col;
	public bool gameover = false;
	[SerializeField] MainMenu menuscript;
	Buildsystem buildscript;
	[SerializeField] Pointsystem pointscript;
	int nextIncrease;
	public bool sreversed, stair=false;
	private float startBoost;

	void Start()
	{
		//PlayerPrefs.SetInt("Speed", 0);
		if (PlayerPrefs.GetInt("Speed") == 0) speed = 9f; else if (PlayerPrefs.GetInt("Speed") == 1) speed = 10f; else speed = 11f;
		animator = GetComponent<Animator>();
		startBoost = transform.position.z + 45;
		player = GetComponent<CharacterController>();
		groundDistance = player.bounds.extents.y;
		buildscript = gameObject.GetComponent<Buildsystem>();
		nextIncrease = 10;
		//if (PlayerPrefs.GetInt("Speed") >= 1) speed = 9f;
	}

	void Update()
	{
		
		if (gameover == false && menuscript.gamestarted==true) { 
		if (speed > 13) Increasing = false; 
		if (Increasing) increaseSpeed(); 	 //pc 12 mobile 11
		animator.SetBool("Run", true);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 360 * Time.deltaTime);
		Vector3 movement = new Vector3(0, -5, speed) * Time.deltaTime * 2f;
		player.Move(movement);

		if (!isGrounded() || sreversed == true)
		{
			player.Move(Vector3.down * groundDistance / 1 * 55 * Time.deltaTime);
		}	 
		}
	}

	bool isGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, groundDistance + 0.1f);
	}
	void OnCollisionEnter(Collision collision)
	{  
		col = collision;
		if (collision.gameObject.layer == 10)
		{
			sreversed = true;
		}								  
		else
		{
			sreversed = false;
		}

	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
			if(hit.gameObject.layer==12)   //Box
		{
			//buildscript.CrashPart.Play();
			buildscript.Loading.gameObject.SetActive(false);
			gameover = true;
			buildscript.QuitGame();
			}
			if (hit.gameObject.layer == 18) Destroy(hit.gameObject.transform.parent.gameObject);		 //TESTPHASE
	}

	void increaseSpeed()
	{
		if (pointscript.i >= nextIncrease)
		{
			speed = speed + 0.05f;
			nextIncrease = RoundDown(nextIncrease + 10);
			buildscript.Dist += 0.5f;
		}
	}
	int RoundDown(float ttoRound)
	{
		int toRound = (int)ttoRound;
		return toRound - toRound % 10;
	}




}