using UnityEngine;
using System.Collections;

public class ZposEqualsThis : MonoBehaviour {

		public float zPos;

		public bool isUpdatingEveryFrame = true;
		private Vector3 NewPosition = Vector3.zero;

		// Update is called once per frame
		void LateUpdate () {

				NewPosition = transform.position;
				NewPosition.z = zPos;
				transform.position = NewPosition;

				if(!isUpdatingEveryFrame)
						enabled = false;

		}
}
