using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController_startUp : MonoBehaviour {

		public int targetFramerate = 30;

		public Material loading_barMaterial;
		public float    loading_progress = 0.0f;

		// Use this for initialization
		void Start () {
				
				Application.targetFrameRate = targetFramerate;

		}
		
		// Update is called once per frame
		void Update () {

				loading_barMaterial.SetTextureOffset("_Stencil", new Vector2(0.5f - loading_progress,0));
				loading_progress += Time.deltaTime;

				if(loading_progress >= 1.0f)
						SceneManager.LoadScene(1, LoadSceneMode.Single);
		
		}
}
