using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zorax.PaperOrigami.UnityPhysics
{
	[RequireComponent(typeof(MeshFilter))]
	public class Metal : MonoBehaviour
	{
		Mesh mesh;

		Vector3[] vertices;
		int[] triangles;

		public int xSize = 20;
		public int zSize = 20;

		public MeshFilter meshFilter;

		public GameObject prefab;

		public List<GameObject> gameObjects = new List<GameObject>();

		void Start()
		{
			mesh = new Mesh();
			CreateShape();
			UpdateMesh();
		}

		void Update()
		{
			UpdatePoints();
			UpdateMesh();
			SetMesh();
		}

		int SearchVector3Array(Vector3 v, Vector3[] values)
		{
			int minIdx = -1;
			Vector3 min = Vector3.one * Mathf.Infinity;

			for (int i = 0; i < values.Length; i++)
			{
				if (Vector3.Distance(values[i], v) < Vector3.Distance(min, v))
				{
					min = values[i];
					minIdx = i;
				}
			}

			return minIdx;
		}

		void CreateShape()
		{
			gameObjects = new List<GameObject>();
			vertices = new Vector3[(xSize + 1) * (zSize + 1)];

			for (int i = 0, z = 0; z <= zSize; z++)
			{
				for (int x = 0; x <= xSize; x++)
				{
					Vector3 pos = new Vector3((float)x, 0, (float)z);
					vertices[i] = pos;
					gameObjects.Add(Instantiate(prefab, pos, Quaternion.identity));

					i++;
				}

			}

			triangles = new int[xSize * zSize * 6];
			int vert = 0;
			int tris = 0;

			for (int z = 0; z < zSize; z++)
			{
				for (int x = 0; x < xSize; x++)
				{
					triangles[tris + 0] = vert + 0;
					triangles[tris + 1] = vert + xSize + 1;
					triangles[tris + 2] = vert + 1;

					triangles[tris + 3] = vert + 1;
					triangles[tris + 4] = vert + xSize + 1;
					triangles[tris + 5] = vert + xSize + 2;

					HingeJoint hingeJoint;
					hingeJoint = ConfigHingeJoints(vert, vert + 1);
					hingeJoint = ConfigHingeJoints(vert, vert + xSize + 1);

					vert++;
					tris += 6;
				}

				vert++;

			}

		}

		HingeJoint ConfigHingeJoints(int vert, int connectedVert)
		{
			HingeJoint ConfigOneHingeJoint(HingeJoint joint)
			{
				joint.useLimits = true;
				JointLimits jointLimits = joint.limits;
				jointLimits.min = 0;
				jointLimits.max = 1.0f;
				joint.limits = jointLimits;

				return joint;
			}

			HingeJoint hingeJoint;

			hingeJoint = gameObjects[vert].AddComponent<HingeJoint>();
			hingeJoint.connectedBody = gameObjects[connectedVert].GetComponent<Rigidbody>();
			hingeJoint.axis = new Vector3(1, 0, 0);
			hingeJoint = ConfigOneHingeJoint(hingeJoint);

			hingeJoint = gameObjects[vert].AddComponent<HingeJoint>();
			hingeJoint.connectedBody = gameObjects[connectedVert].GetComponent<Rigidbody>();
			hingeJoint.axis = new Vector3(0, 1, 0);
			hingeJoint = ConfigOneHingeJoint(hingeJoint);

			hingeJoint = gameObjects[vert].AddComponent<HingeJoint>();
			hingeJoint.connectedBody = gameObjects[connectedVert].GetComponent<Rigidbody>();
			hingeJoint.axis = new Vector3(0, 0, 1);
			hingeJoint = ConfigOneHingeJoint(hingeJoint);

			return hingeJoint;
		}

		void UpdateMesh()
		{
			mesh.Clear();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();

			SetMesh();
		}

		void SetMesh()
		{
			meshFilter.mesh = mesh;
			//meshCollider.sharedMesh = mesh;
		}

		void UpdatePoints()
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = gameObjects[i].transform.position;
			}
		}

	}
}