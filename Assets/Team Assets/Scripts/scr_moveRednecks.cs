using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class scr_moveRednecks : MonoBehaviour 
{
	private string direction = "left";

	private bool isBeingRaptured = false;
	private bool isSelected = false;
	private bool isFollowingPath = false;
	
	private float speedControl = 0.06f;
	private float spriteAlpha = 1.0f;
	
	private float test = 1.0f;

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
	
	public Sprite abductionSprite_1;
	public Sprite abductionSprite_2;


	void Start()
	{
		direction = "left";

		currentSprite = stillSprite;

		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

		pathToFollow = new List<Vector3> ();
		LayerMask.NameToLayer("Ignore Raycast");

		drawPath = transform.GetComponent<LineRenderer>();
	}

	void Update()
	{
		if (isBeingRaptured == false)
		{
			if (isSelected == true)
			{
				//// Dear Unity.... SCREW YOU AND YOUR ENTIRE COLLISION DETECTION SYSTEM!!!!!
				if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
				{
					Vector3 touchDownPosition = Input.mousePosition;
					touchDownPosition = Camera.main.ScreenToWorldPoint(touchDownPosition);

					if ((!pathToFollow.Contains(touchDownPosition)) && 
					    (touchDownPosition.x < 7.45f) && 
					    (touchDownPosition.x > -7.45f) && 
					    (touchDownPosition.y < 2.63f) && 
					    (touchDownPosition.y > -5.6f) &&
					    (scr_godController.gameController.willFollow == true))
					{
						pathToFollow.Add(touchDownPosition);

//						if (drawPath == null)
//						{
//							drawPath = transform.GetComponent<LineRenderer>();
//						}
//
//						drawPath.SetVertexCount(pathToFollow.Count());
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
							Debug.Log("Directionally impaired cow 1");
						}
					}
					else
					{
						Debug.Log("Directionally impaired cow 2");
					}


					transform.position = Vector3.MoveTowards(transform.position, movePoint, speedControl);

					if (transform.position == movePoint)
					{
						pathToFollow.Remove(pathToFollow[0]);
						drawPath.SetVertexCount(pathToFollow.Count());
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
						Debug.Log("Directionally impaired cow 3");
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
					else
					{
						Debug.Log("Directionally impaired cow 4");
					}
				}
			}

//			Debug.Log("Draw Path Count: " + pathToFollow.Count());
//			for (int count = 0; count < pathToFollow.Count(); count++)
//			{
//				Debug.Log("Draw Path: " + pathToFollow[count]);
//				drawPath.SetPosition(count, pathToFollow[count]);
//			}
		}
		else
		{
			//// Test Site ////

//			float fade = Mathf.SmoothDamp(1.0f, 0.0f, ref test, 1.0f);
//			transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, fade);
		
			if ((currentSprite != abductionSprite_1) && (currentSprite != abductionSprite_2))
			{
				currentSprite = abductionSprite_1;
			}
			else
			{
				if (currentSprite == abductionSprite_1)
				{
					FlipImage(abductionSprite_2);

				}
				else
				{
					FlipImage(abductionSprite_1);
				}

				GameObject ufo = GameObject.FindGameObjectWithTag("spr_ship");
				Vector3 movePoint = new Vector3(ufo.transform.position.x, ufo.transform.position.y, transform.position.z);
				transform.position = Vector3.MoveTowards(transform.position, movePoint, 0.02f);
			}

			//// End Test Site
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
		if ((collision.gameObject.transform.tag == "blocker") || (collision.gameObject.transform.tag == "spr_barn"))
		{
			pathToFollow.Clear();
			drawPath.SetVertexCount(pathToFollow.Count());
			
//			drawPath = null;
			Debug.Log("Check 1");
		}
	}

	void OnMouseDown()
	{
		pathToFollow.Clear();
		drawPath.SetVertexCount(pathToFollow.Count());

//		drawPath = null;
		Debug.Log("Check 2");

		if (pathToFollow.Count > 0)
		{
			ClearPath();
			drawPath.SetVertexCount(pathToFollow.Count());
			
//			drawPath = null;
			Debug.Log("Check 3");
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
		transform.GetComponent<BoxCollider2D>().enabled = false;
		transform.GetComponent<BoxCollider2D>().enabled = false;
		isBeingRaptured = true;
		ClearPath();
	}

	public void ClearPath()
	{

		pathToFollow.Clear();
		
		isFollowingPath = false;
		isSelected = false;
		
		transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stillSprite;
	}
}


// ΩX
// TG