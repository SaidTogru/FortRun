using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	// Start is called before the first frame update	 
	Vector2 firstPressPos;               //Swipe
	Vector2 secondPressPos;
	Vector2 currentSwipe;	
	[SerializeField] Text TaptoPlay,InfoPrice,InfoHighscore;
	public bool ready = true;
	int minSwipe = 20;                   // GameStart
	LoadbarScript loadbarscript;
	GameObject FirstGround;
	public bool gamestarted = false;
	[SerializeField] GameObject cameraObject;
	CameraControll camerascript;
	Camera camera;
	GameObject Light;
	[SerializeField] GameObject generating;
		
	[SerializeField] GameObject Earth, Fire, Air, Bolt, Dark;         //SwipeAndLerpThePlayer
	[SerializeField] Color32 EarthColor, FireColor, AirColor, BoltColor, DarkColor, EarthColorBackUp, FireColorBackUp, AirColorBackUp, BoltColorBackUp, DarkColorBackUp;
	[SerializeField] Text EarthText, FireText, AirText, BoltText, DarkText;
	[SerializeField] GameObject EarthCircle, AirCircle,FireCircle,  BoltCircle, DarkCircle;
	[SerializeField] GameObject EarthPoint,AirPoint, FirePoint, BoltPoint, DarkPoint;
	[SerializeField] MusicPlayer musicscript;
	Color32 CurrentColor, NextColor;
	GameObject SelectedPlayer, PreviousPlayer;
	bool shouldLerp = false;
	float timeStartedLerping;
	float lerpTime = 0.6f;
	public Vector3 startPosition;
	public Vector3 endPosition;
	public bool swiped = true;
	bool Right;
	//[SerializeField] GameObject TaptoPlay;
	Coroutine toStop, TexttoStop;
	public int playerNumber = 1;
	[SerializeField] public Material OutlineEarth, OutlineAir;
	
	[SerializeField] GameObject DotObject;
	Image Dot;
	[SerializeField] GameObject MenuSlide;
	[SerializeField] AudioSource buySound;


	void Start()
	{
		PlayerPrefs.SetInt("CurrentPlayer", 1);
	/*	PlayerPrefs.SetInt("highscore", 0);       //RESET
		PlayerPrefs.SetInt("Speed", 0);
		;           //RESET	
		PlayerPrefs.SetInt("Tutorial", 0);    //RESET
		Debug.Log("Highscore "+ PlayerPrefs.GetInt("highscore"));
		Debug.Log("Speed " + PlayerPrefs.GetInt("speed"));
		Debug.Log("CurrentPlayer " + PlayerPrefs.GetInt("CurrentPlayer"));
		Debug.Log("Tutorial " + PlayerPrefs.GetInt("Tutorial"));	*/
		playerNumber = PlayerPrefs.GetInt("CurrentPlayer");
		camerascript = cameraObject.GetComponent<CameraControll>();		
		Dot = DotObject.GetComponent<Image>();
		camera = cameraObject.GetComponent<Camera>();
		FirstGround = GameObject.Find("FirstGround");
		Light = GameObject.Find("Directional Light");
		OutlineEarth.SetFloat("_Width", 0.02f);   //Only for visual purpose
		OutlineAir.SetFloat("_Width", 0.02f);
	/*	SelectedPlayer = PreviousPlayer = Earth;
		CurrentColor = NextColor = EarthColor;
		EarthText.enabled = true;	  */
		if (PlayerPrefs.GetInt("CurrentPlayer")==1) camera.backgroundColor = CurrentColor= EarthColor;
		if (PlayerPrefs.GetInt("CurrentPlayer") == 2) camera.backgroundColor = CurrentColor= AirColor;
		if (PlayerPrefs.GetInt("CurrentPlayer") == 3) camera.backgroundColor = CurrentColor = FireColor;
		if (PlayerPrefs.GetInt("CurrentPlayer") == 4) camera.backgroundColor = CurrentColor = BoltColor; 
		if (PlayerPrefs.GetInt("CurrentPlayer") == 5) camera.backgroundColor = CurrentColor = DarkColor;
		//Debug.Log(PlayerPrefs.GetInt(playerNumber.ToString()));
		PlayerPrefs.SetInt(1.ToString(), 1);
	/*	PlayerPrefs.SetInt(2.ToString(), 0);	  //RESET
		PlayerPrefs.SetInt(3.ToString(), 0);
		PlayerPrefs.SetInt(4.ToString(), 0);
		PlayerPrefs.SetInt(5.ToString(), 0);	  	*/
		switchPlayer(true);
	}

	void Update()
	{
		if (gamestarted==false) SwipePC();

		if (shouldLerp)
		{
			SelectedPlayer.transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);
			if (!Right) PreviousPlayer.transform.position = Lerp(endPosition, new Vector3(startPosition.x - 10, startPosition.y, startPosition.z), timeStartedLerping, lerpTime);
			else
				PreviousPlayer.transform.position = Lerp(endPosition, new Vector3(startPosition.x + 10, startPosition.y, startPosition.z), timeStartedLerping, lerpTime);
		}
	}


	public void SwipePC()
	{
		if (Input.GetMouseButtonDown(0))
		{
			firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
	
		if (Input.GetMouseButtonUp(0))
		{
			secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
			//currentSwipe.Normalize();
			
			if (currentSwipe.x > minSwipe || currentSwipe.y > minSwipe || currentSwipe.x < -minSwipe || currentSwipe.y < -minSwipe)
			{
				currentSwipe.Normalize();
				if (currentSwipe.y >= 0.8f)                   //Up
				{
					Buy(playerNumber);
				}
		/*		if (currentSwipe.y <= -0.9f)                    //Down
				{

				}			  */
				if (currentSwipe.x >= 0.7f && ready)       //Right
				{
					Right = false;
					ready = false; EarthText.enabled = false; AirText.enabled = false; FireText.enabled = false; BoltText.enabled = false; DarkText.enabled = false;
					switchPlayer(false);	 
					StartLerping();
				}
				if (currentSwipe.x <= -0.7f && ready)
				{        //Left	

					Right = true;
					ready = false; EarthText.enabled = false; AirText.enabled = false; FireText.enabled = false; BoltText.enabled = false; DarkText.enabled = false;
					switchPlayer(false);
					StartLerping();
				}
				if (PlayerPrefs.GetInt(playerNumber.ToString()) == 0) TaptoPlay.text = "Tap to Info"; else TaptoPlay.text = "Tap to Play";
			}
			else
			{
				if (ready && PlayerPrefs.GetInt(playerNumber.ToString()) == 1) StartCoroutine(CoroutineStart(0.3f)); else if (ready && PlayerPrefs.GetInt(playerNumber.ToString()) == 0 && InfoPrice.enabled==false) GetInfo(playerNumber);
				//Buy(playerNumber);
			}

		}  

	}


	/* private void GetInfo(int i)
	  {
		  //string x = "Break the Highscore of ö or Double Tap to buy it for ü";
		  string x = "Break the highscore of ö \n or \nDouble Tap to buy it for ü";
		  switch (i)
		  {
			  case 2:	x  =x.Replace("ö", "<b><color=#000000>250</color></b>"); x=x.Replace("ü", "<b><color=#000000>0.99$</color></b>"); break;
			  case 3: x = x.Replace("ö", "<b><color=#000000>500</color></b>"); x = x.Replace("ü", "<b><color=#000000>0.99$</color></b>"); break;
			  case 4: x = x.Replace("ö", "<color=#2B2B2B>750</color>"); x = x.Replace("ü", "<color=#2B2B2B>0.99$</color>"); break;
			  case 5: x = x.Replace("ö", "<b><color=#000000>1000</color></b>"); x = x.Replace("ü", "<b><color=#000000>9.99$</color></b>"); break;
		  }	
		  Info.text = x; 	 
		  Info.enabled = true;
		  StartCoroutine(Disabletext());
	  }	 */
	private void GetInfo(int i)
	{
		//string x = "Break the Highscore of ö or Double Tap to buy it for ü";
		string x = "Needed Highscore: ö";
		string y = "Swipe Up to buy: ü";
		InfoPrice.color = new Color32(0, 0, 0, 255); InfoHighscore.color = new Color32(0, 0, 0, 255);
		switch (i)
		{
			case 2: x = x.Replace("ö", "<b><color=#BEBEBE>250</color></b>"); y = y.Replace("ü", "<b><color=#BEBEBE>0.99$</color></b>"); break;
			case 3: x = x.Replace("ö", "<b><color=#9A0005>500</color></b>"); y = y.Replace("ü", "<b><color=#9A0005>1.99$</color></b>");  break;
			case 4: x = x.Replace("ö", "<color=#303030>750</color>"); y = y.Replace("ü", "<color=#303030>2.99$</color>"); break;
			case 5: x = "\nö"; y = "\nü"; x = x.Replace("ö", "<b><color=#000000>1000</color></b>"); y = y.Replace("ü", "<b><color=#000000>3.99$</color></b>");  InfoPrice.color = new Color32(0, 0, 0, 255); InfoHighscore.color = new Color32(0, 0, 0, 255); break;
		}
		InfoPrice.text = y;
		InfoHighscore.text = x;
		InfoHighscore.enabled = true;
		InfoPrice.enabled = true;
		toStop= StartCoroutine(Disabletext());
	}

	public void Buy(int i)
	{
		switch (i)
		{
			case 2: if (PlayerPrefs.GetInt(2.ToString()) == 0) { IAPManager.instance.BuyAir(); PlayerPrefs.SetInt("2", 1); buySound.Play(0); PlayerPrefs.SetInt("Music", 1); } break;
			case 3: if (PlayerPrefs.GetInt(3.ToString()) == 0) { IAPManager.instance.BuyFire(); PlayerPrefs.SetInt("3", 1); buySound.Play(0); PlayerPrefs.SetInt("Music", 1); } break;
			case 4: if (PlayerPrefs.GetInt(4.ToString()) == 0) { IAPManager.instance.BuyBolt(); PlayerPrefs.SetInt("4", 1); buySound.Play(0); PlayerPrefs.SetInt("Music", 1); } break;
			case 5: if (PlayerPrefs.GetInt(5.ToString()) == 0) { IAPManager.instance.BuyDark(); PlayerPrefs.SetInt("5", 1); buySound.Play(0); PlayerPrefs.SetInt("Music", 1); } break;
		}
		
	}
	/*	public void SwipeMobile()
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
						if (currentSwipe.y >= 0.9f)
						{
						}
						if (currentSwipe.y <= -0.9f)
						{
						}
						if (currentSwipe.x >= 0.9f)
						{
						}
						if (currentSwipe.x <= -0.9f)
						{
						}
					}
				}
			}
		}*/

	private void StartLerping()
	{
		timeStartedLerping = Time.time;
		if (!Right) startPosition = new Vector3(5, 0, 165); else startPosition = new Vector3(-5, 0, 165);					 //set 145 to change animation of sliding
		shouldLerp = true;
	}

	public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime)
	{
		PreviousPlayer.SetActive(true);
		float timeSinceStarted = Time.time - timeStartedLerping;
		float percentageComplete = timeSinceStarted / lerpTime;                //percentage Complete is LerpTimer
		var result = Vector3.Lerp(start, end, percentageComplete);
		camera.backgroundColor = Color.Lerp(CurrentColor, NextColor, percentageComplete);

		if (percentageComplete >= 1f)
		{
			CurrentColor = NextColor;
			PreviousPlayer.SetActive(false);
			shouldLerp = false;
			percentageComplete = 0;
		}

		return result;
	}

	private void switchPlayer(bool start)
	{
		if (!start) if (!Right) playerNumber++; else playerNumber--;
		if (playerNumber == 6) playerNumber = 1;
		if (playerNumber == 0) playerNumber = 5;

		switch (playerNumber)
		{
			case 1:
				NextColor = EarthColor;
				PreviousPlayer = SelectedPlayer;
				SelectedPlayer = Earth;
				EarthText.enabled = true; 
				Earth.SetActive(true);
				Air.SetActive(false);
				Fire.SetActive(false);
				Bolt.SetActive(false);
				Dark.SetActive(false);
				Dot.color = new Color32(106, 60, 53, 150);
				Dot.transform.position = EarthCircle.transform.position;
				TexttoStop= StartCoroutine(CoroutineEnableText(EarthText)); 
				break;
			case 2:
				NextColor = AirColor;
				PreviousPlayer = SelectedPlayer;
				SelectedPlayer = Air;
				AirText.enabled = true;
				Earth.SetActive(false);
				Air.SetActive(true);
				Fire.SetActive(false);
				Bolt.SetActive(false);
				Dark.SetActive(false);
				Dot.color = new Color32(255, 255, 255, 80);
				Dot.transform.position = AirCircle.transform.position;
				TexttoStop = StartCoroutine(CoroutineEnableText(AirText));
				break;
			case 3:
				NextColor = FireColor;
				PreviousPlayer = SelectedPlayer;
				SelectedPlayer = Fire;
				FireText.enabled = false;
				Earth.SetActive(false);
				Air.SetActive(false);
				Fire.SetActive(true);
				Bolt.SetActive(false);
				Dark.SetActive(false);
				Dot.color = new Color32(255, 0, 0, 70);
				Dot.transform.position = FireCircle.transform.position;
				TexttoStop = StartCoroutine(CoroutineEnableText(FireText));
				break;

			case 4:
				NextColor = BoltColor;
				PreviousPlayer = SelectedPlayer;
				SelectedPlayer = Bolt;
				BoltText.enabled = false; 
				Earth.SetActive(false);
				Air.SetActive(false);
				Fire.SetActive(false);
				Bolt.SetActive(true);
				Dark.SetActive(false);
				Dot.color = new Color32(53, 53, 53, 130);
				Dot.transform.position = BoltCircle.transform.position;
				TexttoStop = StartCoroutine(CoroutineEnableText(BoltText));
				break;

			case 5:
				NextColor = DarkColor;
				PreviousPlayer = SelectedPlayer;
				SelectedPlayer = Dark;
				DarkText.enabled = false; AirText.enabled = false; FireText.enabled = false; BoltText.enabled = false; DarkText.enabled = true;
				Earth.SetActive(false);
				Air.SetActive(false);
				Fire.SetActive(false);
				Bolt.SetActive(false);
				Dark.SetActive(true);
				Dot.color = new Color32(0, 0, 0, 40);
				Dot.transform.position = DarkCircle.transform.position;
				TexttoStop = StartCoroutine(CoroutineEnableText(DarkText));
				break;
			default:
				Debug.Log("Fehler!");
				break;
		}
	}

	IEnumerator CoroutineStart(float time)
	{
		MenuSlide.SetActive(false);
		PlayerPrefs.SetInt("CurrentPlayer", playerNumber);
		generating.SetActive(true);	 
		camerascript.gameObject.GetComponent<ColorCycler>().enabled = true;
		EarthText.enabled = false; AirText.enabled = false; FireText.enabled = false; BoltText.enabled = false; DarkText.enabled = false;
		if (TexttoStop!=null) StopCoroutine(TexttoStop);
		if (playerNumber == 1) EarthPoint.SetActive(true); else if (playerNumber == 2) AirPoint.SetActive(true); else if (playerNumber == 3) FirePoint.SetActive(true); else if (playerNumber == 4) BoltPoint.SetActive(true); else if (playerNumber == 5) DarkPoint.SetActive(true);
		loadbarscript = GameObject.Find("LoadbarControll").GetComponent<LoadbarScript>();
		camerascript.enabled = true;
		loadbarscript.LoadbarObject.SetActive(true);
		FirstGround.GetComponent<MeshRenderer>().enabled = true;
		Light.transform.position = new Vector3(0, 3, 0);
		DotObject.SetActive(false);
		InfoPrice.enabled = false; InfoHighscore.enabled = false;
		//Light.transform.eulerAngles = new Vector3(50, -30, 0);
		Light.transform.eulerAngles = new Vector3(36, 336.5f, 9.5f);  
		SelectedPlayer.GetComponent<Buildsystem>().enabled = true;
		TaptoPlay.enabled = false;
		yield return new WaitForSeconds(time);
		musicscript.enabled = true;
		OutlineEarth.SetFloat("_Width", 0.1f);
		OutlineAir.SetFloat("_Width", 0.00001f);
		GameObject.Find("CharacterModel").GetComponent<Outline>().enabled = true;
	}

	IEnumerator CoroutineEnableText(Text text)
	{
		InfoPrice.enabled = false;	  if (toStop!=null) StopCoroutine(toStop);
		InfoHighscore.enabled = false;
		yield return new WaitForSeconds(lerpTime+0.05f);
		text.enabled = true;
		ready = true;
	}
	IEnumerator Disabletext()
	{
		yield return new WaitForSeconds(3);
		InfoPrice.enabled = false;
		InfoHighscore.enabled = false;
	}

}
