using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using Facebook.MiniJSON;
//using Facebook.Unity;

public class BLINDED_Facebook_Controller : BLINDED_GameComponent{
	public static BLINDED_Facebook_Controller instance;

	public static string key_user_id     = "id";
	public static string key_user_name   = "name";
	public static string key_user_score  = "score";

	public static string key_app_Name = "name";
	public static string key_app_FacebookUrl = "link";
	public static string key_app_GoogleUrl   = "google_play";
	public static string key_app_iOSUrl      = "itunes";
	public static string key_app_logoUrl     = "logo_url";

	
	public static Dictionary<string,object> userData;
	public static Dictionary<string,object> appData;

	public static List<Dictionary<string, object>>  leaderBoard = null;
	public static Dictionary<string, Sprite> images  = new Dictionary<string, Sprite>();


	public List<string> permissions_read = new List<string>(){

		"public_profile",
		"user_friends",
		"email"
	};

	public List<string> permissions_publish = new List<string>(){

			"publish_actions"
	};

/*

	///<summary>Called in the Load up scene by LoadUpScene_Controller</summary>
	public override void LoadUp_Init (CallBack callback)
	{
		base.LoadUp_Init (callback);
	
		instance = this;

#if UNITY_WEBGL
			callback();
			return;
#endif

		FB.Init(delegate {

			userData = new Dictionary<string, object>(){
				{ key_user_id, null},
				{ key_user_name, "Johny Appleseed"},
				{ key_user_score, "0"}
				
			};
			
			if(Application.isMobilePlatform)
				FB.ActivateApp();

			if(FB.IsInitialized &&
			   PlayerPrefs.GetInt("did_LogIntoFacebook",0) == 1 &&
			   !FB.IsLoggedIn){ // check for login
				
				
				LogMeIn(delegate(ILoginResult login_result) {
					
					callback();
				});

			}else
				callback();


		}, delegate(bool isUnityShown) {

		});

	}


	#region Developer calls
		public delegate void LoginCallback(ILoginResult result);
	///<summary> Log in before all else</summary>
	public static void LogMeIn(LoginCallback callback){

		PlayerPrefs.SetInt("did_LogIntoFacebook",1);
		FB.LogInWithReadPermissions(instance.permissions_read, delegate(ILoginResult result) {

						Debug.Log(result.RawResult);

						if(result.Error != null || result.Cancelled){
								callback(result);
						}else{

							FB.LogInWithPublishPermissions(instance.permissions_publish, delegate(ILoginResult result2) {
										Debug.Log(result2.RawResult);

									callback(result2);
							});
						}
		});
			
	}
		public delegate void GraphCallback(IGraphResult result);
	///<summary> Gets the user's personal info and puts it into userData </summary>
		public static void FetchUserInfo(GraphCallback callback)
	{
				
				FB.API("me?fields=name,id", HttpMethod.GET, delegate (IGraphResult result) {
						Debug.Log(result.RawResult);
						if(result.Error != null || result.Cancelled){
								callback(result);
						}else{
								
							var data = result.ResultDictionary;
							userData[key_user_id] = data[key_user_id];
							userData[key_user_name] = data[key_user_name];
							callback(result);
						}
		});
	}

	///<summary> Gets the app's info and puts it into appData </summary>
	public static void FetchAppInfo(GraphCallback callback)
	{

		FB.API("app?fields=link,logo_url,object_store_urls,name", HttpMethod.GET, delegate (IGraphResult result) {
						Debug.Log(result.RawResult);
			if(result.Error != null || result.Cancelled){
					callback(result);
			}else{
			
					IDictionary<string, object> data = result.ResultDictionary;
					appData = new Dictionary<string, object>();

					appData.Add(key_app_Name, data[key_app_Name]);
					appData.Add(key_app_FacebookUrl, data[key_app_FacebookUrl]);
					appData.Add(key_app_logoUrl, data[key_app_logoUrl]);

					if(data.ContainsKey("object_store_urls")){
						Dictionary<string, object> stores = (Dictionary<string, object>) data["object_store_urls"];

						if(stores.ContainsKey(key_app_GoogleUrl)){
							appData.Add(key_app_GoogleUrl, stores[key_app_GoogleUrl]);
						}else{
							appData.Add(key_app_GoogleUrl, data[key_app_FacebookUrl]);
						}


						if(stores.ContainsKey(key_app_iOSUrl)){
							appData.Add(key_app_iOSUrl, stores[key_app_iOSUrl]);
						}else{
							appData.Add(key_app_iOSUrl, data[key_app_FacebookUrl]);
						}
					}else{
							appData.Add(key_app_GoogleUrl, data[key_app_FacebookUrl]);
							appData.Add(key_app_iOSUrl, data[key_app_FacebookUrl]);
					}

					callback(result);
			}
		});

	}

	///<summary> Logs them out </summary>
	public static void LogMeOut(){
		
		if(FB.IsLoggedIn){
			FB.LogOut();
		}
		
	}

	private static bool isSharingGame = false;
	public delegate void ShareCallback(IShareResult result);
	///<summary> Popup to post a link to the game </summary>
	public static void ShareGame(ShareCallback callback){

		if(!FB.IsLoggedIn || isSharingGame){
			return;
		}

		isSharingGame = true;

			FetchAppInfo(delegate (IGraphResult graph_result){

						if(graph_result.Error != null || graph_result.Cancelled){
								callback(null);
						}else{


						FB.ShareLink(new System.Uri(appData[key_app_FacebookUrl].ToString()),
								"Title",
								"description", 
										new System.Uri(appData[key_app_logoUrl].ToString()), 
								delegate(IShareResult share_result) {
								
								isSharingGame = false;
						});
			
//			FB.Feed(picture:appData[key_app_logoUrl].ToString(),
//			        linkName: userData[key_user_first_name].ToString() + " is Playing " + appData[key_app_Name].ToString(),
//			        link: appData[key_app_FacebookUrl].ToString(),
//			        callback: callback);
				
						}
			
		});
		        
		//"http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest"),
		
	}

	///<summary> Popup to invite friends </summary>
	public static void InviteFriends(CallBack callback){

				FetchAppInfo(delegate(IGraphResult result) {
					
						FB.Mobile.AppInvite(
								new System.Uri(appData[key_app_FacebookUrl].ToString()),
								new System.Uri(appData[key_app_logoUrl].ToString()),
						delegate(IAppInviteResult result2) {
						
						});
				});
	}           

	///<summary> Gets the user's highscore and puts it into userData </summary>
	public static void FetchUserHighScore(GraphCallback callback){

			FB.API("me?fields=score", HttpMethod.GET, delegate(IGraphResult result) { 
						
					if(result.Error != null || result.Cancelled){
							callback(result);
					}else{
								IDictionary<string, object> resultDic = result.ResultDictionary;
								IDictionary<string, object> scores = null;

								if(resultDic.TryGetValue("scores", out scores)){

										IList<object> data = (IList<object>) scores["data"];
										IDictionary<string, object> score = (IDictionary<string, object>) data[0];

										userData[key_user_score] = score["score"];
								}else{
										userData[key_user_score] = 0;
								}

			
						callback(result);
					}
		});

	}

	///<summary> Post the value to the game's highscore if it is greater </summary>
	public static void PosthUserHighScore(int value, GraphCallback callback){

		FetchUserHighScore(delegate(IGraphResult result) {

			if(int.Parse(userData[key_user_score].ToString()) < value){
				userData[key_user_score] = value.ToString();

				var query = new Dictionary<string, string>();
				query["score"] = value.ToString();
				
				FB.API("me/scores", HttpMethod.POST, delegate(IGraphResult result2) {
								callback(result2);
				}, query);

			}else{

				callback(result);
			}
		});

	}

	///<summary> Gets limited number of leaderboard ids, names, and scores then puts them into leaderBoard </summary>
		public static void FetchLeaderboardScores(int limit, GraphCallback callback){

	
			FB.API("app/scores?fields=score,user.limit(20)", HttpMethod.GET, delegate(IGraphResult result) {
						Debug.Log(result.RawResult);
				if(result.Error != null || result.Cancelled){
					callback(result);
				}else{

					leaderBoard = new List<Dictionary<string, object>>();

					var responseObject = result.ResultDictionary;
					object scoresh;
					var scores = new List<object>();
					if (responseObject.TryGetValue ("data", out scoresh)) 
					{
						scores = (List<object>) scoresh;

						for(int i=0; i<scores.Count; i++){ // lay out the players
							Dictionary<string,object> score = scores[i] as Dictionary<string, object>;
							Dictionary<string,object> player = new Dictionary<string, object>();

							player.Add("score",score["score"].ToString());

							Dictionary<string,object> id_name = score["user"] as Dictionary<string,object>;
							player.Add("id", id_name["id"].ToString());
							player.Add("name", id_name["name"].ToString());


							leaderBoard.Add(player);
						}
					}

					callback(result);

				}
		});

	}

	///<summary> Gets a user's icon </summary>
	public static void FetchUserIcon(Image image, string userId ){

		if(images.ContainsKey(userId)){

			image.sprite = images[userId];
	
		}

		LoadPictureAPI(GetPictureURL(userId, 128, 128),pictureTexture =>
		{
			if (pictureTexture != null)
			{
				
				Texture2D pictureTexture2d = pictureTexture as Texture2D;

				Sprite newSprite = Sprite.Create(pictureTexture2d,
				                                 new Rect(0, 0, pictureTexture.width, pictureTexture.height),
				                                 new Vector2(0.5f,0.5f)
				                                 );

				if(!images.ContainsKey(userId))
					images.Add(userId, newSprite);

				if(image != null)
					image.sprite = newSprite;
			}

		});

	}
	
	#endregion


	#region JSon stuff

	static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
	{
		string url = string.Format("/{0}/picture", facebookID);
		string query = width != null ? "&width=" + width.ToString() : "";
		query += height != null ? "&height=" + height.ToString() : "";
		query += type != null ? "&type=" + type : "";
		query += "&redirect=false";
		if (query != "") url += ("?g" + query);

				Debug.Log(url);

		return url;
	}

	static Dictionary<string, string> DeserializeJSONProfile(string response)
	{
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object nameH;
		var profile = new Dictionary<string, string>();
		if (responseObject.TryGetValue("first_name", out nameH))
		{
			profile["first_name"] = (string)nameH;
		}
		return profile;
	}

	static List<object> DeserializeScores(string response) 
	{
		
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object scoresh;
		var scores = new List<object>();
		if (responseObject.TryGetValue ("data", out scoresh)) 
		{
			scores = (List<object>) scoresh;
		}
		
		return scores;
	}
	
	static List<object> DeserializeJSONFriends(string response)
	{
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object friendsH;
		var friends = new List<object>();
		if (responseObject.TryGetValue("invitable_friends", out friendsH))
		{
			friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
		}
		if (responseObject.TryGetValue("friends", out friendsH))
		{
			friends.AddRange((List<object>)(((Dictionary<string, object>)friendsH)["data"]));
		}
		return friends;
	}

	static private int GetScoreFromEntry(object obj)
	{
		Dictionary<string,object> entry = (Dictionary<string,object>) obj;
		return int.Parse((entry["score"].ToString()));
	}

	delegate void LoadPictureCallback (Texture texture);

	static IEnumerator LoadPictureEnumerator(string url, LoadPictureCallback callback)    
	{
		WWW www = new WWW(url);
		yield return www;
		callback(www.texture);
	}

	static void LoadPictureAPI (string url, LoadPictureCallback callback)
	{
		FB.API(url, HttpMethod.GET,result =>
       {
			if (result.Error != null)
			{
				return;
			}
			
			var imageUrl = DeserializePictureURLString(result.RawResult);
			
			instance.StartCoroutine(LoadPictureEnumerator(imageUrl,callback));
		});
	}
	static void LoadPictureURL (string url, LoadPictureCallback callback)
	{
		instance.StartCoroutine(LoadPictureEnumerator(url,callback));
		
	}

	static string DeserializePictureURLString(string response)
	{
		return DeserializePictureURLObject(Json.Deserialize(response));
	}
	
	static string DeserializePictureURLObject(object pictureObj)
	{

		var picture = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj)["data"]);
		object urlH = null;
		if (picture.TryGetValue("url", out urlH))
		{
			return (string)urlH;
		}
		
		return null;
	}



	#endregion
*/
}
