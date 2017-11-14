using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

		public Image three;
		public Image two;
		public Image one;
		public Image go;

		public delegate void Callback();

		Callback callback;

		public void StartCountdown(Callback returnCode){
				callback = returnCode;
				StartCoroutine(TheRealCountdown());
		}

		IEnumerator TheRealCountdown(){

				three.enabled = true;
				two.enabled = false;
				one.enabled = false;
				go.enabled = false;

				yield return new WaitForSeconds(1.0f);
				three.enabled = false;
				two.enabled = true;

				yield return new WaitForSeconds(1.0f);
				two.enabled = false;
				one.enabled = true;

				yield return new WaitForSeconds(1.0f);
				one.enabled = false;
				go.enabled = true;

				yield return new WaitForSeconds(0.5f);
				go.enabled = false;

				callback();
		}
}
