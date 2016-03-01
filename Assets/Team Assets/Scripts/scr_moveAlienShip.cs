using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class scr_moveAlienShip : MonoBehaviour 
{
	
	private static scr_moveAlienShip instance = null;
	public static scr_moveAlienShip gameController
	{
		get 
		{
			if (instance == null) 
			{
				instance = GameObject.Find("obj_abductionPoint").GetComponent<scr_moveAlienShip>();
			}
			return instance;
		}
	}

	private float speedControl = 0.02f;

	private float targetedRedneckDistance = 1000.0f;
	private float redneckDistance = 0.0f;

	private Vector3 targetPosition = Vector3.zero;

//	private bool isMovingToPoint = false;
	
	public bool startShip = false;
	private bool runChecks = true;
	private bool isRapturing = false;
	private bool inPosition = false;
	private bool didSetupShip = false;

	private GameObject targetedRedneck = null;
	private List<GameObject> redneckList;

	void Start()
	{
		ToggleBeam(false);
	}

	void Update()
	{
		if (startShip == true)
		{
			if (didSetupShip == false)
			{
				redneckList = new List<GameObject> ();
				
				GameObject[] redneckArray = GameObject.FindGameObjectsWithTag("spr_redneck");
				
				foreach (GameObject redneck in redneckArray)
				{
					redneckList.Add(redneck);
				}

				didSetupShip = true;
			}

			if ((runChecks == true) && (isRapturing == false))
			{
				if ((redneckList != null) && (redneckList.Count > 0))
				{
					targetedRedneck = null;
					foreach (GameObject redneck in redneckList) 
					{
						redneckDistance = Vector2.Distance(transform.transform.position, redneck.transform.position);

						if (targetedRedneck == null)
						{
							targetedRedneck = redneck;
							targetedRedneckDistance = redneckDistance;
						}
						else if (redneckDistance < targetedRedneckDistance)
						{
							targetedRedneck = redneck;
							targetedRedneckDistance = redneckDistance;
						}
						else
						{
	//						Debug.Log("Staying on target");
						}
					}
					
					transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
					targetPosition = new Vector3(targetedRedneck.transform.position.x, (targetedRedneck.transform.position.y - (targetedRedneck.transform.localScale.y * 0.5f)), transform.position.z);
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedControl);
	//				targetedRedneckDistance = 10000.0f;
				}
			}

			if (targetedRedneck != null)
			{
				if (isRapturing == false)
				{
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedControl);

					if ((transform.position == targetPosition) && (inPosition == false) && (isRapturing == false))
					{
						scr_moveRednecks script = targetedRedneck.transform.GetComponent<scr_moveRednecks>();
						script.ClearPath();
						
						isRapturing = true;

						StartCoroutine(BeginTheRapture(1.0f));
					}
				}
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D objectHit) 
	{
//		if (isRapturing == false)
//		{
//			if (objectHit.gameObject.tag == "spr_redneck")
//			{
//				theHolyOne = objectHit.gameObject;
//				scr_moveRednecks script = theHolyOne.transform.GetComponent<scr_moveRednecks>();
//				script.ClearPath();
//
//				isRapturing = true;
//			}
//		}
	}
	
	void OnTriggerExit2D(Collider2D objectHit) 
	{

	}

	IEnumerator BeginTheRapture(float time)
	{
		inPosition = true;
		ToggleBeam(true);
		
		scr_moveRednecks script = targetedRedneck.transform.GetComponent<scr_moveRednecks>();
		script.BeginTheRapture();

		yield return new WaitForSeconds(time);

		Debug.Log("Name: " + targetedRedneck.name);

		redneckList.Remove(targetedRedneck);
		transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		
		targetedRedneck = null;
		Destroy(targetedRedneck);
		
		scr_godController.gameController.AdjustRedneckCounts(-1);

		ToggleBeam(false);
		
		runChecks = true;
		isRapturing = false;

		targetedRedneck = null;
		inPosition = false;
	}

	void ToggleBeam(bool beamIsOn)
	{
		foreach (Transform child in transform)
		{
			if (child.name == "spr_ufoBeam")
			{
				child.GetComponent<SpriteRenderer>().enabled = beamIsOn;
			}
		}
	}
}


// ΩX
// TG