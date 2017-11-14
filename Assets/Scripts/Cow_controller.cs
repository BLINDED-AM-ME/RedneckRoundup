using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cow_controller : MonoBehaviour {
		
		public static List<Cow_controller> instances = new List<Cow_controller>();
		public static Cow_controller selected_cow = null;

		static int parameterID_ySpeed = Animator.StringToHash("Yspeed");
		static int parameterID_Velocity = Animator.StringToHash("Velocity");
		static int parameterID_abduction = Animator.StringToHash("Abducted");

		private Animator       comp_animator;
		private SpriteRenderer comp_renderer;
		private LineRenderer   comp_lineRenderer;

		[HideInInspector]
		public PathHandler    comp_path;

		public  LayerMask     path_checkMask = 0;

		public float          movement_speed = 2.0f;


		private Bounds        bounds_screenToWorld; // set at start
		private Camera        camera_main; // set at start
	     

	void Awake(){
				instances.Add(this);
	}

	void OnDestroy() {
				instances.Remove(this);
	}

	// Use this for initialization
	void Start () {

				comp_animator = GetComponent<Animator>();
				comp_renderer = GetComponent<SpriteRenderer>();
				comp_lineRenderer = GetComponent<LineRenderer>();
				comp_path         = GetComponent<PathHandler>();


				camera_main = Camera.main;

				Vector3 bottomLeft = camera_main.ViewportToWorldPoint(new Vector3(0,0,0));
				Vector3 topRight = camera_main.ViewportToWorldPoint(new Vector3(1,1,0));

				bounds_screenToWorld = new Bounds(camera_main.transform.position,
									   new Vector3(topRight.x - bottomLeft.x,
												   topRight.y - bottomLeft.y, 1.0f));
				

				enabled = false;
	}
	
		// Update is called once per frame
		void Update () {
	

				if (Input.GetMouseButtonDown(0)){
						MouseDownCall();
				}

				// only the selected will go through
				if (selected_cow == this){
						if(Input.GetMouseButton(0))
									MouseDragCall();
							
						if (Input.GetMouseButtonUp(0))
									MouseUpCall();

				
				}

				// others may still move if they have to
				if(comp_path.PathCount > 0)
					Movement();

		}


		void MouseDownCall(){

				// all cows will make this call
				comp_lineRenderer.enabled = false; // only the selected cow will show his line

				// check if a cow was already selected
				if(selected_cow == null){

						// get the touch position
						Vector3 touchDownPosition = Input.mousePosition;
						touchDownPosition = Camera.main.ScreenToWorldPoint(touchDownPosition);
						touchDownPosition.z = transform.position.z;

						Bounds checkBounds = comp_renderer.bounds;
						checkBounds.center += Vector3.down * 0.5f;

						// check if this cow was selected
						if(checkBounds.Contains(touchDownPosition)){

								// it was
								selected_cow = this;

								// create a new path
								comp_path.SetPath(new Vector3[]{
										new Vector3(transform.position.x, transform.position.y, 0.0f)
								});

								// light it up
								comp_lineRenderer.enabled = true;
						}

				}

		}


		static RaycastHit2D[] results_rayhits = new RaycastHit2D[1];
		void MouseDragCall(){

				Vector3 touchDownPosition = Input.mousePosition;
				touchDownPosition = Camera.main.ScreenToWorldPoint(touchDownPosition);

				Vector3 secondToLastPoint = new Vector3(transform.position.x, transform.position.y, 0.0f);
				if(comp_path.PathCount > 1)
						secondToLastPoint = comp_path.GetPoint(comp_path.PathCount-2);
				

				if(bounds_screenToWorld.Contains(touchDownPosition) && 
						Physics2D.LinecastNonAlloc((Vector2) secondToLastPoint, (Vector2) touchDownPosition, results_rayhits, path_checkMask) < 1)
				{

						if(Vector2.Distance((Vector2) secondToLastPoint, (Vector2) touchDownPosition) >= comp_path.maxSegmentDistance){
								comp_path.InsertPoint(comp_path.PathCount-1, touchDownPosition); // to push the last one back
						}

						comp_path.SetPoint(comp_path.PathCount-1, touchDownPosition);

				}else{
						
						if(comp_path.PathCount > 1){
								comp_path.RemovePoint(comp_path.PathCount-1);
								MouseUpCall();
						}else{
								comp_path.SetPoint(0, transform.position);
						}

				}
		}

		void MouseUpCall(){

				selected_cow = null;
		}


		Vector3 targetPoint  = Vector3.zero;
		Vector3 currentPoint = Vector3.zero;
		Vector3 direction    = Vector3.zero;
		void Movement(){

				currentPoint = transform.position;
				currentPoint.z = 0.0f;

				targetPoint = comp_path.GetPoint(0);
				targetPoint.z = 0.0f;

				if(currentPoint == targetPoint){
						if(comp_path.PathCount > 1){
								targetPoint = comp_path.GetPoint(1);
								targetPoint.z = 0.0f;
								comp_path.RemovePoint(0);
						}
				}


				direction = targetPoint - currentPoint;
				direction.Normalize();

				if(direction.y > 0.5f)
						direction.y = 1.0f;
				else if(direction.y < -0.5f)
						direction.y = -1.0f;
				else
						direction.y = 0.0f;

				if(direction.x > 0)
						comp_renderer.flipX = true;
				else if(direction.x < 0)
						comp_renderer.flipX = false;
						

				comp_animator.SetFloat(parameterID_ySpeed, direction.y);
				comp_animator.SetFloat(parameterID_Velocity, direction == Vector3.zero ? 0.0f : 1.0f);


				transform.position = Vector3.MoveTowards(currentPoint, targetPoint, movement_speed * Time.deltaTime);


		}

		public void Abduction(){

				enabled = false;
				comp_lineRenderer.enabled = false;
				comp_animator.SetTrigger(parameterID_abduction);

				if(selected_cow == this)
						MouseUpCall();

				GetComponent<ZposEqualsYpos>().enabled = false;
		}


		public static void StartGamePlay(){

				for(int i=0; i<instances.Count; i++){
						instances[i].enabled = true;

				}
		}

		public static void StopGamePlay(){
				
				for(int i=0; i<instances.Count; i++){
						instances[i].enabled = false;

				}
		}
}
