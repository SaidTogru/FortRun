using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointsystem : MonoBehaviour
{
	GameObject Player;
	float nextPoint;
	public int i = 0;
	Text point;
	[SerializeField] Movement playerscript;
	[SerializeField] Highscore highscorescript;
    // Start is called before the first frame update
    void Start()
    {
		Player= GameObject.FindGameObjectWithTag("Player");
		point = GetComponent<Text>();
		nextPoint = Player.transform.position.z+10;
		//gameObject.GetComponent<Text>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
           if (Player.transform.position.z > nextPoint)
		{
			i += 1;
			point.text = i.ToString();
			nextPoint = RoundDown(Player.transform.position.z+10);
		}
		if (playerscript.gameover)
		{
			//if (i>175) PlayerPrefs.SetInt("Speed", PlayerPrefs.GetInt("Speed") + 1);
			highscorescript.Points = i;
		}
		   
    }
	int RoundDown(float ttoRound)
	{
		int toRound = (int)ttoRound;
		return toRound - toRound % 10;
	}
}
