using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using Facebook.Unity;
using UnityEngine.UI;

public class SceneController_mainMenu : MonoBehaviour {

		public Text       Text_bestscore;
		public GameObject UI_main;
		public GameObject UI_facebook;
		public Button     button_facebook;
		public Button     button_facebookInvite;


		public Text facebook_score;
		public Image facebook_pic;
	
		private FacebookFriendLeaderboard comp_leaderboard;

		// Use this for initialization
		void Start () {
	
				comp_leaderboard = GetComponent<FacebookFriendLeaderboard>();

				Text_bestscore.text = TimerDisplay.MakeTimeString(float.Parse(BLINDED_SaveData.GetValue("playerBestTime", "0")) / 100.0f );

		}
	
		// Update is called once per frame
		void Update () {
		
		}
				
		public void LoadScene(int sceneIndex){
				SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
		}

		public void LoadScene(string sceneName){
				SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}

	

		#region Facebook
/*
		public void Button_FaceBook(){

				button_facebook.interactable = false;

				if(!FB.IsLoggedIn){// login
						BLINDED_Facebook_Controller.LogMeIn( delegate(ILoginResult result) {
						
								if(FB.IsLoggedIn){
										// success
										Button_FaceBook(); // recursion
								}else{
										button_facebook.interactable = true;
								}
						});
						return;
				}

				BLINDED_Facebook_Controller.FetchUserInfo(delegate(IGraphResult result) {
					
						BLINDED_Facebook_Controller.FetchUserIcon(facebook_pic,
								BLINDED_Facebook_Controller.userData[BLINDED_Facebook_Controller.key_user_id].ToString());

						BLINDED_Facebook_Controller.FetchUserHighScore(delegate(IGraphResult result2) {

								facebook_score.text = BLINDED_Facebook_Controller.userData[BLINDED_Facebook_Controller.key_user_score].ToString();

								BLINDED_Facebook_Controller.FetchLeaderboardScores(10, delegate(IGraphResult result3) {
										UI_facebook.SetActive(true);
										comp_leaderboard.AdjustList();

										button_facebook.interactable = true;
								});
						});
				});

		}

		public void Button_FacebookInvite(){

				button_facebookInvite.interactable = false;

				BLINDED_Facebook_Controller.InviteFriends(delegate() {

						button_facebookInvite.interactable = true;
				});
		}
*/
		#endregion
}
