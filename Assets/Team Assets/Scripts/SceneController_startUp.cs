using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController_startUp : MonoBehaviour {


	public GameComponentManager manager;

	// Use this for initialization
	IEnumerator Start () {


		while(manager.PercentComplete < 100.0f)
			yield return null;
		

		// Game Components are done Initializing
			
		yield return new WaitForSeconds(1.0f); 

		SceneManager.LoadSceneAsync( 1, LoadSceneMode.Single);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
