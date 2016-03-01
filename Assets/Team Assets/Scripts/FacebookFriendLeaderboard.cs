using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FacebookFriendLeaderboard : MonoBehaviour {

		public FacebookFriendCell firstCell;

		private RectTransform listRect;

		private int listSize = 1;
		private float cellHeight = 100.0f;

				

		public void AdjustList(){


				if(listRect == null)
						listRect = firstCell.transform.parent.GetComponent<RectTransform>();

				cellHeight = firstCell.GetComponent<RectTransform>().sizeDelta.y;



				listSize = BLINDED_Facebook_Controller.leaderBoard.Count;

				listRect.sizeDelta = new Vector2(listRect.sizeDelta.x, cellHeight * listSize);

				for(int i=listRect.childCount; i < listSize; i++){
						RectTransform newCell = Instantiate(firstCell).GetComponent<RectTransform>();
						newCell.SetParent(listRect, false);
				}

				for(int i=listRect.childCount; i > listSize; i--){
						Destroy(listRect.GetChild(i).gameObject);
				}
					
				FacebookFriendCell          cellRef = null;
				Dictionary<string, object>  dicRef = null;
				for(int i=0; i < listSize; i++){
						cellRef = listRect.GetChild(i).GetComponent<FacebookFriendCell>();
						dicRef  =  BLINDED_Facebook_Controller.leaderBoard[i];
					
						cellRef.nameText.text = dicRef[BLINDED_Facebook_Controller.key_user_name].ToString();
						cellRef.scoreText.text = dicRef[BLINDED_Facebook_Controller.key_user_score].ToString();
						cellRef.numberText.text = i+1 + ".";
				//		BLINDED_Facebook_Controller.FetchUserIcon(cellRef.profilePic,
				//				dicRef[BLINDED_Facebook_Controller.key_user_id].ToString());
				}

		}

}
