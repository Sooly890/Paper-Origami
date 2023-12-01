using System.Collections.Generic;
using UnityEngine;

public class PaperVertex : MonoBehaviour
{
	public List<GameObject> gameObjects = new List<GameObject>();
	public int right;
	public int up;
	public int left = -1;
	public int down = -1;

	private Vector3 lastPos;

	void Start()
	{
		
	}

	void Update()
	{
		if (right >= 0)
		{
			float rightDist = Vector3.Distance(gameObjects[right].transform.position, transform.position);

			if (rightDist > 1.0f)
			{
				Vector3 direction = (gameObjects[right].transform.position - transform.position).normalized;

				if (lastPos == transform.position)
				{
					transform.position = gameObjects[right].transform.position - direction;
				}
				else
				{
					gameObjects[right].transform.position = transform.position + direction;
				}
			}
		}

		if (up >= 0)
		{
			float upDist = Vector3.Distance(gameObjects[up].transform.position, transform.position);

			if (upDist > 1.0f)
			{
				Vector3 direction = (gameObjects[up].transform.position - transform.position).normalized;

				if (lastPos == transform.position)
				{
					transform.position = gameObjects[up].transform.position - direction;
				}
				else
				{
					gameObjects[up].transform.position = transform.position + direction;
				}
			}
		}


		if (left >= 0)
		{
			float leftDist = Vector3.Distance(gameObjects[left].transform.position, transform.position);

			if (leftDist > 1.0f)
			{
				Vector3 direction = (gameObjects[left].transform.position - transform.position).normalized;

				if (lastPos == transform.position)
				{
					transform.position = gameObjects[left].transform.position - direction;
				}
				else
				{
					gameObjects[left].transform.position = transform.position + direction;
				}
			}
		}

		if (down >= 0)
		{
			float downDist = Vector3.Distance(gameObjects[down].transform.position, transform.position);

			if (downDist > 1.0f)
			{
				Vector3 direction = (gameObjects[down].transform.position - transform.position).normalized;

				if (lastPos == transform.position)
				{
					transform.position = gameObjects[down].transform.position - direction;
				}
				else
				{
					gameObjects[down].transform.position = transform.position + direction;
				}
			}
		}

		lastPos = transform.position;
	}
}
