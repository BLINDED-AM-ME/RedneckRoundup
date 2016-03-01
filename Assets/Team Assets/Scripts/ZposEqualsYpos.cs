using UnityEngine;
using System.Collections;

public class ZposEqualsYpos : MonoBehaviour {

		public bool isUpdatingEveryFrame = true;
		private Vector3 NewPosition = Vector3.zero;

	// Update is called once per frame
	void LateUpdate () {
	
				NewPosition = transform.position;
				NewPosition.z = NewPosition.y;
				transform.position = NewPosition;

				if(!isUpdatingEveryFrame)
						enabled = false;

	}
}
