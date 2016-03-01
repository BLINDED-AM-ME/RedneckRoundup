using UnityEngine;
using System.Collections;

public class scr_buttonHandler : MonoBehaviour
{
	public void LoadGame()
	{
		Debug.Log("Are those.... banjos?");
//		Application.LoadLevel ("scn_trailerPark");
	}
	

	public void ReturnToMainMenu()
	{
		Debug.Log("Later");
//		Application.LoadLevel("scn_opening");
		scr_godController.gameController.MainMenuButtonHit();
	}

	public void PlayButtonHit()
	{
		scr_godController.gameController.StartGame();
	}

	public void FacebookButtonHit()
	{
		Debug.Log("Facebook button hit");
	}

	public void GameCenterButtonHit()
	{
		Debug.Log("GameCenter button hit");
	}

	public void HelpButtonHit()
	{
		Debug.Log("Help button hit");
	}
}


// ΩX
// TG