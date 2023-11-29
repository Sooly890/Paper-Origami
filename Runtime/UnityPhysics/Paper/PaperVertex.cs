using System.Collections.Generic;
using UnityEngine;

public class PaperVertex : MonoBehaviour
{
	public List<GameObject> gameObjects = new List<GameObject>();
	public int right;
	public int up;

	private Vector3 lastPos;

	public bool tryMiddle = false;

	void Start()
	{
		
	}

	void Update()
	{
		float rightDist = Vector3.Distance(gameObjects[right].transform.position, transform.position);
		float upDist    = Vector3.Distance(gameObjects[up   ].transform.position, transform.position);

		if (rightDist > 1.0f && upDist > 1.0f && tryMiddle)
		{

			Vector3 rightDirection = (gameObjects[right].transform.position - transform.position).normalized;
			Vector3 upDirection = (gameObjects[up].transform.position - transform.position).normalized;

			Vector3 direction = (rightDirection + upDirection).normalized;

			if (lastPos == transform.position)
			{
				transform.position = gameObjects[right].transform.position - direction;
			}
			else
			{
				gameObjects[right].transform.position = transform.position + direction;
			}

		}
		else
		{
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

		lastPos = transform.position;
	}
}
