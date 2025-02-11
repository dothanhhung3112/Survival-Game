using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public Transform parent;
        public int size;
    }
    public static ObjectPooler instance;
    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        //object
        poolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (var pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.parent);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SetObject(string tag, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject prefab = null;
        foreach (GameObject obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);

                obj.transform.position = new Vector3(position.x, position.y, obj.transform.position.z);
                prefab = obj;
                return prefab;
            }
        }

        if (prefab == null)
        {
            foreach (var pool in pools)
            {
                if (pool.tag == tag)
                {
                    prefab = Instantiate(pool.prefab, pool.parent);
                    prefab.transform.position = new Vector3(position.x, position.y, prefab.transform.position.z);
                    return prefab.gameObject;
                }
            }
        }
        return null;
    }

    public void SetInactive(GameObject obj, float delay)
    {
        StartCoroutine(SetInactiveAfterDelay(obj, delay));
    }

    IEnumerator SetInactiveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}
