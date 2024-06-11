using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;  // The prefab to spawn
    public int numberOfObjects = 10;  // Number of objects to spawn
    public Vector3 spawnAreaMin;      // Minimum position for spawn area
    public Vector3 spawnAreaMax;      // Maximum position for spawn area

    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);

        Vector3 spawnPosition = new Vector3(x, y, z);
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
