using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class scr_moveRednecks_B : MonoBehaviour 
{
	private string direction = "left";

	private bool isBeingRaptured = false;
	private bool isSelected = false;
	private bool isFollowingPath = false;

	private float speedControl = 0.06f;

	private List<Vector3> pathToFollow;

	private LineRenderer drawPath = null;

	private int flipImageCounter_max = 5;
	private int flipImageCounter = 0;

	private Sprite currentSprite = null;

	public Sprite stillSprite;

	public Sprite movementSprite_1;
	public Sprite movementSprite_2;
	public Sprite movementSprite_3;
	public Sprite movementSprite_4;
	public Sprite movementSprite_5;
	public Sprite movementSprite_6;
	
	public Sprite upSprite_1;
	public Sprite upSprite_2;
	public Sprite upSprite_3;
	public Sprite upSprite_4;
	public Sprite upSprite_5;
	public Sprite upSprite_6;
	public Sprite upSprite_7;
	
	public Sprite downSprite_1;
	public Sprite downSprite_2;
	public Sprite downSprite_3;
	public Sprite downSprite_4;
	public Sprite downSprite_5;
	public Sprite downSprite_6;
	public Sprite downSprite_7;

	// new variables

	public  float  path_minDragDistance = 0.1f;
	private float  zPosition_lines; // set at start
	private Bounds bounds_screenToWorld; // set at start
	private Camera camera_main; // set at start


	void Start()
	{
		direction = "left";

		currentSprite = stillSprite;

		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

		pathToFollow = new List<Vector3> ();
		LayerMask.NameToLayer("Ignore Raycast");

		drawPath = transform.GetComponent<LineRenderer>();


		// new
		{
			camera_main = Camera.main;
			zPosition_lines =  camera_main.transform.position.z + camera_main.nearClipPlane;

			Vector3 bottomLeft = camera_main.ViewportToWorldPoint(new Vector3(0,0,0));
			Vector3 topRight = camera_main.ViewportToWorldPoint(new Vector3(1,1,0));

			bounds_screenToWorld = new Bounds(camera_main.transform.position,
			                                  new Vector3(topRight.x - bottomLeft.x,
												          topRight.y - bottomLeft.y, 1.0f));
		}

	}

	void Update()
	{
		if (isBeingRaptured == false)
		{
			if (isSelected == this)
			{
				if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
				{

					/*  original
					if ((!pathToFollow.Contains(touchDownPosition)) && 
					    (touchDownPosition.x < 6.4f) && 
					    (touchDownPosition.x > -6.4f) && 
					    (touchDownPosition.y < 3.0f) && 
					    (touchDownPosition.y > -3.0f) &&
					    (scr_godController.gameController.willFollow == true))
					{
						pathToFollow.Add(touchDownPosition);

						if (drawPath == null)
						{
							drawPath = transform.GetComponent<LineRenderer>();
						}

						drawPath.SetVertexCount(pathToFollow.Count());
					}
					*/

					// new
					{
						Vector3 touchDownPosition = Input.mousePosition;
						touchDownPosition = Camera.main.ScreenToWorldPoint(touchDownPosition);

						if(bounds_screenToWorld.Contains(touchDownPosition) &&
						  (scr_godController.gameController.willFollow == true))
						{

							if(pathToFollow.Count == 0)
								pathToFollow.Add(touchDownPosition);
							else{

								Vector3 secondToLastPoint = new Vector3(transform.position.x, transform.position.y, zPosition_lines);
								if(pathToFollow.Count > 1)
									secondToLastPoint = pathToFollow[pathToFollow.Count-2];

								if(Vector2.Distance((Vector2) secondToLastPoint, (Vector2) touchDownPosition) > path_minDragDistance)
									pathToFollow.Insert(pathToFollow.Count-1, touchDownPosition); // to push the last one back

								pathToFollow[pathToFollow.Count-1] = touchDownPosition;
							}
						}else
							OnMouseUp();
					}

				}
			}

			if (isFollowingPath == true)
			{
				if (pathToFollow.Count > 0)
				{
					transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);				

					Vector3 movePoint = new Vector3(pathToFollow[0].x, pathToFollow[0].y, transform.position.z);

					float adjustment = 0.1f;
					float tempX = movePoint.x - transform.position.x;
					float tempY = movePoint.y - transform.position.y;

					if (tempX < 0)
					{
						tempX *= -1;
					}
					if (tempY < 0)
					{
						tempY *= -1;
					}

					if (tempX >= tempY)
					{
						if (movePoint.x >= transform.position.x + adjustment)
						{
							Debug.Log("Right");
							direction = "right";

							if (transform.localScale.x > 0)
							{
								Vector3 flipVector = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
								transform.localScale = flipVector;
							}
						}
						else if (movePoint.x <= transform.position.x - adjustment)
						{
							Debug.Log("Left");
							direction = "left";

							if (transform.localScale.x < 0)
							{
								Vector3 flipVector = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
								transform.localScale = flipVector;
							}
						}
					}
					else if (tempY >= tempX)
					{
						if (movePoint.y >= transform.position.y + adjustment)
						{
							Debug.Log("Up");
							if (movePoint.x >= transform.position.x + adjustment)
							{
								Debug.Log("Up Right");
								direction = "up right";
								
								if (transform.localScale.x > 0)
								{
									Vector3 flipVector = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
									transform.localScale = flipVector;
								}
							}
							else if (movePoint.x <= transform.position.x - adjustment)
							{
								Debug.Log("Up Left");
								direction = "up left";
								
								if (transform.localScale.x < 0)
								{
									Vector3 flipVector = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
									transform.localScale = flipVector;
								}
							}
						}
						else if (movePoint.y <= transform.position.y + adjustment)
						{
							Debug.Log("Down");
							if (movePoint.x >= transform.position.x + adjustment)
							{
								Debug.Log("Down Right");
								direction = "down right";
								
								if (transform.localScale.x > 0)
								{
									Vector3 flipVector = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
									transform.localScale = flipVector;
								}
							}
							else if (movePoint.x <= transform.position.x - adjustment)
							{
								Debug.Log("Down Left");
								direction = "down left";
								
								if (transform.localScale.x < 0)
								{
									Vector3 flipVector = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
									transform.localScale = flipVector;
								}
							}
						}
						else
						{
							Debug.Log("Directionally impaired 1");
						}
					}
					else
					{
						Debug.Log("Directionally impaired 2");
					}


					transform.position = Vector3.MoveTowards(transform.position, movePoint, speedControl);

					if (transform.position == movePoint)
					{
						pathToFollow.Remove(pathToFollow[0]);
						// new
						{
							//drawPath.SetVertexCount(pathToFollow.Count());
						}
					}

					if ((direction == "right") || (direction == "left"))
					{
						ImageDirections("forward");
					}
					else if ((direction == "up right") || (direction == "up left"))
					{
						ImageDirections("up");
					}
					else if ((direction == "down right") || (direction == "down left"))
					{
						ImageDirections("down");
					}
					else
					{
						Debug.Log("Directionally Impaired 3");
					}
				}
				else if (pathToFollow.Count <= 0)
				{
					isFollowingPath = false;

					if ((direction == "right") || (direction == "left"))
					{
						transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stillSprite;
					}
					else if ((direction == "up right") || (direction == "up left"))
					{
						transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = upSprite_1;
					}
					else if ((direction == "down right") || (direction == "down left"))
					{
						transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = downSprite_1;
					}
				}
			}
		
		}else{ // new
			drawPath.enabled = false;
		}

		// new
		{
			drawPath.SetVertexCount(pathToFollow.Count+1);
			drawPath.SetPosition(0, new Vector3(transform.position.x, transform.position.y, zPosition_lines));
			for (int i=0; i<pathToFollow.Count; i++){
				
				drawPath.SetPosition(i+1, new Vector3(pathToFollow[i].x, pathToFollow[i].y, zPosition_lines));
			}
		}
	}

	void ImageDirections(string checkDirection)
	{
		
		if ((currentSprite == stillSprite) || (currentSprite == upSprite_1) || (currentSprite == downSprite_1))
		{
			if (checkDirection == "up")
			{
				FlipImage(upSprite_2);
			}
			else if (checkDirection == "down")
			{
				FlipImage(downSprite_2);
			}
			else if (checkDirection == "forward")
			{
				FlipImage(movementSprite_1);
			}
		}
		else if ((currentSprite == upSprite_2) || (currentSprite == downSprite_2) || (currentSprite == movementSprite_1))
		{
			if (checkDirection == "up")
			{
				FlipImage(upSprite_3);
			}
			else if (checkDirection == "down")
			{
				FlipImage(downSprite_3);
			}
			else if (checkDirection == "forward")
			{
				FlipImage(movementSprite_2);
			}
		}
		else if ((currentSprite == upSprite_3) || (currentSprite == downSprite_3) || (currentSprite == movementSprite_2))
		{
			if (checkDirection == "up")
			{
				FlipImage(upSprite_4);
			}
			else if (checkDirection == "down")
			{
				FlipImage(downSprite_4);
			}
			else if (checkDirection == "forward")
			{
				FlipImage(movementSprite_3);
			}
		}
		else if ((currentSprite == upSprite_4) || (currentSprite == downSprite_4) || (currentSprite == movementSprite_3))
		{
			if (checkDirection == "up")
			{
				FlipImage(upSprite_5);
			}
			else if (checkDirection == "down")
			{
				FlipImage(downSprite_5);
			}
			else if (checkDirection == "forward")
			{
				FlipImage(movementSprite_4);
			}
		}
		else if ((currentSprite == upSprite_5) || (currentSprite == downSprite_5) || (currentSprite == movementSprite_4))
		{
			if (checkDirection == "up")
			{
				FlipImage(upSprite_6);
			}
			else if (checkDirection == "down")
			{
				FlipImage(downSprite_6);
			}
			else if (checkDirection == "forward")
			{
				FlipImage(movementSprite_5);
			}
		}
		else if ((currentSprite == upSprite_6) || (currentSprite == downSprite_6) || (currentSprite == movementSprite_5))
		{
			if (checkDirection == "up")
			{
				FlipImage(upSprite_7);
			}
			else if (checkDirection == "down")
			{
				FlipImage(downSprite_7);
			}
			else if (checkDirection == "forward")
			{
				FlipImage(movementSprite_6);
			}
		}
		else if ((currentSprite == upSprite_7) || (currentSprite == downSprite_7) || (currentSprite == movementSprite_6))
		{
			if (checkDirection == "up")
			{
				FlipImage(upSprite_2);
			}
			else if (checkDirection == "down")
			{
				FlipImage(downSprite_2);
			}
			else if (checkDirection == "forward")
			{
				FlipImage(movementSprite_1);
			}
		}
		else
		{
			Debug.Log("Directionally Impaired 4");
		}
	}

	void FlipImage(Sprite newSprite)
	{
		if (flipImageCounter == flipImageCounter_max)
		{
			flipImageCounter = 0;
			transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = newSprite;
			currentSprite = newSprite;
		}
		else
		{
			flipImageCounter++;
		}
	}

	void OnCollisionEnter2D(Collision2D collision) 
	{
		if (collision.gameObject.transform.tag == "blocker")
		{
			isFollowingPath = false;
			isSelected = false;
			transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

			pathToFollow.Clear();
			// new
			{
				//drawPath.SetVertexCount(pathToFollow.Count());
				drawPath.enabled = false;
			}
			
//			drawPath = null;
			Debug.Log("Check 1");

			transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stillSprite;
		}
	}

	void OnMouseDown()
	{
		pathToFollow.Clear();
		// new
		{
			//drawPath.SetVertexCount(pathToFollow.Count());
			drawPath.enabled = true;
		}

//		drawPath = null;
		Debug.Log("Check 2");

		if (pathToFollow.Count > 0)
		{
			isFollowingPath = false;
			isSelected = false;
			transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			
			pathToFollow.Clear();

			// new
			{
				//drawPath.SetVertexCount(pathToFollow.Count());
				drawPath.enabled = true;
			}
			
//			drawPath = null;
			Debug.Log("Check 3");
			
			transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stillSprite;
		}

		isFollowingPath = true;
		isSelected = true;
	}


	void OnMouseUp()
	{
		scr_godController.gameController.willFollow = true;
		isSelected = false;
	}

	
	public void BeginTheRapture()
	{
		isBeingRaptured = true;
		pathToFollow.Clear();

		isFollowingPath = false;
		isSelected = false;
	
		transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stillSprite;
	}
}


// ΩX
// TG