using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using Facebook.Unity;
using UnityEngine.UI;

public class SceneController_mainMenu : MonoBehaviour {

	public Text       Text_bestscore;
	public GameObject UI_main;

	// Use this for initialization
	void Start () {

		Text_bestscore.text = TimerDisplay.MakeTimeString(float.Parse(SaveDataGameComponent.GetValue("playerBestTime", "0")) / 100.0f );

	}
	
	// Update is called once per frame
	void Update () {
	
	}
			
	public void LoadScene(int sceneIndex){
		SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
}
