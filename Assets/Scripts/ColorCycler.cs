using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorCycler : MonoBehaviour
{	
 	public MaterialPropertyBlock block;
	int randomColor =0;
	List<Color32> AllBoxColors = new List<Color32>();
	List<Color32>  AllClearColors = new List<Color32>();
	List<Color32> AllColors = new List<Color32>();
	public Color32 ColorBackground,ColorBox,ColorLoadBox;
	public Color32 nextColorBG, nextColorBox, nextLoadColor = Color.clear;
	public GameObject lastGround, spawnGround;
	Camera camera;
	public float lerpDuration = 5; 
	float lerpTimer = 0;
	public bool isLerping = false;
	int change = 1;
	Generator generatorscript;
	int[] lightColors = { 0, 1, 2, 3, 4,5,9,10,11,12,13,14,15,16,17,18,20,24,25,26,27,28,29,30,31,32,33,34,39,40,41,42};
	int[] darkColors = { 6,7,8,19,21,22,23,35,36,37,38,43,44,45,46,47,48,49,50,51};
	Color32 StartColor;

	void Start () {
		camera = GetComponent<Camera>();
		generatorscript = GameObject.Find("GeneratingObstacles").GetComponent<Generator>();
		AllColors.Add(new Color32(204, 53,53, 255)); 	  //h			
       // AllColors.Add(new Color32(204, 53, 53, 255));	  
        AllColors.Add(new Color32(204,53,53,255)); 
        AllColors.Add(new Color32(204,53,53,255));
        AllColors.Add(new Color32(204, 53, 53, 255));
       // AllColors.Add(new Color32(204, 53, 53, 255));  
		AllColors.Add(new Color32(70, 7, 7, 0)); 	  //d
      //  AllColors.Add(new Color32(70, 7, 7, 0)); 
        //AllColors.Add(new Color32(70, 7, 7, 0));
		//AllColors.Add(new Color32(185, 107, 107,0)); 	//h
		AllColors.Add(new Color32(185, 107, 107,255));	
		AllColors.Add(new Color32(185, 107, 107, 255)); 
		//AllColors.Add(new Color32(185, 107, 107, 255)); 
		AllColors.Add(new Color32(185, 107, 107, 255));
       // AllColors.Add(new Color32(185, 107, 107, 0)); 
	   // AllColors.Add(new Color32(212, 203, 91, 0)); 
		//AllColors.Add(new Color32(82, 123, 88, 0)); 
		AllColors.Add(new Color32(82, 123, 88, 0)); 
		//AllColors.Add(new Color32(82, 123, 88, 0));	
		AllColors.Add(new Color32(69, 99, 88, 0)); 	   //d	
		//AllColors.Add(new Color32(214,138,96,0));      //h
		AllColors.Add(new Color32(69, 99, 88, 0));		//d
		AllColors.Add(new Color32(214, 138, 96, 0)); 	//d
		AllColors.Add(new Color32(69, 99, 92, 0));  	
		//AllColors.Add(new Color32(117, 117, 117, 0)); 	 //h
		AllColors.Add(new Color32(117, 117, 117, 0));  		
		AllColors.Add(new Color32(209, 137, 137, 0));
		AllColors.Add(new Color32(209, 137, 137, 0));
		AllColors.Add(new Color32(209, 137, 137, 0));
		AllColors.Add(new Color32(209, 137, 137, 0));
		AllColors.Add(new Color32(209, 137, 137, 0));
	    AllColors.Add(new Color32(209, 137, 137, 0)); 
		//AllColors.Add(new Color32(209, 137, 137, 0)); 
		AllColors.Add(new Color32(137, 191, 209, 0)); 
		AllColors.Add(new Color32(137, 191, 209, 0));
		//AllColors.Add(new Color32(137, 191, 209, 0)); 
		AllColors.Add(new Color32(137, 191, 209, 0));
		AllColors.Add(new Color32(137, 191, 209, 0));
		//AllColors.Add(new Color32(91, 52, 38,0));
		//AllColors.Add(new Color32(171, 124, 107, 0)); 	 //h
		//AllColors.Add(new Color32(171, 124, 107, 0));
		//AllColors.Add(new Color32(171,124, 107, 0)); 	 
		//AllColors.Add(new Color32(142, 110, 110, 0)); 
		//AllColors.Add(new Color32(70, 70, 7, 0));		  //d
		//AllColors.Add(new Color32(70, 70, 7, 0));
		AllColors.Add(new Color32(70, 70, 7, 0));	  
		AllColors.Add(new Color32(7, 70, 63, 0));
		AllColors.Add(new Color32(7, 70, 63, 0));
		//AllColors.Add(new Color32(7, 70, 63, 0));
		AllColors.Add(new Color32(7, 70, 8, 0));
		//AllColors.Add(new Color32(7, 70, 8, 0));
		AllColors.Add(new Color32(46, 0, 1, 0));			 


		AllBoxColors.Add(new Color32(94, 143, 171, 255)); 		  //1
       // AllBoxColors.Add(new Color32(197, 250, 143, 255));	 	//2
        AllBoxColors.Add(new Color32(91, 171, 9, 255));  		   //3
        AllBoxColors.Add(new Color32(254, 255, 28, 255));          //4
		AllBoxColors.Add(new Color32(49, 255, 255, 255));             //5
		//AllBoxColors.Add(new Color32(255, 255, 255, 255));               //6
		AllBoxColors.Add(new Color32(249, 255, 109, 255));                  //7
		//AllBoxColors.Add(new Color32(112, 115, 42, 255)); 
       // AllBoxColors.Add(new Color32(45, 46, 32, 255));	 
		//AllBoxColors.Add(new Color32(140, 140, 140, 0)); 
		AllBoxColors.Add(new Color32(170, 255, 248, 0));
		AllBoxColors.Add(new Color32(255, 203, 203, 255));	
		//AllBoxColors.Add(new Color32(239, 255, 167, 255));
		AllBoxColors.Add(new Color32(112, 255,242,255)); 
		//AllBoxColors.Add(new Color32(139,248,193,255)); 
		//AllBoxColors.Add(new Color32(192, 255, 226, 0));	
		//AllBoxColors.Add(new Color32(191, 247, 255, 0)); 	  
		AllBoxColors.Add(new Color32(85, 154, 132, 0));		    
		//AllBoxColors.Add(new Color32(138, 255, 230, 0));
		AllBoxColors.Add(new Color32(85, 154, 132, 0));
		//AllBoxColors.Add(new Color32(162, 255, 171, 0));       
		AllBoxColors.Add(new Color32(162, 255, 205, 0));       
		AllBoxColors.Add(new Color32(255, 210, 162, 0));      
		AllBoxColors.Add(new Color32(241, 189, 160, 0));              
		//AllBoxColors.Add(new Color32(255, 255, 255, 0));
		AllBoxColors.Add(new Color32(255, 255, 255, 0));
		AllBoxColors.Add(new Color32(255, 198, 200, 0));
		AllBoxColors.Add(new Color32(255, 198, 200, 0));
		AllBoxColors.Add(new Color32(255, 198, 200, 0));
		AllBoxColors.Add(new Color32(255, 198, 200, 0));
		AllBoxColors.Add(new Color32(198, 255, 250, 0));
		AllBoxColors.Add(new Color32(254, 255, 170, 0)); 
		//AllBoxColors.Add(new Color32(206, 255, 221, 0));
		AllBoxColors.Add(new Color32(97, 255, 231, 0)); 
		AllBoxColors.Add(new Color32(255, 66, 81, 0));			//drot
		//AllBoxColors.Add(new Color32(66, 255, 196,0)); 		   //grün
		AllBoxColors.Add(new Color32(252, 255, 172, 0)); 		//braungold		//STAAAAAAAAAAAAAAAAAAAAAAAAAAAART
		AllBoxColors.Add(new Color32(255, 172, 200, 0));		//hellrot
		//AllBoxColors.Add(new Color32(253, 255, 88, 0));			 //gelb
		//AllBoxColors.Add(new Color32(167, 255, 226, 0)); 		//türkis  //maybeeeeeeeeeeeeeeeeeeeeee
		//AllBoxColors.Add(new Color32(167, 205, 255, 0));	   //boringblue
		//AllBoxColors.Add(new Color32(255, 246, 167, 0));  //GOLD	
		//AllBoxColors.Add(new Color32(145, 217, 231, 0)); 	   //dunkeltürkis
		//AllBoxColors.Add(new Color32(110, 7, 7, 0));    //vermehren	//weinrot
		//AllBoxColors.Add(new Color32(111, 78, 46, 0));			//braun
		AllBoxColors.Add(new Color32(223, 255, 131, 0));		//für dunkle farben gelb
		AllBoxColors.Add(new Color32(160, 212, 255, 0));		  //hellblau
		AllBoxColors.Add(new Color32(160, 255, 239, 0));		  //türkis
		//AllBoxColors.Add(new Color32(233, 255, 131, 0));
		AllBoxColors.Add(new Color32(176, 51, 51, 0));			 //
		//AllBoxColors.Add(new Color32(229, 233, 34, 0));
		AllBoxColors.Add(new Color32(1, 55, 45, 0));



		AllClearColors.Add(new Color32(43, 103, 137, 255)); 
     //   AllClearColors.Add(new Color32(10, 128, 20, 170));
        AllClearColors.Add(new Color32(2, 51, 6, 170)); 
        AllClearColors.Add(new Color32(82, 126, 0, 170)); 
        AllClearColors.Add(new Color32(0, 166, 112, 170));
		//AllClearColors.Add(new Color32(135, 135, 135, 170));  
        AllClearColors.Add(new Color32(150, 185, 87, 170));	
		//AllClearColors.Add(new Color32(47, 75, 2, 170)); 
        //AllClearColors.Add(new Color32(0, 0, 0, 170)); 		
		//AllClearColors.Add(new Color32(56, 51, 51, 170)); 
		AllClearColors.Add(new Color32(77, 142, 111, 170));	
		AllClearColors.Add(new Color32(135, 95,73, 170)); 
		//AllClearColors.Add(new Color32(66, 115,63, 170)); 
		AllClearColors.Add(new Color32(77, 142, 111, 170));  
		//AllClearColors.Add(new Color32(77, 142, 111, 170));   
		//AllClearColors.Add(new Color32(85, 154, 132, 170));  
		//AllClearColors.Add(new Color32(90, 121, 130, 170));  			
		AllClearColors.Add(new Color32(24, 60, 36, 0));
		//AllClearColors.Add(new Color32(65, 159, 139, 170));
		AllClearColors.Add(new Color32(24, 60, 36, 0));
		//AllClearColors.Add(new Color32(76, 140, 72, 170)); 		
		AllClearColors.Add(new Color32(72, 140, 94, 170));
		AllClearColors.Add(new Color32(118, 81, 57, 170)); 
		AllClearColors.Add(new Color32(118, 81, 57, 170)); 	
		//AllClearColors.Add(new Color32(118, 118, 118, 170));
		AllClearColors.Add(new Color32(118, 118, 118, 170));
		AllClearColors.Add(new Color32(82, 53, 53, 0));
		AllClearColors.Add(new Color32(82, 53, 53, 0));
		AllClearColors.Add(new Color32(82, 53, 53, 0));
		AllClearColors.Add(new Color32(82, 53, 53, 0));			//Earth
		AllClearColors.Add(new Color32(57, 125, 96, 0));
		AllClearColors.Add(new Color32(156, 137, 64, 0)); 
		//AllClearColors.Add(new Color32(44, 113, 54, 0));	
		AllClearColors.Add(new Color32(40, 154, 110, 0)); 
		AllClearColors.Add(new Color32(118, 21, 27, 0));
		//AllClearColors.Add(new Color32(32, 135, 90,170));  
		AllClearColors.Add(new Color32(130, 117, 67, 170)); 
		AllClearColors.Add(new Color32(130, 67, 67, 70));	
		//AllClearColors.Add(new Color32(152, 150, 16, 0));
		//AllClearColors.Add(new Color32(52, 147, 103, 0)); 
		//AllClearColors.Add(new Color32(52, 112, 147, 0));
		//AllClearColors.Add(new Color32(144, 144, 85, 0));  
		//AllClearColors.Add(new Color32(33, 108, 102, 0)); 
		//AllClearColors.Add(new Color32(70, 7, 15, 0));
		//AllClearColors.Add(new Color32(94, 54, 31, 0));
		AllClearColors.Add(new Color32(139, 154, 68, 0));
		AllClearColors.Add(new Color32(53, 102, 116, 0));
		AllClearColors.Add(new Color32(88, 154, 124, 0));
		//AllClearColors.Add(new Color32(140, 128, 60, 0));
		AllClearColors.Add(new Color32(72, 9, 13, 0));
		//AllClearColors.Add(new Color32(125, 109, 5, 0));
		AllClearColors.Add(new Color32(0, 31, 20, 0));
		
		//start setzen wir die farbe die wir möchten, sowohl target als auch jetzige Farbe
		if (PlayerPrefs.GetInt("Tutorial") == 0) randomColor = 14; else randomColor= Random.Range(0, AllBoxColors.Count);		
		//if (Random.Range(0, 2) == 1) { randomColor = Random.Range(0, lightColors.Length); } else { randomColor = darkColors[randomColor]; }	
		ColorBackground = AllColors[randomColor];
		ColorBox = AllBoxColors[randomColor];
		ColorLoadBox = AllClearColors[randomColor]; 
		nextColorBG = ColorBackground; 
		nextColorBox = ColorBox;  
		nextLoadColor = ColorLoadBox; 
		block = new MaterialPropertyBlock();
		block.SetColor("_BaseColor", ColorBox);
		spawnGround.GetComponent<MeshRenderer>().SetPropertyBlock(block);
	}																																			 
	void Update()
	{
		if (isLerping && generatorscript.firstStep /*&& Time.timeScale == 1*/) UpdateLerp();
	}																								  


	// actual lerp is happening here
	void UpdateLerp()
	{
		lerpTimer += Time.deltaTime / lerpDuration;
		block.SetColor("_BaseColor", Color32.Lerp(ColorBox, nextColorBox, lerpTimer));
		List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
		meshRenderers.AddRange(spawnGround.GetComponentsInChildren<MeshRenderer>());
		meshRenderers.AddRange(lastGround.GetComponentsInChildren<MeshRenderer>());

		foreach (MeshRenderer thisMeshRenderer in meshRenderers)
		{
			if (thisMeshRenderer.transform.gameObject.layer != 14) {
			thisMeshRenderer.SetPropertyBlock(block);	   	   }
			else
			{
			 thisMeshRenderer.gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color32.Lerp(ColorLoadBox, nextLoadColor, lerpTimer));
				//thisMeshRenderer.SetPropertyBlock(block);
			}
		}		   

		camera.backgroundColor = Color.Lerp(ColorBackground, nextColorBG, lerpTimer);
		if (lerpTimer >= 1f)
		{
			isLerping = false;
			ColorBackground = nextColorBG;
			ColorBox = nextColorBox;
			ColorLoadBox = nextLoadColor;
			setNextColor();
			lerpTimer = 0;
		}
	}
	void setNextColor()
	{
	/*	if (change == 1) {
		randomColor = Random.Range(0, lightColors.Length);
			randomColor = lightColors[randomColor];
  }
		else
		{
		randomColor = Random.Range(0, darkColors.Length);
			randomColor = darkColors[randomColor];
		}		*/		
		change = change * (-1);


	/*	if (randomColor == AllColors.Count-1) {
				randomColor = 0;
					}
			else
			{
			 randomColor += 1;	
			}	*/					  
		randomColor = Random.Range(0, AllColors.Count - 1);


		
		nextColorBG = AllColors[randomColor];
		nextColorBox = AllBoxColors[randomColor];
		nextLoadColor= AllClearColors[randomColor];
	}

}