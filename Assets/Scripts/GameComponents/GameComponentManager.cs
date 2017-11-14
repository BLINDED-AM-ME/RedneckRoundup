using UnityEngine;
using System.Collections;

public class GameComponentManager : MonoBehaviour {

	public class GameComponent : MonoBehaviour {

		public delegate void CallBack();
		public virtual void Initialize(CallBack callback){

		}

	}


	public int targetFps = 60;

	private int _progress = 0;
	private int _numberOfProcesses = 1;

	public float PercentComplete{
		get{
			if(_progress == 0)
				return 0.0f;
			else
				return (float) _progress/(float) _numberOfProcesses * 100.0f;
		}
	}


	// Use this for initialization
	IEnumerator Start () {

		DontDestroyOnLoad(gameObject);

		Application.targetFrameRate = targetFps;

		yield return new WaitForSeconds(2.0f); // give time to load app

		GameComponent[] comps = GetComponents<GameComponent>(); // num of processes

		_numberOfProcesses = comps.Length;



		for(int i=0; i<_numberOfProcesses; i++){

			comps[i].Initialize(delegate { 
				_progress++;
			});

			while(_progress <= i)
				yield return null; // wait for the current comps[i].Initialize(callback)
		}
			

		enabled = false;

	}
					
}