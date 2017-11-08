using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathHandler : MonoBehaviour {

		private List<Vector3> path = new List<Vector3>(5);
		public  float         maxSegmentDistance = 0.5f;
		public  float         lineZposition = 0.0f; // set at start

		private LineRenderer   comp_lineRenderer;


		private Vector3 temp_Vector = Vector3.zero;

		// Use this for initialization
		void Start () {
	
				comp_lineRenderer = GetComponent<LineRenderer>();


				Vector3 tempPosition = transform.position;
				tempPosition.z = lineZposition;
				path.Add(tempPosition);
		}
	
		// Update is called once per frame
		void LateUpdate () {
		
				DrawPath();
		}

		void DrawPath(){


				comp_lineRenderer.positionCount = path.Count;
				comp_lineRenderer.SetPositions(path.ToArray());

				temp_Vector = transform.position;
				temp_Vector.z = lineZposition;

				comp_lineRenderer.SetPosition(0, temp_Vector);

		}


		public int PathCount{

				get{
						return path.Count;
				}
		}

		public void SetPath(Vector3[] points){

				path.Clear();

				for(int i=0; i < points.Length; i++)
						points[i].z = lineZposition;

				path.AddRange(points);
		}

		public Vector3 GetPoint(int index){

				temp_Vector = path[index];
				temp_Vector.z = 0.0f;

				return temp_Vector;
		}

		public void SetPoint(int index, Vector3 point){

				point.z = lineZposition;
				path[index] = point;

		}


		public void InsertPoint(int index, Vector3 point){

				point.z = lineZposition;
				path.Insert(index, point);

		}

		public void RemovePoint(int index){

				path.RemoveAt(index);
		}



		#region Standard assets variables
		// standard assets

		private List<float>   points_distances = new List<float>(10);

		[HideInInspector]
		private float      TotalDistance;

		//this being here will save GC allocs
		private int p0n;
		private int p1n;
		private int p2n;
		private int p3n;

		private float i;
		private Vector3 P0;
		private Vector3 P1;
		private Vector3 P2;
		private Vector3 P3;

		#endregion


		#region Standard assets section

		public float CalculatePathDistance()
		{
				// transfer the position of each point and distances between points to arrays for
				// speed of lookup at runtime
				points_distances.Clear();

				float accumulateDistance = 0;
				for (int i = 0; i < path.Count-1; ++i)
				{
						var t1 = path[i];
						var t2 = path[i + 1];

						points_distances.Add(accumulateDistance);
						accumulateDistance += (t1 - t2).magnitude;
						
				}

				points_distances.Add(accumulateDistance);

				TotalDistance = accumulateDistance;

				return TotalDistance;
		}


		public RoutePoint GetRoutePoint(float dist)
		{
				// position and direction
				Vector3 p1 = GetRoutePosition(dist);
				Vector3 p2 = GetRoutePosition(dist + 0.1f);
				Vector3 delta = p2 - p1;
				return new RoutePoint(p1, delta.normalized);
		}


		public Vector3 GetRoutePosition(float dist)
		{

				dist = Mathf.Clamp(dist, 0.0f, TotalDistance);

				if(dist <= 0)
						return path[0];
				else if(dist >= TotalDistance)
						return path[path.Count-1];

				int point = 0;
				while (points_distances[point] < dist)
				{
						point++;
				}

				// get nearest two points, ensuring points wrap-around start & end of circuit
				p1n = point - 1;
				p2n = point;

				// found point numbers, now find interpolation value between the two middle points

				i = Mathf.InverseLerp(points_distances[p1n], points_distances[p2n], dist);

				return Vector3.Lerp(path[p1n], path[p2n], i);

		}


//		private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
//		{
//				// comments are no use here... it's the catmull-rom equation.
//				// Un-magic this, lord vector!
//				return 0.5f * ((2.0f*p1) + (-p0 + p2)*i + (2.0f*p0 - 5.0f*p1 + 4.0f*p2 - p3)*i*i + (-p0 + 3.0f*p1 - 3.0f*p2 + p3)*i*i*i);
//		}


		public struct RoutePoint
		{
				public Vector3 position;
				public Vector3 direction;


				public RoutePoint(Vector3 position, Vector3 direction)
				{
						this.position = position;
						this.direction = direction;
				}
		}

		#endregion
}
