using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Zorax.PaperOrigami.UnityPhysics
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class GenerateMesh : MonoBehaviour
	{
		Mesh mesh;

		Vector3[] vertices;
		int[] triangles;

		public int xSize = 20;
		public int zSize = 20;

		public MeshFilter meshFilter;

		void Start()
		{
			mesh = new Mesh();
			CreateShape();
			UpdateMesh();
		}

		void Update()
		{
			UpdateMesh();
			SetMesh();
		}

		void CreateShape()
		{
			vertices = new Vector3[(xSize + 1) * (zSize + 1)];

			for (int i = 0, z = 0; z <= zSize; z++)
			{
				for (int x = 0; x <= xSize; x++)
				{
					Vector3 pos = new Vector3((float)x, 0, (float)z);
					vertices[i] = pos;

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

					vert++;
					tris += 6;
				}

				vert++;

			}

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
	}
}