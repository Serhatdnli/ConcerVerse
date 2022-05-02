using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPooler : Singleton<ObjectsPooler>
{

    public List<Pool> pools;
    private Dictionary<string, Pool> poolDictionary;


    private void Awake()
    {
        poolDictionary = new Dictionary<string, Pool>();
        for (int i = 0; i < pools.Count; i++)
        {
            poolDictionary.Add(pools[i].poolName, pools[i]);
        }
    }

    public GameObject CreateObject(GameObject prefab)
    {
        GameObject createdObject = Instantiate(prefab);
        createdObject.SetActive(false);
        return createdObject;
    }

    public GameObject GetObjectFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(tag))
        {

            GameObject objectToSpawn = poolDictionary[tag].GetObject(position, rotation);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            return objectToSpawn;
        }
        else
        {
            print("Pool wit tag " + tag + " doesn't excist.");
            return null;
        }
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag].ReturnToPool(obj);
        }
        else
        {
            print("Pool wit tag " + tag + " doesn't excist.");
        }
    }


}

[System.Serializable]
public class Pool
{
    public string poolName;
    public GameObject prefab;

    private List<GameObject> items = new List<GameObject>();
    public List<GameObject> Items { get => items; }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (items.Count > 0)
        {
            obj = items[0];
            items.RemoveAt(0);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            items.Add(ObjectsPooler.Instance.CreateObject(prefab));
            return GetObject(position, rotation);
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        if (!items.Contains(obj))
        {
            obj.SetActive(false);
            items.Add(obj);
        }

    }


}

