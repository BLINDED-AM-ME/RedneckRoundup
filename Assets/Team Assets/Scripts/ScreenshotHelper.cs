using UnityEngine;
using System.Collections;

public class ScreenshotHelper : MonoBehaviour {


		public int take = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

				if(Input.GetKeyDown("k")){
						Time.timeScale = 0.0f;
				}
				if(Input.GetKeyDown("j")){
						Time.timeScale = 1.0f;
				}

				if(Input.GetKeyDown("l")){
						ScreenCapture.CaptureScreenshot("Assets/ScreenShots/Screenshot" + take + ".png");
						take++;
				}
	
	}
}
