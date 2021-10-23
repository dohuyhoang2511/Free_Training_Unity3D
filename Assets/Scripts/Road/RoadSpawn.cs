using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawn : MonoBehaviour
{
    public GameObject[] prefabs;
    private Transform player;

    private List<GameObject> activePrefabs;

    public float backArea = 50.0f;
    public int prefabsOnScreen = 4;
    public int lastPrefab = 0;
    public float spawnPrefabAtPosZ = -21.0f;
    public float prefabLength = 20f;
	//=========================================================

	public GameObject[] prefabsObstacles;
	public Transform[] xPos;

	private List<GameObject> activePrefabsObstacles;

	// Start is called before the first frame update
	void Start()
    {
		activePrefabs = new List<GameObject>();
		activePrefabsObstacles = new List<GameObject>();

		player = GameObject.FindGameObjectWithTag("Player").transform;

		for (int i = 0; i < prefabsOnScreen; i++)
		{
			if (i < prefabsOnScreen)
				Spawn(0);
			else
				Spawn();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (player.position.z - backArea > (spawnPrefabAtPosZ - prefabsOnScreen * prefabLength))
		{
			Spawn();
			DeletePrefab();
		}
		DestroyObstacles();
	}

	private void Spawn(int prefabIndex = -1)
	{
		GameObject myPrefab;
		if (prefabIndex == -1)
        {
			myPrefab = Instantiate(prefabs[RandomPrefabs()]);
		}
        else
        {
			myPrefab = Instantiate(prefabs[prefabIndex]);
		}
		myPrefab.transform.SetParent(transform);
		myPrefab.transform.position = Vector3.forward * spawnPrefabAtPosZ;
		spawnPrefabAtPosZ += prefabLength;
		activePrefabs.Add(myPrefab);

		int randomNumbersObstacles = Random.Range(2, 5);
		for(int i =0; i < randomNumbersObstacles; i++)
        {
			SpawnObstacles();
		}
	}

	private void DeletePrefab()
	{
		Destroy(activePrefabs[0]);
		activePrefabs.RemoveAt(0);
	}

	private int RandomPrefabs()
	{
		if (prefabs.Length <= 1)
			return 0;
		int randomIndex = lastPrefab;
		while (randomIndex == lastPrefab)
		{
			randomIndex = Random.Range(0, prefabs.Length);
		}

		lastPrefab = randomIndex;
		return randomIndex;
	}

	void SpawnObstacles()
	{
		Vector3 posInstantiate = new Vector3(xPos[RandomXPostion()].position.x, 0.5f, RandomZPostion());

		GameObject obstacle = Instantiate(prefabsObstacles[RandomPrefabsObstacles()], posInstantiate, Quaternion.identity);

		activePrefabsObstacles.Add(obstacle);
	}

	private int RandomPrefabsObstacles()
	{
		int randomIndex = Random.Range(0, prefabsObstacles.Length);

		return randomIndex;
	}

	private int RandomXPostion()
	{
		int randomXIndex = Random.Range(0, xPos.Length);

		return randomXIndex;
	}

	private float RandomZPostion()
	{
		float randomZ = Random.Range(spawnPrefabAtPosZ - 18f, spawnPrefabAtPosZ - 2f);

		return randomZ;
	}

	void DestroyObstacles()
    {
		if(player.position.z > activePrefabsObstacles[0].transform.position.z + 1.5f)
        {
			Destroy(activePrefabsObstacles[0], 1.5f);
			activePrefabsObstacles.RemoveAt(0);
		}
	}
}
