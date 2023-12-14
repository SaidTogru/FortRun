using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	// Start is called before the first frame update
	public AudioClip[] clips;
	int i = 0;
	[SerializeField] AudioSource audioSource;
	[SerializeField] MainMenu menuscript;
    void Start()
    {
		//audioSource.loop = false;
		//Randomize<AudioClip>(clips);	
		if (PlayerPrefs.GetInt("Music") == 1) audioSource.volume = 0.1f; else audioSource.volume = 0;
		audioSource.clip = clips[menuscript.playerNumber-1];
		audioSource.Play();
    }
										 
    // Update is called once per frame
    void Update()
    {
		
		/*if (!audioSource.isPlaying && audioSource.enabled)
		  {
			  audioSource.clip = clips[i];
			  audioSource.Play();
			  if (i == clips.Length) i = 0; else i++;
		  }  	   */
	}

	public static void Randomize<T>(T[] items)
	{
		System.Random rand = new System.Random();
		for (int i = 0; i < items.Length - 1; i++)
		{
			int j = rand.Next(i, items.Length);
			T temp = items[i];
			items[i] = items[j];
			items[j] = temp;
		}
	}
}
