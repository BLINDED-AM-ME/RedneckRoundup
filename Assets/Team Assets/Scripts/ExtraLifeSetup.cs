using UnityEngine;
using System.Collections;

public class ExtraLifeSetup : MonoBehaviour {

		public Transform[] startingPath;


	// Use this for initialization
		IEnumerator Start () {
	
				yield return new WaitForEndOfFrame();

				GetComponent<LineRenderer>().enabled = false;
				PathHandler path_comp = GetComponent<PathHandler>();

				Vector3[] newPath = new Vector3[startingPath.Length];

				for(int i=0; i<startingPath.Length; i++){
						newPath[i] = startingPath[i].position;
				}

				path_comp.SetPath(newPath);

				Destroy(this);

		}
	
}
