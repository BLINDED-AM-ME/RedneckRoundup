using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour {

		public Transform[] spawn_points;

		private List<int> takenIndexes = new List<int>();
	

		public void SpawnObjects(int num, GameObject spawn_object){

				int index = 0;

				for(int i=0; i<num; i++){

						index = Random.Range(0, spawn_points.Length);

						while(takenIndexes.Contains(index)){
								index = (index + 1) % spawn_points.Length;
						}

						takenIndexes.Add(index);

						Instantiate(spawn_object, spawn_points[index].position, Quaternion.identity);
				}
		}

}
