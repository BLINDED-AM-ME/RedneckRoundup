using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID
//using GooglePlayGames;
using UnityEngine.SocialPlatforms;
//using GooglePlayGames.BasicApi;

#elif UNITY_IOS
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Runtime.InteropServices;

#elif UNITY_WEBGL

using UnityEngine.SocialPlatforms;

#endif

public class BLINDED_Achievement_Controller : BLINDED_GameComponent {

#if UNITY_IOS
//	[DllImport("__Internal")]
//	private static extern void _ReportAchievement( string achievementID, float progress );
#endif


	public static BLINDED_Achievement_Controller instance;
	public static Dictionary<string, IAchievement> achievements = new Dictionary<string, IAchievement>();

	public string Leaderboard_AndroidID = "";
	public string Leaderboard_iosID = "";

	[System.Serializable]
	public struct NameAndIds{
		public string Name;
		public string Android_ID;
		public string ios_ID;
	}

	public NameAndIds[] namesAndIDs = new NameAndIds[5];

	///<summary>Called in the Load up scene by LoadUpScene_Controller</summary>
	public override void LoadUp_Init (CallBack callback)
	{
		base.LoadUp_Init(callback);

		instance = this;

#if UNITY_WEBGL
		callback();
		return;
#elif UNITY_ANDROID
		//PlayGamesPlatform.Activate();
#endif

		SignIn(callback, callback);
	
	}

	///<summary> checks if the user is signed in </summary>
	public static bool IsSignedIn
	{
		get
		{

#if UNITY_WEBGL || UNITY_EDITOR 
		return false;
#elif UNITY_ANDROID
			return PlayGamesPlatform.Instance.IsAuthenticated();
#elif UNITY_IOS
			return Social.localUser.authenticated;
#endif
		}
	}

	///<summary> Calls to sign the player in </summary>
	public static void SignIn (CallBack goodCall, CallBack badCall) {
		achievements.Clear();

		
#if UNITY_ANDROID

		for(int i=0; i<instance.namesAndIDs.Length; i++)
			achievements.Add(instance.namesAndIDs[i].Android_ID, null);

		// authenticate user:
		Social.localUser.Authenticate((bool success) => {
			// handle success or failure
			if(success){

				LoadAchievements();
				goodCall();

			}else{

				badCall();
			}
			
		});

#elif UNITY_IOS

		for(int i=0; i<instance.namesAndIDs.Length; i++)
			achievements.Add(instance.namesAndIDs[i].ios_ID, null);

		// Authenticate and register a ProcessAuthentication callback
		// This call needs to be made before we can proceed to other calls in the Social API
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		Social.localUser.Authenticate(delegate(bool obj) {

			if(obj){

				LoadAchievements();

				goodCall();

			}else{

				badCall();
			}

		});
#endif

	}

	static void LoadAchievements(){

		if(instance.namesAndIDs.Length == 0)
				return;

#if UNITY_ANDROID

		// make the null stuff not null
		string[] keysStr = new string[achievements.Count];
		achievements.Keys.CopyTo(keysStr,0);

		for(int i=0; i<keysStr.Length; i++){

			IAchievement newAchievement = Social.CreateAchievement();
			newAchievement.id = keysStr[i];
			newAchievement.percentCompleted = (double) PlayerPrefs.GetFloat(Social.localUser.id + keysStr[i], 0.0f);
			achievements[keysStr[i]] = newAchievement;
				
		}


#elif UNITY_IOS

		// only returns completed or in progress achievements
		Social.LoadAchievements (achs => {
			if (achs.Length > 0){
				for(int i=0; i<achs.Length; i++){
					IAchievement achievement = achs[i];
					achievements[achs[i].id] = achievement;
				}
			}

			// make the null stuff not null
			string[] keysStr = new string[achievements.Count];
			achievements.Keys.CopyTo(keysStr,0);

			for(int i=0; i<keysStr.Length; i++){

				float localSaveValue = PlayerPrefs.GetFloat(Social.localUser.id + keysStr[i], 0.0f);

				if(achievements[keysStr[i]] == null){
					IAchievement newAchievement = Social.CreateAchievement();
					newAchievement.id = keysStr[i];
					newAchievement.percentCompleted = (double) localSaveValue;
					achievements[keysStr[i]] = newAchievement;

				}else if(achievements[keysStr[i]].percentCompleted < localSaveValue){

					if(localSaveValue >= 100.0)
						Achievement_Unlock(keysStr[i]);
					else
						achievements[keysStr[i]].percentCompleted = (double) localSaveValue;
				}
			}

		});

#endif

	}

