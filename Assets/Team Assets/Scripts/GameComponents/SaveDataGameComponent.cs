using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

public class SaveDataGameComponent : GameComponentManager.GameComponent {
	
	public static SaveDataGameComponent instance;

	public string secretPassword = "no_such_luck";

	private static Dictionary<string, object> allData;

	private static byte[] encryptedBytes;

	///<summary>Called in the Load up scene by LoadUpScene_Controller</summary>
	public override void Initialize (CallBack callback)
	{
		base.Initialize (callback);
		
		instance = this;

		allData = new Dictionary<string, object>();
		allData.Add("emptyObj", "0");
	
		GetData();

		callback();

	}

	public static void SaveData(){

		if(instance == null)
			return;

		byte[] theKeyBytes  = Encoding.UTF8.GetBytes(instance.secretPassword);
		byte[] theDataBytes = Encoding.UTF8.GetBytes(MiniJSON.Json.Serialize(allData));
		
				int tempInt = 0;

		for(int i=0; i<theDataBytes.Length; i++){
						
				tempInt = (int) theDataBytes[i];
				tempInt += (int) theKeyBytes[i % theKeyBytes.Length];

				tempInt = tempInt  % 256;

				theDataBytes[i] = (byte) tempInt;
		}
		
		string theEncodedString = Convert.ToBase64String(theDataBytes, Base64FormattingOptions.InsertLineBreaks);

			try 
			{
				//Pass the filepath and filename to the StreamWriter Constructor
						using(StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/Save_Data.txt")){
			
							sw.Write(theEncodedString);
							
							//Close the file
							sw.Close();

						}
			}
			catch(Exception e)
			{
				Debug.Log("Exception: " + e.Message);
			}
	
	}

	private static void GetData(){

		if(instance == null)
			return;

		byte[] theKeyBytes  = Encoding.UTF8.GetBytes(instance.secretPassword);

		string finalOutcome = "";
		string line = "";


				try{    //Pass the file path and file name to the StreamReader constructor
						using (StreamReader sr = new StreamReader(Application.persistentDataPath + "/Save_Data.txt")){

								//Read the first line of text
								line = sr.ReadLine();

								//Continue to read until you reach end of file
								while (line != null) 
								{
										finalOutcome += line;
										//Read the next line
										line = sr.ReadLine();
								}

								//close the file
								sr.Close();

								byte[] theDataBytes = Convert.FromBase64String(finalOutcome);

								int tempInt = 0;
								for(int i=0; i<theDataBytes.Length; i++){

										tempInt = (int) theDataBytes[i];
										tempInt -= (int) theKeyBytes[i % theKeyBytes.Length];

										if(tempInt < 0)// aka negative
												tempInt += 256;

										theDataBytes[i] = (byte) tempInt;
								}

								string theDecodedString = Encoding.UTF8.GetString(theDataBytes);
								
								allData = MiniJSON.Json.Deserialize(theDecodedString) as Dictionary<string,object>;

						}
				} 
				catch (Exception e)
				{
						Debug.Log("The file could not be read:");
						Debug.Log(e.Message);

						allData = new Dictionary<string, object>();
						allData.Add("emptyObj", "0");
				}
					

	}

	public static void SetValue(string key, string value){

				if(instance == null)
						return;

				if(allData.ContainsKey(key)){
					allData[key] = value;
				}else{
					allData.Add(key, value);
				}
	}
				
	public static string GetValue(string key, string defualtValue){

				if(instance == null)
						return defualtValue;

				if(allData.ContainsKey(key)){
						return allData[key].ToString();
				}else{
						return defualtValue;
				}
	}

	public void OnApplicationPause(bool pauseStatus) {
		if(pauseStatus)
			SaveData();
	}
	public void OnApplicationQuit(){
		SaveData();
	}

}
