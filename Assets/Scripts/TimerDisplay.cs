using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour {


		public Text         textMinutes;
		public Text         textSeconds;
		public Text         textMilliseconds;

		private static string[] timeValues;
		public void UpdateDisplay (float duration) {
	

				timeValues = MakeTimeString(duration).Split(colonSplit, 3);
				textMinutes.text = timeValues[0];
				textSeconds.text = timeValues[1];
				textMilliseconds.text = timeValues[2];

		}

		private static int	     minutes = 0;
		private static int 	     seconds = 0;
		private static string 	 finalString;
		private static char[]    decimalSplit = ".".ToCharArray();
		private static char[]    colonSplit   = ":".ToCharArray();
		private static string[]  stringSplitResult;

		public static string MakeTimeString(float duration){


				minutes = Mathf.FloorToInt(duration/60.0f);
				seconds = Mathf.FloorToInt(duration - (float) (minutes * 60));

				// 00:00:00
				finalString = minutes < 10 ? "0"+minutes +":" : minutes+":";
				finalString += seconds < 10 ? "0"+seconds +":" : seconds+":";

				stringSplitResult = duration.ToString().Split(decimalSplit, System.StringSplitOptions.None);

				if(stringSplitResult.Length > 1)
					finalString += stringSplitResult[1][0];
				else
						finalString += "0";

				if(stringSplitResult.Length > 1 && stringSplitResult[1].Length > 1)
						finalString += stringSplitResult[1][1];
				else
						finalString += "0";

				return finalString;

		}
}
