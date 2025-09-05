using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    public static ObjectPoolManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetObject(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            return null;
        }

        GameObject obj;
        if (pools[prefabIndex].Count > 0)
        {
            obj = pools[prefabIndex].Dequeue();
        }
        else
        {
            obj = Instantiate(prefabs[prefabIndex]);
            obj.GetComponent<IPoolable>()?.Initialize(o => ReturnObject(prefabIndex, o));
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn();
        return obj;
    }

    public void ReturnObject(int prefabIndex, GameObject obj)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pools[prefabIndex].Enqueue(obj);
    }
}