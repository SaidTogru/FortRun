using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CountdownTimer))]

public class Fade : MonoBehaviour
{
	private CountdownTimer countdownTimer;
	private Text textUI;
	int fadeAwayCount = 1;

	void Awake()
	{
		countdownTimer = GetComponent<CountdownTimer>();
		textUI = GetComponent<Text>();
	}
	// Start is called before the first frame update
	void Start()
	{
		countdownTimer.ResetTimer(1);
	}

	// Update is called once per frame
	void Update()
	{
		if (fadeAwayCount % 2 != 0)
		{
			float alphaRemaining = countdownTimer.GetProportionTimeRemaining();
			fadeAway(alphaRemaining);
			if (alphaRemaining < 0f)
			{
				fadeAwayCount += 1;
			}
		}
		else
		{
			float increaseAlpha = countdownTimer.IncreaseAlpha();
			fadeIn(increaseAlpha);
			if (increaseAlpha > 1f)
			{
				fadeAwayCount += 1;
				countdownTimer.ResetTimer(1);
			}
		}
	}

	void fadeAway(float alphaRemaining)
	{
		Color c = textUI.color;
		c.a = alphaRemaining;
		textUI.color = c;
	}
	void fadeIn(float increaseAlpha)
	{
		Color c = textUI.color;
		c.a = increaseAlpha;
		textUI.color = c;
	}
}