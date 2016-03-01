using UnityEngine;
using System.Collections;

public class CameraDeviceSetting : MonoBehaviour {

		public bool    ipad_override = false;
		public Vector3 ipad_position;
		public float   ipad_cameraSize = 5;

	// Use this for initialization
	void Start () {

#if UNITY_IOS

				if(ipad_override || SystemInfo.deviceModel.Contains("iPad")){
						
						transform.position = ipad_position;

						GetComponent<Camera>().orthographicSize = ipad_cameraSize;
				}



#else


#endif
	
	}
	
}
