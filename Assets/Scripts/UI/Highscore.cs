using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
	// Start is called before the first frame update
	public int Points;
	private int highscore;
	[SerializeField] GameObject Air,Fire,Bolt,Black;
	[SerializeField] GameObject Youtube, Settings, Share;
	[SerializeField] Text TaptoBack;
	[SerializeField] AudioSource unlockedSound,BestSound,unlockedSoundDarkEdition;
	private bool unlocked;

	void Start()
    {	
		highscore = PlayerPrefs.GetInt("highscore", 0);

		if (Points > highscore)
		{
		 PlayerPrefs.SetInt("highscore", Points);
		GetComponent<Text>().text = "Best: "+ Points.ToString();
		}
		else
		{
			GetComponent<Text>().text = "Best: "+highscore.ToString();
		}	

		if (Points >= 1000 && PlayerPrefs.GetInt("5", 0) == 0)
		{
			PlayerPrefs.SetInt("Music", 1);
			PlayerPrefs.SetInt("5", 1);
			PlayerPrefs.SetInt("4", 1);
			PlayerPrefs.SetInt("3", 1);
			PlayerPrefs.SetInt("2", 1);
			PlayerPrefs.SetInt("CurrentPlayer", 5);
			PlayerPrefs.SetInt("Speed", 2);
			//unlockedSound.Play(0);
			unlockedSoundDarkEdition.Play(0);
			Black.SetActive(true);
			LeanTween.scale(Black, new Vector3(2.3f, 4f, 1), 0.3f).setEaseLinear();
			DisableSecondMenu();
			unlocked = true;
		}
		else if (Points >= 750 && PlayerPrefs.GetInt("4", 0) == 0)
		{
			PlayerPrefs.SetInt("Music", 1);
			PlayerPrefs.SetInt("4", 1);
			PlayerPrefs.SetInt("3", 1);
			PlayerPrefs.SetInt("2", 1);
			PlayerPrefs.SetInt("CurrentPlayer", 4);
			PlayerPrefs.SetInt("Speed", 2);
			unlockedSound.Play(0);
			Bolt.SetActive(true);
			LeanTween.scale(Bolt, new Vector3(3.6f, 4f, 1), 0.3f).setEaseLinear();
			DisableSecondMenu();
			unlocked = true;
		}
		else if (Points>= 500 && PlayerPrefs.GetInt("3", 0) == 0)
		{
			PlayerPrefs.SetInt("Music", 1);
			PlayerPrefs.SetInt("3", 1);
			PlayerPrefs.SetInt("2", 1);
			PlayerPrefs.SetInt("CurrentPlayer", 3);
			PlayerPrefs.SetInt("Speed", 1);
			unlockedSound.Play(0);
			Fire.SetActive(true);
			LeanTween.scale(Fire, new Vector3(1.8f, 4f, 1), 0.3f).setEaseLinear();
			DisableSecondMenu();
			unlocked = true;
		}
		else if (Points>= 250 && PlayerPrefs.GetInt("2", 0)==0)
		{
			PlayerPrefs.SetInt("Music", 1);
			PlayerPrefs.SetInt("2", 1);
			PlayerPrefs.SetInt("CurrentPlayer", 2);
			PlayerPrefs.SetInt("Speed", 1);
			unlockedSound.Play(0);
			Air.SetActive(true);
			LeanTween.scale(Air, new Vector3(1.8f, 4f, 1), 0.3f).setEaseLinear();
			DisableSecondMenu();
			unlocked = true;
		} 

		if (!unlocked) BestSound.Play(0);
	}

 void DisableSecondMenu()
	{
		Youtube.SetActive(false);
		Share.SetActive(false);
		Settings.SetActive(false);
		TaptoBack.text = "";

	}
}
