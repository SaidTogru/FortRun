using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
	[SerializeField] GameObject SoundON,SoundOFF,SettingsMenu;
	[SerializeField] AudioSource Music;
    // Start is called before the first frame update
	
    public void activateSettings()
	{
		SettingsMenu.SetActive(true);
	}
	public void deactivateSettings()
	{
		SettingsMenu.SetActive(false);
	}

	public void SoundOn()
	{
		Music.volume = 0.1f;
		PlayerPrefs.SetInt("Music", 1);
		SoundOFF.SetActive(false);
		SoundON.SetActive(true);
	}
	public void SoundOff()
	{
		Music.volume = 0f;
		PlayerPrefs.SetInt("Music", 0);
		SoundON.SetActive(false);
		SoundOFF.SetActive(true);
	}


	private const string AndroidRatingURI = "http://play.google.com/store/apps/details?id={0}";
	private const string iOSRatingURI = "itms://itunes.apple.com/us/app/apple-store/{0}?mt=8";

	[SerializeField] public string iOSAppID;				  // fill this

	private string url;

	void Start()
	{
		if (PlayerPrefs.GetInt("Music") == 1) { SoundON.SetActive(true); SoundOFF.SetActive(false); }
		else { SoundON.SetActive(false); SoundOFF.SetActive(true); }
#if UNITY_IOS
		if (!string.IsNullOrEmpty(iOSAppID))
		{
			url = iOSRatingURI.Replace("{0}", iOSAppID);
		}
		else
		{
			Debug.LogWarning("Please set iOSAppID variable");
		}

#elif UNITY_ANDROID
        url = AndroidRatingURI.Replace("{0}",Application.identifier);
#endif
	}

	public void OpenRating()
	{
		if (!string.IsNullOrEmpty(url))
		{
			Application.OpenURL(url);
		}
		else
		{
			Debug.LogWarning("Unable to open URL, invalid OS");
		}
	}

}
