using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Buildsystem : MonoBehaviour
{
	//Building
	[SerializeField] public GameObject player, floor, stair, stairReversed;
	[SerializeField] Material BuildColor;
	Movement playerscript;
	[HideInInspector] public GameObject lastObject;
	bool wasFloor;
	bool overLine;
	float nextLine;
	private bool autohelp;
	float Stage = 0;           //Stufe 0 ,7 ,14,21  (28 [Decke]) 
	private int tmpNextLine;                                                                          //-------
	public List<GameObject> deleted = new List<GameObject>();                                                //Debugging and Deleting
	int builtCounter;
	public AudioSource ShieldSound;
	public AudioSource AttackSound;
	GameObject lastReverse;      // um gleichzeitiges Bauen von ReversedStair und Floor zu verhindern	
	GameObject lastFloor;// --																			//--------  
	float LaserProgress = 0;                                                                              //Shooting
	RaycastHit Laserhit;
	private bool shot;                                                                            //--------	 Loading
	float Timer,TimerLoading,NeededBattery;
	[HideInInspector]  public Slider Loading;
	[HideInInspector] public ParticleSystem CrashPart,LoadPart,AttackPart;
	[SerializeField] ParticleSystem ExplosionPart;
	[SerializeField] GameObject Loadbar;
	bool trg;		//for Glowbox
	float cubeSize = 2f; float cubeSizeY = 1.5f;   //Cubes Size	 											  //Explosion
	ColorCycler colorscript;
	float powerExplosion = 7;   //power of explosion
	[SerializeField] LayerMask ignoredLayer,checkForGlowbox,LaserIgnore;   //ignore Glowbox and Explosion pieces
	[SerializeField] GameObject RandomObject;
	float cubesPivotDistance;
	Vector3 cubePivot;
	int cubesinRow = 5;                                                                           //--------
	Collider playerCollider;
	int randomNumber = 1;
	public float Dist = 10;
	[SerializeField] MainMenu menuscript;
	float Laserheight = 2f;     //for easier gameplay //stairup and down value changes
	bool readytoShoot = true;
	private Collider[] hitColliders;
	Quaternion rot;													 
	Vector2 firstPressPos;               //	MobileControl
	Vector2 secondPressPos;
	Vector2 currentSwipe; int minSwipe = 20;     
	public bool hoch,runter,schieß=false;
	[SerializeField] private GameObject SecondMenu;
	[SerializeField] private Text points;
	[SerializeField] private Highscore highscorescript;
	private GameObject lastStair,lastlastStair;
	bool startGame;
	private bool wasReverse;  
	private bool inbox;
	private bool free = true;
	bool Delay = true;
	[SerializeField] AudioSource Music;
	public float tutorialend = 0;
	int hochcounter = 0;
	int runtercounter = 0;
	int schießcounter = 0;
	public bool Pause;
	int p = 0;
	[SerializeField] GameObject SwipeUp,SwipeDown,xSwipeUp,xSwipeDown,Tap,xTap, Roof,RoofSign,Health,GlowboxSign,TaptoPlay,QuickTutorial;
	public int lastCol = 0;

	void Start()
	{	
		rot = player.transform.rotation; 
		stair.GetComponent<MeshRenderer>().material = BuildColor;
		floor.GetComponent<MeshRenderer>().material = BuildColor;
		stairReversed.GetComponent<MeshRenderer>().material = BuildColor;
		Loading = Loadbar.GetComponent<Slider>();
		Loading.value = 1f;			NeededBattery = -1f;
		Timer = 1f;			 TimerLoading = 0.04f;
		lastObject = Instantiate(floor, new Vector3(0, 0, -100), transform.rotation); //lastObject should be not null, because of Exception
		cubesPivotDistance = cubeSize * cubesinRow / 2; cubePivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance); //Explosion
		colorscript = GameObject.Find("Main Camera").GetComponent<ColorCycler>();   //Initialize
		playerscript = player.GetComponent<Movement>();									 //--
		playerCollider = player.GetComponent<Collider>();								//--
		LoadPart = GameObject.Find("Shield").GetComponentInChildren<ParticleSystem>();
		AttackPart = GameObject.Find("Attack").GetComponentInChildren<ParticleSystem>();
		CrashPart = GameObject.Find("Crash").GetComponentInChildren<ParticleSystem>();
		lastFloor = lastStair= lastlastStair= lastReverse= GameObject.CreatePrimitive(PrimitiveType.Cube);
	}

 
	void Update()
	{
		if (!playerscript.gameover && menuscript.gamestarted && PlayerPrefs.GetInt("Tutorial")==1)	
		{	SwipeMobile();
			///Avoiding to Build ReversedStair and Stair on same Place
			if (shot == true) { LaserUpdate(); }
			deleteLastObjects();
			overLineUpdate();
			//Floor();
			DamagePlayerAndLoading();
			if (player.transform.position.z > 160) { 
			if ((Input.GetKeyDown(KeyCode.Alpha1) || hoch) && Stage != 21)
			{
				Stair(player.transform.position.z);
			}
			if ((Input.GetKeyDown(KeyCode.Alpha2) || runter) && Stage != 0)
			{
				StairReversed(player.transform.position.z); 		//StartCoroutine(DelayForReverse());
			}
			}
			if ((Input.GetKeyDown(KeyCode.Alpha3) || schieß) && !shot)
			{
				Laser();
			}
			Floor(); 
			if (lastFloor != null && lastReverse != null && lastStair != null)
			{
				if (lastReverse.transform.position.z == lastFloor.transform.position.z - 5 || lastStair.transform.position.z == lastFloor.transform.position.z - 5) Destroy(lastFloor);
			}
		}
		else if (!playerscript.gameover && menuscript.gamestarted && PlayerPrefs.GetInt("Tutorial") == 0)		   //Tutorial
		{
			SwipeMobile();
			if (player.transform.position.z >= 460) PlayerPrefs.SetInt("Tutorial", 1);
			Tutorial();
		}
	}

	void Tutorial()
	{
		if (Mathf.Ceil(player.transform.position.z) < 185) QuickTutorial.SetActive(true); else QuickTutorial.SetActive(false);
		if (Mathf.Ceil(player.transform.position.z) == 183) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 185 && Pause) { PauseGame(1); Pause = false; }
		if (hoch && Mathf.Ceil(player.transform.position.z) == 185) { Pause = false; ResumeGame(); Stair(player.transform.position.z); hoch = false; lastObject = Instantiate(floor, new Vector3(0, 7, 205), rot); Pause = false; }

		if (Mathf.Ceil(player.transform.position.z) == 203) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 205 && Pause) { PauseGame(2); Pause = false; }
		if (runter && Mathf.Ceil(player.transform.position.z) == 205) { ResumeGame(); StairReversed(player.transform.position.z); runter = false; hochcounter = 0; Pause = false; }

		if (Mathf.Ceil(player.transform.position.z) == 213) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 215 && Pause) { PauseGame(3); Pause = false; }
		if (hoch && Mathf.Ceil(player.transform.position.z) == 215 && hochcounter == 2) { ResumeGame(); Stair(player.transform.position.z); hoch = false; Stair(player.transform.position.z + 10); Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 233) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 235 && Pause) { PauseGame(1); Pause = false; }
		if (hoch && Mathf.Ceil(player.transform.position.z) == 235) { ResumeGame(); Stair(player.transform.position.z); hoch = false; lastObject = Instantiate(floor, new Vector3(0, 21, 255), rot); runtercounter = 0; Pause = false; }

		if (Mathf.Ceil(player.transform.position.z) == 253) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 255 && Pause) { PauseGame(4); Pause = false; Roof.SetActive(true); RoofSign.SetActive(true); }
		if (runter && Mathf.Ceil(player.transform.position.z) == 255 && runtercounter == 2) { ResumeGame(); Roof.SetActive(false); RoofSign.SetActive(false); StairReversed(player.transform.position.z); StairReversed(player.transform.position.z + 10); runter = false; Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 273) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 275 && Pause) { PauseGame(2); Pause = false; }
		if (runter && Mathf.Ceil(player.transform.position.z) == 275) { ResumeGame(); StairReversed(player.transform.position.z); runter = false; Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 285) lastObject = Instantiate(floor, new Vector3(0, 0, 295), rot);
		if (Mathf.Ceil(player.transform.position.z) == 295) lastObject = Instantiate(floor, new Vector3(0, 0, 305), rot);

		if (Mathf.Ceil(player.transform.position.z) == 303) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 305 && Pause) { PauseGame(1); Pause = false; }
		if (hoch && Mathf.Ceil(player.transform.position.z) == 305) { ResumeGame(); Stair(player.transform.position.z); hoch = false; Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 313) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 315 && Pause) { PauseGame(5); Pause = false; schießcounter = 0; }
		if (schieß && Mathf.Ceil(player.transform.position.z) == 315) { ResumeGame(); Laserheight = 6; LaserForTutorial(); lastObject = Instantiate(floor, new Vector3(0, 7, 325), rot); schießcounter = 0; Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 325) lastObject = Instantiate(floor, new Vector3(0, 7, 335), rot);

		if (Mathf.Ceil(player.transform.position.z) == 333) Pause = true;
		if (schieß && Mathf.Ceil(player.transform.position.z) == 335 && schießcounter == 2) { ResumeGame(); Laserheight = 2; LaserForTutorial(); StartCoroutine(CoForLaser(0.8f)); lastObject = Instantiate(floor, new Vector3(0, 7, 345), rot); Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 335 && Pause) { PauseGame(6); Pause = false; schießcounter = 0; }

		if (Mathf.Ceil(player.transform.position.z) == 345) lastObject = Instantiate(floor, new Vector3(0, 7, 355), rot);
		if (Mathf.Ceil(player.transform.position.z) == 355) lastObject = Instantiate(floor, new Vector3(0, 7, 365), rot);

		if (Mathf.Ceil(player.transform.position.z) == 353) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 365 && Pause) { PauseGame(2); Pause = false; }
		if (runter && Mathf.Ceil(player.transform.position.z) == 365) { ResumeGame(); StairReversed(player.transform.position.z); runter = false; Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 373) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 375 && Pause) { PauseGame(5); Pause = false; schießcounter = 0; }
		if (schieß && Mathf.Ceil(player.transform.position.z) == 375) { ResumeGame(); LaserForTutorial(); lastObject = Instantiate(floor, new Vector3(0, 0, 385), rot); schießcounter = 0; Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 385) lastObject = Instantiate(floor, new Vector3(0, 0, 395), rot);
		if (Mathf.Ceil(player.transform.position.z) == 393) Pause = true;
		if (Mathf.Ceil(player.transform.position.z) == 395 && Pause) { PauseGame(6); Pause = false; schießcounter = 0; }
		if (schieß && Mathf.Ceil(player.transform.position.z) == 395 && schießcounter == 2) { ResumeGame(); Laserheight = 0; LaserForTutorial(); StartCoroutine(CoForLaser(0.8f)); lastObject = Instantiate(floor, new Vector3(0, 0, 405), rot); schießcounter = 0; Pause = false; }
		if (Mathf.Ceil(player.transform.position.z) == 405) lastObject = Instantiate(floor, new Vector3(0, 0, 415), rot);
		if (Mathf.Ceil(player.transform.position.z) == 415) lastObject = Instantiate(floor, new Vector3(0, 0, 425), rot);
		if (Mathf.Ceil(player.transform.position.z) == 425) lastObject = Instantiate(floor, new Vector3(0, 0, 435), rot);
		if (Mathf.Ceil(player.transform.position.z) == 433) Pause = true;                                     //Glowbox
		if (Mathf.Ceil(player.transform.position.z) == 435 && Pause) { PauseGame(7); Pause = false; Health.SetActive(true); GlowboxSign.SetActive(true); schieß = false; }
		if (schieß && Mathf.Ceil(player.transform.position.z) == 435) { Pause = false; ResumeGame(); Health.SetActive(false); GlowboxSign.SetActive(false); lastObject = Instantiate(floor, new Vector3(0, 0, 445), rot); lastObject = Instantiate(floor, new Vector3(0, 0, 455), rot); Loading.value = 1; schieß = false;  Pause = false; }
		if (schieß && Mathf.Ceil(player.transform.position.z) == 455 && Time.timeScale==0) {  ResumeGame(); TaptoPlay.SetActive(false); lastObject = Instantiate(floor, new Vector3(0, 0, 465), rot);schieß = false;  }   
		if (Mathf.Ceil(player.transform.position.z) == 455 && Time.timeScale == 1 && p == 0) { PauseGame(7); TaptoPlay.SetActive(true); p++; } 			
		
	}
		void PauseGame(int i)
	{
		Time.timeScale = 0;
		switch (i)
		{
			case 1: SwipeUp.SetActive(true); SwipeDown.SetActive(false); xSwipeUp.SetActive(false); xSwipeDown.SetActive(false); Tap.SetActive(false); xTap.SetActive(false); break;
			case 2: SwipeUp.SetActive(false); SwipeDown.SetActive(true); xSwipeUp.SetActive(false); xSwipeDown.SetActive(false); Tap.SetActive(false); xTap.SetActive(false); break;
			case 3: SwipeUp.SetActive(false); SwipeDown.SetActive(false); xSwipeUp.SetActive(true); xSwipeDown.SetActive(false); Tap.SetActive(false); xTap.SetActive(false); break;
			case 4: SwipeUp.SetActive(false); SwipeDown.SetActive(false); xSwipeUp.SetActive(false); xSwipeDown.SetActive(true); Tap.SetActive(false); xTap.SetActive(false); break;
			case 5: SwipeUp.SetActive(false); SwipeDown.SetActive(false); xSwipeUp.SetActive(false); xSwipeDown.SetActive(false); Tap.SetActive(true); xTap.SetActive(false); break;
			case 6: SwipeUp.SetActive(false); SwipeDown.SetActive(false); xSwipeUp.SetActive(false); xSwipeDown.SetActive(false); Tap.SetActive(false); xTap.SetActive(true); break;
			case 7: SwipeUp.SetActive(false); SwipeDown.SetActive(false); xSwipeUp.SetActive(false); xSwipeDown.SetActive(false); Tap.SetActive(false); xTap.SetActive(false); break;
		} 
	}

	void ResumeGame()
	{
		Time.timeScale = 1;
		SwipeUp.SetActive(false); SwipeDown.SetActive(false); xSwipeUp.SetActive(false); xSwipeDown.SetActive(false); Tap.SetActive(false); xTap.SetActive(false);
	}

	IEnumerator CoForLaser(float t)
	{
		yield return new WaitForSeconds(t);
		LaserForTutorial();
	}


	public void SwipeMobile()
	{
		if (Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				firstPressPos = new Vector2(t.position.x, t.position.y);
			}
			if (t.phase == TouchPhase.Ended)
			{
				secondPressPos = new Vector2(t.position.x, t.position.y);
				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				if (currentSwipe.x > minSwipe || currentSwipe.y > minSwipe || currentSwipe.x < -minSwipe || currentSwipe.y < -minSwipe)
				{
					currentSwipe.Normalize();
					if (currentSwipe.y >= 0.5f) { hoch = true; hochcounter++; }
					if (currentSwipe.y <= -0.5f) runter = true; { runtercounter++; }
				}	 
				else { schieß = true; schießcounter++;  }
			}	  
		}
		
	}

	void DamagePlayerAndLoading()
	{
	if (Timer > 0 && NeededBattery<0)			//Damage
		{
			Timer -= Time.deltaTime;
		}
	if (Loading.value <= 0)
		{
			playerscript.gameover = true;
			QuitGame();
		}	
	if (Timer<= 0 && Loading.value>=0)		  //when one second is over damage the player
		{
			Loading.value = Loading.value - 0.045f;
			Timer = 1f;			   // every second 0.05f damage //set Timer again to 1second
		}
/*	else if (Loading.value<=0)
	{
			playerscript.animator.SetTrigger("Die");
	} */

	if (NeededBattery>=0 && TimerLoading>0)				 //smooth Loading
		{
			TimerLoading -= Time.deltaTime;
		}
	if (TimerLoading <= 0)								 //every 0.1 seconds load 0.1f 
		{	
		Loading.value += 0.25f;
		TimerLoading = 0.04f;  
	    NeededBattery = NeededBattery - 1;				   //needed battery range 0-10 integer
	   }

   }

   void deleteLastObjects() //deleting builded Objects //Floar,Stair,Rev..
   {

	   if (builtCounter == 1)
	   {
		   builtCounter = 0; deleted.Add(lastObject); if (deleted.Count == 10) { Destroy(deleted[0]); deleted.RemoveAt(0); }
	   }
   }
   void overLineUpdate()
   {

	   if (player.transform.position.z > (nextLine))     //+1, to prevent that position.z is roundup to the same nextLine as the last and building on each other
	   {
		   overLine = true;
	   }
	   else
	   {
		   overLine = false;
	   }


   }

	void Floor()
	{
		if ((Mathf.Round(player.transform.position.z) % 10 == 9) && overLine == true)
		{
			nextLine = RoundUp(player.transform.position.z);     //More Time to Build Stair or StairReversed
			lastObject = Instantiate(floor, new Vector3(0, Stage, nextLine + 5), rot);
			lastFloor = lastObject;
			builtCounter += 1;
			Laserheight = 3;	
			wasFloor = true;
			if (lastStair == null) lastStair = RandomObject; if (lastReverse == null) lastReverse = RandomObject; if (nextLine == lastStair.transform.position.z || nextLine == lastReverse.transform.position.z) Destroy(lastFloor);
		}
	}

	void Stair(float z)
	{
		float correction = 0;
		bool überuns = false;
		bool nextüberuns = false;
		bool nextnextüberuns = false;
		if ((Physics.OverlapBox(new Vector3(0, Stage + 3.5f+7, RoundDown(z+1) + 5), new Vector3(2, 2f, 2f), rot, ~ignoredLayer)).Length == 0) überuns = true;
		if ((Physics.OverlapBox(new Vector3(0, Stage +3.5f+7, RoundUp(z) + 5), new Vector3(2, 2f, 2f), rot, ~ignoredLayer)).Length == 0) nextüberuns = true;
		if ((Physics.OverlapBox(new Vector3(0, Stage + 3.5f+7, RoundUp(z + 10) + 5), new Vector3(2, 2f, 2f), rot, ~ignoredLayer)).Length == 0) nextnextüberuns = true;
		if ((Mathf.Round(z) % 10 <= 3))
		{
			if (überuns && wasFloor) { nextLine = RoundDown(z+1); player.transform.Translate(Vector3.up* Time.deltaTime*10,Space.World); } else if (nextüberuns) nextLine = RoundUp(z); else if (nextnextüberuns) { nextLine = RoundUp(z + 10); lastObject = Instantiate(floor, new Vector3(0, Stage, RoundUp(z) + 5), rot); lastFloor = lastObject; } else return;
		}
		else if ((Mathf.Round(z) % 10 > 3))
		{
			if (nextüberuns) nextLine = RoundUp(z); else if (nextnextüberuns) { nextLine = RoundUp(z + 10); lastObject = Instantiate(floor, new Vector3(0, Stage, RoundUp(z) + 5), rot); lastFloor = lastObject; } else return;
		}
		else return;
		if (lastStair == null) lastStair = RandomObject; if (lastReverse == null) lastReverse = RandomObject; 
		if (lastStair.transform.position.z == nextLine || lastReverse.transform.position.z==nextLine) return;
		if (Physics.OverlapBox(new Vector3(0, Stage + 3.5f, nextLine + 5), new Vector3(2, 2f, 2f), rot, ~checkForGlowbox).Length > 0) correction = 0.01f;
		if (lastFloor == null) lastFloor = RandomObject; if (nextLine == lastFloor.transform.position.z) Destroy(lastFloor);
		Laserheight = 6f;
		lastObject = Instantiate(stair, new Vector3(0 - correction, Stage - correction, nextLine), rot);
		hoch = false;
		Stage = Stage + 7;
		builtCounter += 1;
		lastStair = lastObject;
		wasFloor = false;
	}						 
	void StairReversed(float z)
	{
		float correction = 0;
		float lastLine = nextLine;
		bool unteruns = false;
		bool nextunteruns = false;
		bool nextnextunteruns = false;
		if (lastStair == null) lastStair = RandomObject;
		if ((Physics.OverlapBox(new Vector3(0, Stage - 3.5f, RoundDown(z)+5), new Vector3(2, 2f, 2f), rot, ~ignoredLayer)).Length == 0) unteruns = true;
		if ((Physics.OverlapBox(new Vector3(0, Stage - 3.5f, RoundUp(z) + 5), new Vector3(2, 2f, 2f), rot, ~ignoredLayer)).Length == 0) nextunteruns = true;
		if ((Physics.OverlapBox(new Vector3(0, Stage - 3.5f, RoundUp(z+10) + 5), new Vector3(2, 2f, 2f), rot, ~ignoredLayer)).Length == 0) nextnextunteruns = true;
		if ((Mathf.Round(z) % 10 <= 5))
		{
			if (unteruns && wasFloor && lastStair.transform.position.z != RoundDown(z)) { nextLine = RoundDown(z); Destroy(lastFloor); } else if (nextunteruns) { nextLine = RoundUp(z); } else if (nextnextunteruns) { nextLine = RoundUp(z + 10);lastObject = Instantiate(floor, new Vector3(0, Stage, RoundUp(z) + 5), rot); lastFloor = lastObject; } else return;
		}
		else if ((Mathf.Round(z) % 10 > 5))
		{
			if (nextunteruns && Stage!=0) { nextLine = RoundUp(z); } else if (nextnextunteruns) { nextLine = RoundUp(z + 10); lastObject = Instantiate(floor, new Vector3(0, Stage, RoundUp(z) + 5), rot); lastFloor = lastObject; } else return;
		}
		else return;
		if (nextLine < lastLine) return; 
		if (lastStair == null) lastStair = RandomObject; if (lastReverse == null) lastReverse = RandomObject; 
		if ((lastStair.transform.position.z == nextLine || lastReverse.transform.position.z == nextLine)) return;
		if (Physics.OverlapBox(new Vector3(0, Stage - 3.5f, nextLine+ 5), new Vector3(2, 2f, 2f), rot, ~checkForGlowbox).Length > 0) correction = 0.01f;
		Laserheight = -1f;        ///1 für arda
		Stage = Stage - 7;
		lastObject = Instantiate(stairReversed, new Vector3(0 - correction, Stage - correction, nextLine), rot);
		runter = false;
		builtCounter += 1;
		lastReverse = lastObject;
		wasFloor = false;
	}

	void LaserForTutorial()
	{
		randomNumber = Random.Range(1, 4);
		schieß = false;	  	
		
		AttackSound.Play(0);	
		shot = true;													
		Loading.value = Loading.value - 0.1f;
		Physics.Raycast(new Vector3(0, player.transform.position.y + Laserheight, player.transform.position.z), Vector3.forward, out Laserhit, 500, ~LaserIgnore);
		StartCoroutine(Explosion());
	
		AttackPart.Play();
		playerscript.animator.ResetTrigger("Energy");
		playerscript.animator.SetTrigger("Shooting" + randomNumber);
	}  
	IEnumerator Explosion()
	{
		yield return new WaitForSeconds(0.25f);
		GameObject x = Instantiate(ExplosionPart.gameObject, new Vector3(2, Laserhit.transform.position.y, Laserhit.transform.position.z), rot);
		x.GetComponent<ParticleSystem>().Play();
		Destroy(x, 4);
		Destroy(Laserhit.transform.gameObject);
	}
	
	void Laser()
   {
		AttackPart.Play();
		schieß = false;
		randomNumber = Random.Range(1, 4);
		AttackSound.Play(0); 
		playerscript.animator.ResetTrigger("Energy");
		playerscript.animator.SetTrigger("Shooting"+randomNumber);
		shot = true;
	    Loading.value = Loading.value - 0.1f;                 //0.2f	arda
		bool tr = false;
		//if (lastObject.gameObject != null) if ((lastObject.layer == LayerMask.NameToLayer("StairReversed") || lastObject.layer == LayerMask.NameToLayer("Stair"))) tr = true;
		int u = 4;
		if (playerscript.speed >= 11.9f) u = 12;
		if (Physics.Raycast(new Vector3(0, player.transform.position.y+Laserheight, player.transform.position.z), Vector3.forward, out Laserhit, 500, ~LaserIgnore)==true){ 
		if ((((Laserhit.transform.position.z - player.transform.position.z - 5) < u)))
		{
				//Debug.Log(Laserhit.transform.gameObject.name);
			LaserProgress = 0f;
		}
		else if (((Laserhit.transform.position.z - player.transform.position.z - 5) < 12) )
			{
				//Debug.Log(Laserhit.transform.gameObject.name);
				LaserProgress = 0.2f;
			}
			else
		{
				LaserProgress = 0.4f;
				//LaserProgress = 0;
		}
		}
   }


	void LaserUpdate()
   {

	   if (LaserProgress > 0)
	   {
		   LaserProgress -= Time.deltaTime;
	   }
	   if (LaserProgress <=0) {           //When the Laser is at the Target Destruction 
			if (Laserhit.transform != null)
			{	
				Destruction();
				GameObject x= Instantiate(ExplosionPart.gameObject, new Vector3(2, Laserhit.transform.position.y, Laserhit.transform.position.z), rot);
				x.GetComponent<ParticleSystem>().Play();
				Destroy(x, 4);
			}		
		   shot = false;
	   }
   }
   void Destruction()
   {
		   if (Laserhit.transform.gameObject.layer == LayerMask.NameToLayer("Box") || Laserhit.transform.gameObject.layer == LayerMask.NameToLayer("BoxProtection"))
		   {
			    if (Laserhit.transform.gameObject.layer == LayerMask.NameToLayer("Box"))
			   {
				   GameObject.Find(Laserhit.transform.name).SetActive(false);
			   }
			   else if (Laserhit.transform.gameObject.layer == LayerMask.NameToLayer("BoxProtection"))
				{
					GameObject.Find(Laserhit.transform.parent.name).SetActive(false);
				}
		   }
   }
  
  

		int RoundUp(float ttoRound)
	{														  
		int toRound = (int)ttoRound;
		if (toRound % 10 == 0) return toRound;
		return (10 - toRound % 10) + toRound;
	}
	int RoundDown(float ttoRound)
	{
		int toRound = (int)ttoRound;
		return toRound - toRound % 10;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 14 && playerscript.gameover==false)			   //Glowbox
		{
			NeededBattery = (1 - Loading.value) * 10;
			TimerLoading = 0.1f;
			playerscript.animator.SetTrigger("Energy");
			LoadPart.Play();
			ShieldSound.Play(0);
			trg = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 14)
		{
			trg = false;
			LoadPart.Stop();
		}
	}
	
  


	public void QuitGame()
	{
		// save any game data here
#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		//UnityEditor.EditorApplication.isPlaying = false;
		//StartCoroutine(CoroutineForEndingGame(time));
		if (lastReverse == null) lastReverse = RandomObject;
		if (lastStair == null) lastStair = RandomObject;
		if (RoundDown(gameObject.transform.position.z - 10) == lastStair.transform.position.z) lastCol = 1;
		if (RoundDown(gameObject.transform.position.z - 10) == lastReverse.transform.position.z) lastCol = 2;
		StartCoroutine(AudioFadeOut.FadeOut(Music, 1.5f));
		playerscript.animator.SetBool("Die", true);
		Loadbar.SetActive(false);
		SecondMenu.SetActive(true);
		//highscorescript.Points = (Int32.Parse(points.text));
		highscorescript.enabled = true;
		//yield return new WaitForSeconds(time);
		menuscript.OutlineEarth.SetFloat("_Width", 0.02f);   //Only for visual purpose
		menuscript.OutlineAir.SetFloat("_Width", 0.02f);
		//UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
	}
	/*IEnumerator CoroutineForEndingGame(float time)
	{
		playerscript.animator.SetBool("Die", true);
		Loadbar.SetActive(false);
		SecondMenu.SetActive(true);
		highscorescript.Points = Convert.ToInt32(points.text);
		highscorescript.enabled = true;
		yield return new WaitForSeconds(time);
		menuscript.OutlineEarth.SetFloat("_Width", 0.02f);	 //Only for visual purpose
		menuscript.OutlineAir.SetFloat("_Width", 0.02f);
		UnityEditor.EditorApplication.isPlaying = false;
	}									*/




}



