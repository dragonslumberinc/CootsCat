using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnerObject
{
    public string name;
    public int maxCount;
    public Transform[] prefabs;
    public Transform[] targets;
    public List<Transform> freePrefabs;
    public List<Transform> inUsePrefabs;
}

public class Spawner : MonoBehaviour
{
    public SpawnerObject[] spawnerObjects;

    private void Start()
    {
        Transform prefabTransform = null;
        Array.ForEach(spawnerObjects, spawnerObject =>
        {
            for(int i = 0; i <  spawnerObject.targets.Length; i++)
            {
                spawnerObject = getPrefab(spawnerObject, out prefabTransform);

                prefabTransform.transform.position = spawnerObject.targets[i].position;
            }
        });
    }

    private SpawnerObject getPrefab(SpawnerObject spawnerObject, out Transform prefabTransform)
    {
        int random;
        if (spawnerObject.freePrefabs.Count > 0)
        {
            random = UnityEngine.Random.Range(0, spawnerObject.freePrefabs.Count);
            prefabTransform = spawnerObject.freePrefabs[random];
            spawnerObject.freePrefabs.RemoveAt(random);
        }
        else
        {
            random = UnityEngine.Random.Range(0, spawnerObject.prefabs.Length);
            prefabTransform = Instantiate<Transform>(spawnerObject.prefabs[random]);
        }

        prefabTransform.gameObject.SetActive(true);
        spawnerObject.inUsePrefabs.Add(prefabTransform);

        return spawnerObject;
    }

    public void spawnAt(string name, Vector3 pos, Vector3 normalUp, Transform parent)
    {
        Transform prefabTransform = null;
        int index = Array.FindIndex(spawnerObjects, e => e.name == name);
        spawnerObjects[index] = getPrefab(spawnerObjects[index], out prefabTransform);
        prefabTransform.transform.position = pos;
        prefabTransform.transform.up = normalUp;
        prefabTransform.transform.parent = parent;
    }
}
