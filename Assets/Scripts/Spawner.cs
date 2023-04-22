using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject prefab;
	public float absoluteMinTime = 0.1f;
	public float minTime;
	public float maxTime;
	public bool repeat = true;
	public PathNode startNode;
	public Transform spawns;

	private float timeSinceLastSpawn;
	private float nextSpawnTime;

	private void Start()
	{
		nextSpawnTime = Random.Range(minTime, maxTime);

		if (startNode == null) Console.Instance.Log("You didn't set startNode");
	}

	void Update()
	{
		timeSinceLastSpawn += Time.deltaTime;
		if (timeSinceLastSpawn > nextSpawnTime)
		{
			var spawned = Instantiate(prefab, spawns);
			spawned.transform.position = startNode.transform.position;
			spawned.GetComponent<Enemy>().Initialize(startNode);

			nextSpawnTime = Random.Range(minTime, maxTime);
			nextSpawnTime = Mathf.Clamp(nextSpawnTime * (1 - Game.Instance.Difficulty), absoluteMinTime, maxTime);
			timeSinceLastSpawn = 0;
		}
	}

	public void Reset()
	{
		foreach (Transform t in spawns)
		{
			Destroy(t.gameObject);
		}
	}
}
