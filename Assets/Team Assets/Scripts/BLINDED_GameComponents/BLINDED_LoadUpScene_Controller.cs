using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class BLINDED_LoadUpScene_Controller : MonoBehaviour {

	public int targetFps = 60;

	public Material loading_barMaterial;

	int progress = 0;
	int numberOfProcesses = 1;
	


	// Use this for initialization
	IEnumerator Start () {

		DontDestroyOnLoad(gameObject);

		loading_barMaterial.SetTextureOffset("_Stencil", new Vector2(0.5f,0));

		yield return new WaitForSeconds(2.0f);

		Application.targetFrameRate = targetFps;

		BLINDED_GameComponent[] comps = GetComponents<BLINDED_GameComponent>();

		numberOfProcesses = comps.Length+1;

		bool isBusy = true;
		BLINDED_GameComponent.CallBack callback = delegate { 
			isBusy = false; 
			progress++;

			loading_barMaterial.SetTextureOffset("_Stencil", new Vector2(0.5f - ((float) progress/(float) numberOfProcesses),0));

		};


		for(int i=0; i<comps.Length; i++){

			isBusy = true;
			comps[i].LoadUp_Init(callback);

			do {
				yield return null;
			} while(isBusy);
		}

		var loading = SceneManager.LoadSceneAsync( 1, LoadSceneMode.Single);

		do {

			loading_barMaterial.SetTextureOffset("_Stencil", new Vector2(0.5f - ((float) progress + loading.progress/(float) numberOfProcesses),0));

			yield return null;

		} while(!loading.isDone);

		Destroy(this);

		gameObject.name = "BLINDED_GameComponents";

		loading_barMaterial.SetTextureOffset("_Stencil", new Vector2(0.5f,0));


	}
					
}

public class BLINDED_GameComponent : MonoBehaviour {

	public delegate void CallBack();

	///<summary>Called in the Load up scene by LoadUpScene_Controller</summary>
	public virtual void LoadUp_Init(CallBack callback){

	}

}
