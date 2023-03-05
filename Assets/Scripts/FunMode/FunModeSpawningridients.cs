using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunModeSpawningridients : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] ingridientsPrefabs;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            var spawnPointID = Random.Range(0, spawnPoints.Length);
            var ingridientsPrefabID = Random.Range(0, ingridientsPrefabs.Length);

            Instantiate(ingridientsPrefabs[ingridientsPrefabID], spawnPoints[spawnPointID].position, spawnPoints[spawnPointID].rotation).GetComponent<InteractableObject>().anotherSpawn = true;

            yield return new WaitForSeconds(1f);
        }
    }
}
