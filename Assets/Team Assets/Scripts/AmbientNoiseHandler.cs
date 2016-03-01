using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AmbientNoiseHandler : MonoBehaviour {

		[System.Serializable]
		public class AmbientSound{

				public AudioClip clip;
				public float     frequency_min = 1.0f;
				public float     frequency_max = 5.0f;
				[Range(0.0f, 1.0f)]
				public float volume = 1.0f;

				public float GetNextTime(){
						return (float) Random.Range(frequency_min, frequency_max);
				}

		}

		public AudioClip      loopingClip;
		public AmbientSound[] sounds = new AmbientSound[1];


		private AudioSource source;

	// Use this for initialization
	void Start () {

				source = GetComponent<AudioSource>();

				if(loopingClip != null){
						source.clip = loopingClip;
						source.loop = true;
						source.Play();
				}else{
						source.loop = false;
				}

				for(int i=0; i<sounds.Length; i++){
						StartCoroutine(ScheduleEffect(sounds[i]));
				}
	
	}
	

		IEnumerator ScheduleEffect(AmbientSound sound){

				yield return new WaitForSeconds(sound.GetNextTime());

				source.PlayOneShot(sound.clip, sound.volume);

				StartCoroutine(ScheduleEffect(sound));
		}
}
