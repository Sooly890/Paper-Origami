using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Zorax.PaperOrigami.CPU
{
	public class CPUPaper : MonoBehaviour
	{
		Mesh mesh;

		Vector3[] vertices;
		int[] triangles;

		public int xSize = 20;
		public int zSize = 20;

		public MeshFilter meshFilter;
		public MeshCollider meshCollider;
		public Camera camera;

		public List<Job> jobs = new List<Job>();

		void Start()
		{
			mesh = new Mesh();
			CreateShape();
			UpdateMesh();
		}

		void Update()
		{
			if (jobs.Count == 0)
			{
				if (Input.GetMouseButton(0))
				{
					ChainEditVertex(SearchVector3Array(new Vector3(0,0,5), vertices));
					/*
					Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);

					if (Physics.Raycast(pos, camera.transform.position, out RaycastHit hit, 1000))
					{
						if (hit.transform == transform)
						{
							ChainEditVertex(SearchVector3Array(pos, vertices));
						}
					}
					*/
				}
			}
			else
			{
				ContinueChainEditVertex();
			}

			SetMesh();
		}

		[System.Serializable]
		public class Job
		{
			public Vector2Int pos;
			public Vector2Int dir;
			public int index;
			public Job(Vector2Int pos, Vector2Int dir)
			{
				this.pos = pos;
				this.dir = dir;
			}
		};

		void AddJob(Job job)
		{
			List<Job> j = new List<Job>();

			if (job.dir != Vector2Int.right) j.Add(new Job(job.pos + -Vector2Int.right, -Vector2Int.right));
			if (job.dir != Vector2Int.down) j.Add(new Job(job.pos + -Vector2Int.down, -Vector2Int.down));
			if (job.dir != Vector2Int.left) j.Add(new Job(job.pos + -Vector2Int.left, -Vector2Int.left));
			if (job.dir != Vector2Int.up) j.Add(new Job(job.pos + -Vector2Int.up, -Vector2Int.up));

			for (int i = 0; i < j.Count; i++)
			{
				Debug.Log("i ========== " + i);
				Debug.Log("list length " + j.Count);

				try
				{
					int index = Pos(j[i].pos);
					if (vertices[index] == Vector3.zero) ;
					j[i].index = index;
				}
				catch
				{
					j.RemoveAt(i);
					Debug.Log("j[i] pos =   " + j[i].pos);
				}
			}

			jobs.AddRange(j);
		}

		void ContinueChainEditVertex()
		{
			LoopCEV();
		}

		void ChainEditVertex(int vertex)
		{
			jobs = new List<Job>
			{
				new Job(Idx(vertex), Vector2Int.zero)
			};
			AddJob(jobs[0]);
			LoopCEV();
		}

		void LoopCEV()
		{
			static Vector3 MoveInRange(Vector3 pos, Vector3 other, float range)
			{
				Vector3 result;

				if (Vector3.Distance(pos, other) > range)
				{
					Vector3 dir = (pos - other).normalized;
					result = dir * range;
				}
				else
				{
					result = pos;
				}

				return result;
			}

			int i = 0;
			Job last = jobs[0];

			while (jobs.Count > 0)
			{
				AddJob(jobs[0]);
				vertices[jobs[0].index] = MoveInRange(vertices[jobs[0].index], vertices[last.index], 1.0f);

				last = jobs[0];
				jobs.RemoveAt(0);
				if (i <= 0) break;
				i--;
			}
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

		int Pos(int x, int z)
		{
			return z + (x * zSize);
		}
		int Pos(Vector2Int p)
		{
			return p.y + (p.x * zSize);
		}

		Vector2Int Idx(int i)
		{
			return new Vector2Int(Mathf.FloorToInt(zSize / i), zSize % i);
		}

		void CreateShape()
		{
			vertices = new Vector3[(xSize + 1) * (zSize + 1)];

			for (int i = 0, z = 0; z <= zSize; z++)
			{
				for (int x = 0; x <= xSize; x++)
				{
					vertices[i] = new Vector3(x, 0, z);

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
			meshCollider.sharedMesh = mesh;
		}

	}
}