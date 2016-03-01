using UnityEngine;
using System.Collections;

public class scr_tempFixZ : MonoBehaviour 
{
	void Start()
	{
//		if (transform.position.y >= )
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * -1);
	}

	void Update()
	{

	}

	void OnMouseEnter()
	{
//		Debug.Log("Object: " + transform.tag);
		scr_godController.gameController.willFollow = false;
	}

	void OnMouseExit()
	{
//		Debug.Log("Left Object");
		scr_godController.gameController.willFollow = true;
	}
}


// ΩX
// TG