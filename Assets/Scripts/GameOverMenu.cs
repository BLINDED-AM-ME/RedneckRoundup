using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverMenu : MonoBehaviour {

		public GameObject rootPanel_noAd;
		public Text[] text_currentTimes;
		public Text[] text_bestTimes;

		public string CurrentTime{

				set{
						foreach(Text displayText in text_currentTimes)
								displayText.text = value;
				}
				get{
						return text_currentTimes[0].text;
				}

		}

		public string BestTime{

				set{
						foreach(Text displayText in text_bestTimes)
								displayText.text = value;
				}
				get{
						return text_bestTimes[0].text;
				}

		}


		public void Show(string currentTime = "", string bestTime = ""){

				CurrentTime = currentTime;
				BestTime    = bestTime;

				rootPanel_noAd.SetActive(true);
		
		}

}
