using UnityEngine;
using System.Collections;

public class SpaceShip_controller : MonoBehaviour {

		public static SpaceShip_controller instance;

		public Transform      ship_body;
		public GameObject     beam_gameObject;
		public float          beam_radius = 0.5f; 
		public float          abduction_duration = 2.0f;
		public AudioClip      abduction_sound;
		public AudioClip      abduction_afterSound;
	

		[HideInInspector]
		public Vector3       startingPoint = Vector3.zero;

		Cow_controller targetCow;

		[System.Serializable]
		public class DifficultyLevel{

				public float time_Activation = 0.0f;
				public float targetChangeRate = 10.0f;
				public float movement_speed = 3.0f;

				public float smartness_timeFollowingActivation = 5.0f;
				public float smartness_leadDistance = 5.0f;

		}

		public DifficultyLevel[] difficultLevels = new DifficultyLevel[1];
		[HideInInspector]
		public DifficultyLevel currentDifficulty;


		float tempFloat1 = 0.0f;
		float tempFloat2 = 0.0f;
		Vector3 temp3V1   = Vector3.zero;
		Vector3 temp3V2   = Vector3.zero;

		void Awake(){
				instance = this;
		}

		// Use this for initialization
		void Start () {

				startingPoint = transform.position;

				currentDifficulty = difficultLevels[0];

				StartCoroutine(ChangeTarget(currentDifficulty.targetChangeRate));
		}

		// Update is called once per frame
		void Update () {
				
				// this means it is not abducting
				if(!beam_gameObject.activeInHierarchy){

						if(Cow_controller.instances.Count > 0){

								for(int i=difficultLevels.Length-1; i>=0; i--){
										if(difficultLevels[i].time_Activation <= SceneController_gameScene.instance.time_duration){
												currentDifficulty = difficultLevels[i];
												break;
										}
								}

								//MoveToClosestCow();
								SmartMove();

								BeamCheck();
						}else{

								temp3V1 = transform.position;
								temp3V1.z = 0.0f;

								temp3V2 = startingPoint;
								temp3V2.z = 0.0f;

								// then move towards the closest cow
								transform.position = Vector3.MoveTowards(temp3V1, temp3V2, currentDifficulty.movement_speed * Time.deltaTime);

						}
				}

		}

		IEnumerator ChangeTarget(float delay){

				yield return new WaitForSeconds(delay);

				targetCow = null;

				StartCoroutine(ChangeTarget(currentDifficulty.targetChangeRate));

		}

		private Vector3 SmartMove_IntersectionPoint = Vector3.zero;
		private bool    SmartMove_isIntersecting = false;
		private float   SmartMove_IntersectionCountdown = 0.0f;
		void SmartMove(){

				if(targetCow == null)
						targetCow = Cow_controller.instances[Random.Range(0, Cow_controller.instances.Count)];


				if(targetCow.comp_path == null)
						return;


				// intersection call
				if(!SmartMove_isIntersecting){
						
						if(SmartMove_IntersectionCountdown <= 0 && 
								Smart_Intersection(targetCow.comp_path.CalculatePathDistance(), targetCow.comp_path)){
								SmartMove_isIntersecting = true;
								SmartMove_IntersectionCountdown = currentDifficulty.smartness_timeFollowingActivation;
						}else{
								SmartMove_IntersectionCountdown -= Time.deltaTime;
						}
				}


				temp3V1 = transform.position;
				temp3V1.z = 0.0f;

				temp3V2 = targetCow.transform.position;
				temp3V2.z = 0.0f;

				if(SmartMove_isIntersecting &&
						Vector2.Dot((SmartMove_IntersectionPoint - temp3V1).normalized, (temp3V2 - temp3V1).normalized) > -0.95f){
						temp3V2 = SmartMove_IntersectionPoint;
				}else{

						SmartMove_isIntersecting = false;

						temp3V2 = targetCow.transform.position;
						temp3V2.z = 0.0f;
				}
						
				transform.position = Vector3.MoveTowards(temp3V1, temp3V2, currentDifficulty.movement_speed * Time.deltaTime);

				// check if he reached the path point
				if(transform.position == SmartMove_IntersectionPoint){
						SmartMove_isIntersecting = false;
				}else{
						
				}
						

		}

		bool Smart_Intersection(float cowPathDistance, PathHandler comp_path){

				if(cowPathDistance > currentDifficulty.smartness_leadDistance)
				{
					
						temp3V2 = comp_path.GetRoutePosition(currentDifficulty.smartness_leadDistance);
						temp3V2.z = 0.0f;

						// distance/speed = ETA	

						// if you can intersect
						if(currentDifficulty.smartness_leadDistance / targetCow.movement_speed <
						Vector2.Distance((Vector2) temp3V2, (Vector2) transform.position)/currentDifficulty.movement_speed)
						{
								// target intersection point
								SmartMove_IntersectionPoint = temp3V2;
								return true;

						}else{
								return false;
						}
				}else{
						return false;
				}

		}


		void MoveToClosestCow(){

				temp3V1 = transform.position;
				temp3V1.z = 0.0f;

				// first find the closest cow

				targetCow = Cow_controller.instances[0];
				tempFloat1 = Vector2.Distance((Vector2) temp3V1, (Vector2) targetCow.transform.position);

				for(int i=1; i<Cow_controller.instances.Count; i++){

						tempFloat2 = Vector2.Distance((Vector2) transform.position, (Vector2) Cow_controller.instances[i].transform.position);

						if(tempFloat1 > tempFloat2){
								tempFloat1 = tempFloat2;
								targetCow = Cow_controller.instances[i];
						}
				}

				temp3V2 = targetCow.transform.position;
				temp3V2.z = 0.0f;


				// then move towards the closest cow
				transform.position = Vector3.MoveTowards(temp3V1, temp3V2, currentDifficulty.movement_speed * Time.deltaTime);

		}

		void BeamCheck(){


				// check if the cow is in the beam
				temp3V1 = transform.position;
				temp3V1.z = 0.0f;

				temp3V2 = targetCow.transform.position;
				temp3V2.z = 0.0f;

				if(Vector3.Distance(temp3V1, temp3V2) <= beam_radius)
						StartCoroutine(Abduct(targetCow));

		}

		float abduction_progress = 0.0f;

		IEnumerator Abduct(Cow_controller cow){

				beam_gameObject.SetActive(true);

				GetComponent<AudioSource>().PlayOneShot(abduction_sound);

				if(Cow_controller.instances.Count == 1)
						SceneController_gameScene.instance.GameOver();

				cow.Abduction();

				temp3V1 = transform.position;
				temp3V1.z = temp3V1.y;
				temp3V2 = ship_body.position;
				temp3V2.z = temp3V1.y;
	

				cow.transform.position = new Vector3(cow.transform.position.x, cow.transform.position.y, temp3V1.z);
			
				abduction_progress = 0.0f;
				while(abduction_progress < 1.0f){

						cow.transform.position = Vector3.MoveTowards(cow.transform.position,
								Vector3.Lerp(temp3V1, temp3V2, abduction_progress),
								Time.deltaTime * 3.0f);

						abduction_progress += Time.deltaTime/abduction_duration;

						yield return null;
				}

				Destroy(cow.gameObject);

				GetComponent<AudioSource>().PlayOneShot(abduction_afterSound);

				beam_gameObject.SetActive(false);
		}

		void OnDrawGizmos() {
				
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(transform.position, beam_radius);
		}


}
