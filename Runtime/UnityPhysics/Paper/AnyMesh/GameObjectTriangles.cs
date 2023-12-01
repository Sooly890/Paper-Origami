using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameObjectTriangles : MonoBehaviour
{
	public Vector3[] vertices;
	public int[] triangles;

	private List<GameObject> gameObjects = new List<GameObject>();
	public GameObject prefab;

	public bool importFromMeshFilter = true;
	public MeshFilter meshFilter;

	int ticks = 10;

	void Awake()
	{
		
	}

	void Update()
	{
		if (ticks >= 0)
		{
			if (importFromMeshFilter)
			{
				ImportFromMeshFilter();
			}
			SetupGameObjects();
			SetupPaperVertices();

			ticks--;
		}
	}

	void ImportFromMeshFilter()
	{
		vertices = meshFilter.mesh.vertices;
		triangles = meshFilter.mesh.triangles;
	}

	void SetupGameObjects()
	{
		for (int i = 0; i < gameObjects.Count; i++)
		{
			Destroy(gameObjects[i]);
		}

		gameObjects = new List<GameObject> ();
		for (int i = 0; i < vertices.Length; i++)
		{
			gameObjects.Add(Instantiate(prefab, vertices[i], Quaternion.identity));
		}
	}

	void SetupPaperVertices()
	{
		for (int i = 0; i < triangles.Length / 3; i++)
		{
			GameObject go = gameObjects[triangles[i * 3 + 0]];
			PaperVertex paperVertex = go.AddComponent<PaperVertex>();
			paperVertex.gameObjects = gameObjects;
			paperVertex.up    = triangles[i * 3 + 1];
			paperVertex.right = triangles[i * 3 + 2];
		}
	}
}