	///<summary> signs out, Android Only </summary>
	public static void SignOut(){

#if UNITY_ANDROID
		//PlayGamesPlatform.Instance.SignOut();
#elif UNITY_IOS

#endif

	}

	///<summary> pulls up the leaderboard ui </summary>
	public static void ShowLeaderboard(){

		if(!IsSignedIn)
			return;

#if UNITY_ANDROID
		//PlayGamesPlatform.Instance.ShowLeaderboardUI(instance.Leaderboard_AndroidID);
#elif UNITY_IOS

		Social.ShowLeaderboardUI();
#endif

	}

	///<summary> pulls up the achievements ui </summary>
	public static void ShowAchievements(){

		if(!IsSignedIn)
			return;

#if UNITY_ANDROID

		Social.ShowAchievementsUI();
#elif UNITY_IOS

		Social.ShowAchievementsUI();
#endif

	}

	///<summary> posts to the leaderboard </summary>
	public static void PostToLeaderBoard(int value){

		if(!IsSignedIn)
			return;

#if UNITY_ANDROID
		Social.ReportScore(value, instance.Leaderboard_AndroidID, (bool success) => {
			// handle success or failure
		});
#elif UNITY_IOS

		Social.ReportScore(value, instance.Leaderboard_iosID, (bool success) => {

		});
#endif

	}

	///<summary> adds steps to an achievement's progress </summary>
	public static void Achievement_Increment(string name, int steps, int stepsGoal){

		if(!IsSignedIn)
			return;

		string id = "";
		foreach(NameAndIds element in instance.namesAndIDs){
			if(element.Name.Equals(name)){
				#if UNITY_ANDROID
				id = element.Android_ID;
				#elif UNITY_IOS
				id = element.ios_ID;	
				#endif
				break;
			}
		}

		if(achievements[id] == null)
			return;

		if(achievements[id].percentCompleted >= 100.0)
			return;


		IAchievement theAchievement = achievements[id];
		
		double tempValue = theAchievement.percentCompleted; 
		tempValue += ( ((double) steps)/((double) stepsGoal) ) * 100.0; // new progress

		if(tempValue >= 100.0){

			Achievement_Unlock(name);

		}else{

			int wholeNumbers = Mathf.FloorToInt((float)tempValue) - Mathf.FloorToInt((float)theAchievement.percentCompleted);

			theAchievement.percentCompleted = tempValue;
			PlayerPrefs.SetFloat(Social.localUser.id + id, (float) tempValue);
			
			if(wholeNumbers >= 1){ // full percent or more
				#if UNITY_ANDROID
				//PlayGamesPlatform.Instance.IncrementAchievement(id, wholeNumbers, (bool success) => {
					// handle success or failure
			//	});
				#elif UNITY_IOS
				//_ReportAchievement(id, (float) tempValue);
				#endif
			}
		}

	}
	
	///<summary> unlocks an achievement </summary>
	public static void Achievement_Unlock(string name){

		if(!IsSignedIn)
			return;
		
		string id = "";
		foreach(NameAndIds element in instance.namesAndIDs){
			if(element.Name.Equals(name)){
				#if UNITY_ANDROID
				id = element.Android_ID;
				#elif UNITY_IOS
				id = element.ios_ID;	
				#endif
				break;
			}
		}
		
		if(achievements[id] == null)
			return;
		
		if(achievements[id].percentCompleted >= 100.0)
			return;


#if UNITY_ANDROID

		Social.ReportProgress(id, 100.0f, (bool success) => {
			// handle success or failure
			if(success){

				achievements[id].percentCompleted = 100.0;
				PlayerPrefs.SetFloat(Social.localUser.id + id, 100.0f);

			}else{


			}
		});

#elif UNITY_IOS

		//_ReportAchievement(id, 100.0f);

		achievements[id].percentCompleted = 100.0;
		PlayerPrefs.SetFloat(Social.localUser.id + id, 100.0f);

#endif

	}

	///<summary> ios dev use only </summary>
	public static void ResetAchievements(){

#if UNITY_IOS

		string[] keysStr = new string[achievements.Count];
		achievements.Keys.CopyTo(keysStr,0);
		
		for(int i=0; i<keysStr.Length; i++){
			PlayerPrefs.SetFloat(Social.localUser.id + keysStr[i], 0.0f);
			achievements[keysStr[i]].percentCompleted = 0.0;
		}

		GameCenterPlatform.ResetAllAchievements( (resetResult) => {


		});
#endif

	}

}
