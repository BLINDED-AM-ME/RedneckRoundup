using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class scr_godController : MonoBehaviour 
{
	private static scr_godController instance = null;
	public static scr_godController gameController
	{
		get 
		{
			if (instance == null) 
			{
				instance = GameObject.Find("obj_God").GetComponent<scr_godController>();
			}
			return instance;
		}
	}
	
	public Text scoreTimerText = null;

	public bool willFollow = true;
	public bool gameOver = false;
	public bool gameStarted = false;
	private bool hardResetNeeded = false;

	private int redneckCount_max = 4;
	private int redneckCount = 0;

	private float scoreTimer = 0.0f;

//	public RawImage gameOverScreen;
//	public Button mainMenu_button;
//	public Text mainMenu_text;

	public GameObject cow;

	public GameObject rock;
	public GameObject pond;
	public GameObject hayBaleLeft;
	public GameObject hayBaleRight;
	public GameObject fenceLeft;
	public GameObject fenceRight;
	public GameObject counterToDecrement;

	public Texture[] countDownImageArray;

	private List<GameObject> obstacleList;
	private List<Vector3> obstaclePositionsList;

	void Awake()
	{
//		GameObject canvas = GameObject.Find("Canvas");
		//		Text[] textValue = canvas.GetComponentsInChildren<Text>();
		//		scoreTimerText = textValue[0];
		
		obstacleList = new List<GameObject>();
		obstaclePositionsList = new List<Vector3>();

		//		SetupObstacle(pond);
		//		SetupObstacle(rock);
		//		SetupObstacle(fenceLeft);
		//		SetupObstacle(fenceRight);
		//		SetupObstacle(hayBaleLeft);
		//		SetupObstacle(hayBaleRight);
	}

	void Start()
	{

	}

	void Update()
	{
		if (gameStarted == true)
		{
			if ((scoreTimerText != null) && (gameOver == false))
			{
				scoreTimer += Time.deltaTime;
				scoreTimerText.text = scoreTimer.ToString("F2");
			}
		}
	}

	public void StartGame()
	{
		ToggleMainMenu(false);
		ToggleGamePlayUI(true);

		StartCoroutine(CountDownTimer(1.0f));
	}

	private void GameSetup()
	{
		scoreTimer = Time.deltaTime;
		
		obstacleList.Add(pond);
		obstacleList.Add(rock);
		obstacleList.Add(hayBaleLeft);
		obstacleList.Add(hayBaleRight);
		obstacleList.Add(fenceLeft);
		obstacleList.Add(fenceRight);
		
		obstaclePositionsList.Add(new Vector3(-6.11f, 1.43f, 0.0f));
		obstaclePositionsList.Add(new Vector3(-6.34f, -4.35f, 0.0f));
		obstaclePositionsList.Add(new Vector3(-0.62f, -3.76f, 0.0f));
		obstaclePositionsList.Add(new Vector3(5.88f, -4.35f, 0.0f));
		obstaclePositionsList.Add(new Vector3(1.82f, 1.55f, 0.0f));
		obstaclePositionsList.Add(new Vector3(-2.08f, -0.79f, 0.0f));
		obstaclePositionsList.Add(new Vector3(-3.03f, -3.0f, 0.0f));
		
		for (int count = 3; count > 0; count--)
		{
			int randObstacle = Random.Range(0, obstacleList.Count());
			SetupObstacle(obstacleList[randObstacle]);
			
			obstacleList.Remove(obstacleList[randObstacle]);
		}
		
		
		for (int count = redneckCount; count > 0; count--)
		{
			SetupObstacle(cow);
		}
		
		scr_moveAlienShip.gameController.startShip = true;
	}

	private void SetupObstacle(GameObject obstacle)
	{
		int randPos = Random.Range(0, obstaclePositionsList.Count());
		Instantiate(obstacle, obstaclePositionsList[randPos], obstacle.transform.rotation);
		obstaclePositionsList.Remove(obstaclePositionsList[randPos]);
	}

	public void AdjustRedneckCounts(int count)
	{
		redneckCount += count;

		if (redneckCount <= 0)
		{
			ItsGameOverMan();
		}
	}

	public void ItsGameOverMan()
	{
		gameOver = true;
		gameStarted = false;

//		gameOverScreen.enabled = true;
//		mainMenu_button.image.enabled = true;
//		mainMenu_text.enabled = true;

		ToggleGamePlayUI(false);
		ToggleGameOverMenu(true);
	}

	public void MainMenuButtonHit()
	{
		ToggleGameOverMenu(false);
		ToggleMainMenu(true);
	}

	public void RestartGame()
	{
		GameObject[] blockerArray = GameObject.FindGameObjectsWithTag("blocker");

		foreach (GameObject blocker in blockerArray)
		{
			Destroy(blocker);
		}

//		obstacleList.Clear();

		Transform ufo = scr_moveAlienShip.gameController.transform;
		ufo.position = new Vector3(0.0f, 4.0f, ufo.position.z);
		
		scoreTimer = 0.0f;
		scoreTimerText.text = scoreTimer.ToString("F2");

		gameOver = false;
	}

	public IEnumerator CountDownTimer(float time)
	{
		int count = 0;
		bool decrement = true;
		counterToDecrement.GetComponent<RawImage>().enabled = true;

		while (decrement == true)
		{
			counterToDecrement.GetComponent<RawImage>().texture = countDownImageArray[count];
			count += 1;
			
			if (count == countDownImageArray.Length-1)
			{
				decrement = false;
			}
			
			yield return new WaitForSeconds(time);
		}

		counterToDecrement.GetComponent<RawImage>().enabled = false;

		Debug.Log("Starting");
		if (hardResetNeeded == true)
		{
			RestartGame();
		}
		
		redneckCount = redneckCount_max;
		gameStarted = true;
		
		GameSetup();
		
		hardResetNeeded = true;
	}

	///////////////////
	/// UI Toggles ////
	///////////////////

	private void ToggleGamePlayUI(bool toggle)
	{
		GameObject scoreBoard = GameObject.FindGameObjectWithTag("img_scoreBoard");
		ToggleScreen(toggle, scoreBoard, "img_scoreBoard");

		GameObject score = GameObject.FindGameObjectWithTag("txt_score");
		ToggleText(toggle, score, "txt_score");

		GameObject pauseButton = GameObject.FindGameObjectWithTag("btn_pause");
		ToggleButton(toggle, pauseButton, "btn_pause");
	}

	private void ToggleGameOverMenu(bool toggle)
	{
		GameObject gameOverMenu = GameObject.FindGameObjectWithTag("img_gameOverMenu");
		ToggleScreen(toggle, gameOverMenu, "img_gameOverMenu");

		GameObject goHomeButton = GameObject.FindGameObjectWithTag("btn_goHome");
		ToggleButton(toggle, goHomeButton, "btn_goHome");

		GameObject score = GameObject.FindGameObjectWithTag("txt_score-gameOver");
		ToggleText(toggle, score, "txt_score-gameOver");
		score.GetComponentInChildren<Text>().text = scoreTimer.ToString("F2");

		GameObject bestScore = GameObject.FindGameObjectWithTag("txt_bestScore-gameOver");
		ToggleText(toggle, bestScore, "txt_bestScore-gameOver");

		GameObject blackness = GameObject.FindGameObjectWithTag("img_blackness");
		ToggleScreen(toggle, blackness, "img_blackness");
	}

	private void ToggleMainMenu(bool toggle)
	{
		GameObject mainMenu = GameObject.FindGameObjectWithTag("img_mainMenu");
		ToggleScreen(toggle, mainMenu, "img_mainMenu");
		
		GameObject playButton = GameObject.FindGameObjectWithTag("btn_play");
		ToggleButton(toggle, playButton, "btn_play");
		
		GameObject helpButton = GameObject.FindGameObjectWithTag("btn_help");
		ToggleButton(toggle, helpButton, "btn_help");
		
		GameObject facebookButton = GameObject.FindGameObjectWithTag("btn_facebook");
		ToggleButton(toggle, facebookButton, "btn_facebook");
		
		GameObject gameCenterButton = GameObject.FindGameObjectWithTag("btn_gameCenter");
		ToggleButton(toggle, gameCenterButton, "btn_gameCenter");
		
		GameObject bestBoard = GameObject.FindGameObjectWithTag("img_bestScoreBoard");
		ToggleScreen(toggle, bestBoard, "img_bestScoreBoard");
		
		GameObject blackness = GameObject.FindGameObjectWithTag("img_blackness");
		ToggleScreen(toggle, blackness, "img_blackness");
	}

	private void ToggleButton(bool willUnhide, GameObject buttonObject, string buttonName)
	{
		if (buttonObject != null)
		{
			Button button = buttonObject.GetComponent<Button>();
			button.image.enabled = willUnhide;

			if (button.transform.childCount > 0)
			{
				button.GetComponentInChildren<Text>().enabled = willUnhide;
			}
		}
		else
		{
			Debug.Log("Can't find button: " + buttonName);
		}
	}
	
	private void ToggleScreen(bool willUnhide, GameObject screen, string screenName)
	{
		if (screen != null)
		{
			Debug.Log("Can't find screen: " + screenName);
			RawImage image = screen.GetComponent<RawImage>();
			image.enabled = willUnhide;
		}
		else
		{
			Debug.Log("Can't find screen: " + screenName);
		}
	}
	
	private void ToggleText(bool willUnhide, GameObject text, string textName)
	{
		if (text != null)
		{
			text.GetComponentInChildren<Text>().enabled = willUnhide;
		}
		else
		{
			Debug.Log("Can't find screen: " + textName);
		}
	}
}


// ΩX
// TG