using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class SceneController_gameScene : MonoBehaviour {

		public static  SceneController_gameScene instance;

		public int          obstacles_num = 2;
		public GameObject[] obstacles_array;
		public int          cow_num = 3;
		public GameObject   cow_obj;
		public GameObject   farmer_obj;
		public float        time_duration = 0.0f;

		public GameObject   extra_Life;
		public UnityEngine.UI.Text         bonus_Text;
		public int                         bonus_secPerCow = 5;
		public int                         bonus_everySecs = 30;
		private float bonus_TimeTarget = 30.0f;

		ObjectSpawner comp_spawner;
		GameOverMenu  comp_gameOver;
		TimerDisplay  comp_timeDisplay;

		void Awake(){
				instance = this;
		}

		// Use this for initialization
		void Start () {
		
				comp_spawner = GetComponent<ObjectSpawner>();
				comp_gameOver = GetComponent<GameOverMenu>();
				comp_timeDisplay = GetComponent<TimerDisplay>();

				for(int i=0; i<obstacles_num; i++)
						comp_spawner.SpawnObjects(1, obstacles_array[Random.Range(0, obstacles_array.Length)]);


				comp_spawner.SpawnObjects(cow_num, cow_obj);
				comp_spawner.SpawnObjects(1, farmer_obj);

				bonus_TimeTarget = (float) bonus_everySecs;
				bonus_Text.gameObject.SetActive(false);


				enabled = false;


				if(GetComponent<Countdown>()){
						GetComponent<Countdown>().StartCountdown(delegate {
								StartTheGamePlay();
						});
				}else
						StartTheGamePlay();

		}


		// Update is called once per frame
		void Update () {

				time_duration += Time.deltaTime;

				if(extra_Life != null && !extra_Life.activeInHierarchy)
				if(time_duration >= bonus_TimeTarget){

						time_duration += Cow_controller.instances.Count * bonus_secPerCow;

						bonus_Text.gameObject.SetActive(true);
						bonus_Text.text = "Bonus " + Cow_controller.instances.Count + " cows x " + bonus_secPerCow + " sec";
						bonus_Text.GetComponent<Animator>().SetTrigger("Show");

						bonus_TimeTarget = time_duration + bonus_everySecs;
				}


				comp_timeDisplay.UpdateDisplay(time_duration);
		}

				

		void StartTheGamePlay(){

				enabled = true;

				Cow_controller.StartGamePlay();

		}

		public void GameOver(){

				enabled = false;
				StartCoroutine(GameOverPart2());
		}

		IEnumerator GameOverPart2(){

				yield return new WaitForSeconds(2.0f); // wait for abduction


				// extra life
				if(extra_Life != null){

					StartCoroutine(ExtraLife());

				}else{

					float highscore = float.Parse(SaveDataGameComponent.GetValue("playerBestTime", "0")) / 100.0f;

					if(time_duration > highscore){
							comp_gameOver.Show(TimerDisplay.MakeTimeString(time_duration), TimerDisplay.MakeTimeString(time_duration));

							SaveDataGameComponent.SetValue("playerBestTime", Mathf.FloorToInt(time_duration * 100.0f).ToString());
					}else
							comp_gameOver.Show(TimerDisplay.MakeTimeString(time_duration), TimerDisplay.MakeTimeString(highscore));

				}

		}

		IEnumerator ExtraLife(){

				while((Vector2) SpaceShip_controller.instance.transform.position != (Vector2) SpaceShip_controller.instance.startingPoint){
					yield return null;
				}

				extra_Life.SetActive(true);
				extra_Life = null;

				yield return new WaitForSeconds(0.5f);

				StartTheGamePlay();

		}

		public void LoadScene_Index(int index){

				SceneManager.LoadScene(index, LoadSceneMode.Single);


		}

		
}
