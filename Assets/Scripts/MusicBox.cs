using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicBox : MonoBehaviour {


		public AudioClip[] tracks = new AudioClip[1];
		public bool isRandom = false;

		private AudioSource source;
		private int current_track = -1;

		// Use this for initialization
		void Start () {

				source = GetComponent<AudioSource>();

				PlaySomething();
	
		}
	
		// Update is called once per frame
		void Update () {

				if(!source.isPlaying)
						PlaySomething();

		}

		public void PlaySomething(){


				if(isRandom)
						current_track = Random.Range(0, tracks.Length);
				else
						current_track = (current_track+1) % tracks.Length;
				

				source.clip = tracks[current_track];

				source.Play();
		}
}
