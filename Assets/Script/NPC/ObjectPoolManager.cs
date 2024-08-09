using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject objectPoolParent;
    public ObjectPool[] prefabPools;

    // Reference to the other script
    public QueueManager queueManager;

    [System.Serializable]
    public class ObjectPool
    {
        public GameObject prefab;
        public int amount;
    }

    void Start()
    {
        Debug.Log("ObjectPoolManager Start");
        InstantiatePrefab();
        if (queueManager != null)
        {
            queueManager.FillQueueAtStart();
        }
        else
        {
            Debug.LogError("QueueManager is not assigned in ObjectPoolManager.");
        }
    }

    public void InstantiatePrefab()
    {
        for (int i = 0; i < prefabPools.Length; i++)
        {
            for (int j = 0; j < prefabPools[i].amount; j++)
            {
                GameObject obj = Instantiate(prefabPools[i].prefab, objectPoolParent.transform);
                obj.SetActive(false);
                Debug.Log($"Instantiated prefab {prefabPools[i].prefab.name} and set it inactive.");
            }
        }
    }
}
