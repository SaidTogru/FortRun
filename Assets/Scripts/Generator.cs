using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour


{
	[SerializeField] public GameObject Box, GlowBox, Ground;
	ColorCycler colorscript;
	public GameObject[] AllGrounds;
	GameObject player;
	public float ZPosition;
	public float GroundPosition;
	GameObject lastGround, spawnGround, lastGround1;
	public Color32 Colour;
	public MaterialPropertyBlock block;
	GameObject One, Two, Three, Four;
	public GameObject Target;
	float[] Stages = { 3.5f, 10.5f, 17.5f, 24.5f };
	int inc = 0;
	public Boolean firstStep = false;
	int HochLaufen;
	int Art1;
	int currentStage = 0;
	int t1, t2;
	[SerializeField] GameObject tutorialground;
	Buildsystem buildscript;
	bool tutorial;


	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		buildscript = player.GetComponent<Buildsystem>();
		ZPosition = player.transform.position.z-60;
		GroundPosition = GameObject.Find("FirstGround").transform.position.z;
		colorscript = GameObject.Find("Main Camera").GetComponent<ColorCycler>();
		currentStage = Random.Range(0, 4);
		Physics.IgnoreCollision(player.GetComponent<CapsuleCollider>(), GlowBox.GetComponent<Collider>());
		if (PlayerPrefs.GetInt("Tutorial") == 0) tutorial = false; else tutorial = true;
	}

	void Update()
	{
		if (player.transform.position.z >= ZPosition && tutorial)
		{
			Destroy(lastGround);
			lastGround = spawnGround;
			//ZPosition = player.transform.position.z + Ground.transform.localScale.z;
			ZPosition = ZPosition + Ground.transform.localScale.z;
			GroundPosition = GroundPosition + Ground.transform.localScale.z;
			spawnGround = Instantiate(Ground, new Vector3(Ground.transform.position.x, Ground.transform.position.y, GroundPosition), player.transform.rotation);
			spawnGround.gameObject.name = "-1";
			spawnBoxes();
			colorscript.spawnGround = spawnGround;
			if (firstStep == true) colorscript.lastGround = lastGround;
			colorscript.isLerping = true;
			firstStep = true;
		}
		else if (player.transform.position.z >= ZPosition && !tutorial)
		{
			tutorial = true;
			Destroy(lastGround);
			lastGround = spawnGround;
			ZPosition = ZPosition + Ground.transform.localScale.z;
			GroundPosition = GroundPosition + Ground.transform.localScale.z;
			spawnGround = Instantiate(tutorialground, new Vector3(0, -0.25f, 350), player.transform.rotation);
			spawnGround.gameObject.name = "-1";
			colorscript.spawnGround = spawnGround;
			if (firstStep == true) colorscript.lastGround = lastGround;
			colorscript.isLerping = true;
			firstStep = true;
		}
	}


	private void spawnBoxes()
	{
		float Step = -145;
		for (int i = 0; i < 15; i++)
		{
			Art1 = Random.Range(0, 2);
			HochLaufen = Random.Range(0, 2);
			//Debug.Log("Hochlaufen? :" + HochLaufen + "currentStage: " + currentStage);
			if (currentStage == 0)
			{
				One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition + Step), Box.transform.rotation);
				Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition + Step), Box.transform.rotation);
				currentStage = 1 + 10;

			}
			else if (currentStage == 1)
			{
				if (HochLaufen == 1)
				{
					if (Art1 == 1)
					{
						One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition + Step), Box.transform.rotation);
						Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition + Step), Box.transform.rotation);
					}
					else
					{
						One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition + Step + 10), Box.transform.rotation);
						Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition + Step), Box.transform.rotation);
					}

					currentStage = 2 + 10;
				}
				else
				{
					if (Art1 == 1)
					{
						One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition + Step), Box.transform.rotation);
						Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition + Step), Box.transform.rotation);
					}
					else
					{
						One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition + Step), Box.transform.rotation);
						Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition + Step), Box.transform.rotation);	 //ATTTTENTION
						Three = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[3], GroundPosition + Step + 10), Box.transform.rotation);
					}
					currentStage = 0 + 10;
				}
			}
			else if (currentStage == 2)
			{
				if (HochLaufen == 1)
				{
					if (Art1 == 1)
					{
					 One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition + Step), Box.transform.rotation);
					Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition + Step), Box.transform.rotation);
					}
					else
					{
						One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition + Step+10), Box.transform.rotation);
						Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition + Step), Box.transform.rotation);
					}
					currentStage = 3 + 10;
				}
				else
				{
					if (Art1 == 1)
					{
						One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition + Step), Box.transform.rotation);
						Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[3], GroundPosition + Step), Box.transform.rotation);
						Three = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition + Step), Box.transform.rotation);
						
					}
					else
					{
						One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition + Step), Box.transform.rotation);
						Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[3], GroundPosition + Step+10), Box.transform.rotation);
						Three = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition + Step), Box.transform.rotation);
					} 
				currentStage = 1 + 10;
				}
			}
			else if (currentStage == 3)
			{
				One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[3], GroundPosition + Step), Box.transform.rotation);
				Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition + Step), Box.transform.rotation);
				Three = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition + Step), Box.transform.rotation);
				currentStage = 2 + 10;
			}
			currentStage = currentStage - 10;
			One.gameObject.name = inc.ToString(); inc++;
			One.transform.SetParent(spawnGround.transform);
			if (Two != null)
			{
				Two.gameObject.name = inc.ToString(); inc++;
				Two.transform.SetParent(spawnGround.transform);
			}
			if (Three != null)
			{
				Three.gameObject.name = inc.ToString(); inc++;
				Three.transform.SetParent(spawnGround.transform);
			}

			Step += 20f;		
		}
			int k = Random.Range(0,4);
		    while(1==1){if(CheckIfPlaceIsFree(new Vector3(spawnGround.transform.position.x, Stages[k], GroundPosition-75))) { t1 = k; break; } else { k++;  if (k == 4) k = 0; }}
			k = Random.Range(0, 4);
			while (1==1){if(CheckIfPlaceIsFree(new Vector3(spawnGround.transform.position.x, Stages[k], GroundPosition + 75))) {  t2 = k;  break; } else { k++; if (k == 4) k = 0; }}
		Target = Instantiate(GlowBox, new Vector3(spawnGround.transform.position.x, Stages[t1], GroundPosition -75), GlowBox.transform.rotation);
		Target.gameObject.name = inc.ToString(); inc++;
		Target.transform.SetParent(spawnGround.transform);
		Target = Instantiate(GlowBox, new Vector3(spawnGround.transform.position.x, Stages[t2], GroundPosition + 75), GlowBox.transform.rotation);
		Target.gameObject.name = inc.ToString(); inc++;
		Target.transform.SetParent(spawnGround.transform);
		//Random Wallplace, except the points of the glowboxes
		int z1 = GiveMeANumber(new HashSet<int>() { 7, 22});
		int z2 = GiveMeANumber(new HashSet<int>() { 7, 22, z1});
		int z3 = GiveMeANumber(new HashSet<int>() { 7, 22,z1,z2 });
		//Debug.Log(z1+ " "+ z2+ " "+ z3);
		spawnWall(z1*10);
		spawnWall(z2* 10);
		spawnWall(z3 * 10);
	}

	Boolean CheckIfPlaceIsFree(Vector3 pos)
	{
		var hitColliders = Physics.OverlapSphere(pos, 1);
		if (hitColliders.Length > 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	void DestroyIfObject(Vector3 pos)
	{
		var hitColliders = Physics.OverlapSphere(pos, 1);
		if (hitColliders.Length>0) Destroy(hitColliders[0].transform.gameObject);
	}
	void spawnWall(int y)
	{
		DestroyIfObject(new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition - 145 + y)); 		 //Destroy the Cubes that on the Wallspawnvectors
		DestroyIfObject(new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition - 145 + y));
		DestroyIfObject(new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition - 145 + y));
		DestroyIfObject(new Vector3(spawnGround.transform.position.x, Stages[3], GroundPosition - 145 + y));
		One = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[0], GroundPosition -145+y), Box.transform.rotation);   	 //Spawn the wall
		Two = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[1], GroundPosition -145+y), Box.transform.rotation); 
		Three = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[2], GroundPosition -145+y), Box.transform.rotation); 
		Four = Instantiate(Box, new Vector3(spawnGround.transform.position.x, Stages[3], GroundPosition - 145+y), Box.transform.rotation); 
		One.gameObject.name = inc.ToString(); inc++;
		One.transform.SetParent(spawnGround.transform);
		Two.gameObject.name = inc.ToString(); inc++;
		Two.transform.SetParent(spawnGround.transform);
		Three.gameObject.name = inc.ToString(); inc++;
		Three.transform.SetParent(spawnGround.transform);
		Four.gameObject.name = inc.ToString(); inc++;
		Four.transform.SetParent(spawnGround.transform);
	}

	private int GiveMeANumber(HashSet<int> avoidNumbers)
	{
		var exclude = avoidNumbers;
		var range = Enumerable.Range(1, 28).Where(i => !exclude.Contains(i));
		int index = Random.Range(1, 28 - exclude.Count);
		return range.ElementAt(index);
	}



}
