using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public QueuePosition[] queuePositions;
    public bool isQueueFull = false;

    public GameObject queuePositionParent;

    // Reference to the other script
    public ScoreManager scoreManager;
    public ObjectPoolManager objectPoolManager;
    public AudioManager audioManager;

    [System.Serializable]
    public class QueuePosition
    {
        public float x;
        public float y;
        public float scale;
        public string sortingLayerName; // Changed from float to string
        public bool isOccupied;
    }

    void Awake()
{
    // Automatically find the AudioManager instance in the scene if it's not assigned in the Inspector
    if (audioManager == null)
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }
}

    void Start()
    {
        audioManager.PlaySound("Main Menu");
        // Initialize queue positions
        for (int i = 0; i < queuePositions.Length; i++)
        {
            queuePositions[i].isOccupied = false;
        }

        Debug.Log("QueueManager Start");
        FillQueueAtStart();
    }

    void Update()
    {
        InputSystem();

        // Check if the first queue position is unoccupied and fill it
        if (!queuePositions[0].isOccupied)
        {
            FillFirstQueuePosition();
        }

        // Scoring System
    }

    public void InputSystem()
    {
        ScoringSystem();
        if (Input.GetKeyDown(KeyCode.LeftArrow) || 
            Input.GetKeyDown(KeyCode.DownArrow) || 
            Input.GetKeyDown(KeyCode.RightArrow) || 
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Playing Swipe Sound");
            audioManager.PlaySound("Swipe Sound");
            SwipePerson();
            ShufflePrefabs();
        }
    }
    
    public void ScoringSystem()
    {
        // Find the last occupied index
        int lastOccupiedIndex = -1;
        for (int i = queuePositions.Length - 1; i >= 0; i--)
        {
            if (queuePositions[i].isOccupied)
            {
                lastOccupiedIndex = i;
                break;
            }
        }

        // Ensure the last occupied index is valid
        if (lastOccupiedIndex >= 0 && lastOccupiedIndex < queuePositionParent.transform.childCount)
        {
            Transform lastOccupied = queuePositionParent.transform.GetChild(lastOccupiedIndex);
            string tag = lastOccupied.tag;

            if (Input.GetKeyDown(KeyCode.LeftArrow) && tag == "Male")
            {
                scoreManager.currentScore += 1;
                Debug.Log("Score: " + scoreManager.currentScore);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && tag == "Robot")
            {
                scoreManager.currentScore += 1;
                Debug.Log("Score: " + scoreManager.currentScore);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && tag == "Female")
            {
                scoreManager.currentScore += 1;
                Debug.Log("Score: " + scoreManager.currentScore);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && tag == "Alien")
            {
                scoreManager.currentScore += 1;
                Debug.Log("Score: " + scoreManager.currentScore);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || 
                    Input.GetKeyDown(KeyCode.DownArrow) || 
                    Input.GetKeyDown(KeyCode.RightArrow) || 
                    Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("Game Over");
                scoreManager.isGameOver = true;
            }
        }
    }


    public void FillQueueAtStart()
    {
        Debug.Log("Filling Queue at Start");
        foreach (Transform child in objectPoolManager.objectPoolParent.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                for (int i = 0; i < queuePositions.Length; i++)
                {
                    if (!queuePositions[i].isOccupied)
                    {
                        child.gameObject.SetActive(true);
                        child.SetParent(queuePositionParent.transform);
                        child.localPosition = new Vector3(queuePositions[i].x, queuePositions[i].y, 0);
                        child.localScale = new Vector3(queuePositions[i].scale, queuePositions[i].scale, 1);
                        
                        // Set the sorting layer for all child GameObjects
                        SetSortingLayerForAllChildren(child, queuePositions[i].sortingLayerName);

                        queuePositions[i].isOccupied = true;
                        Debug.Log($"Moved prefab to queue position {i} at ({queuePositions[i].x}, {queuePositions[i].y}).");
                        
                        // Ensure the child is at the correct index in the hierarchy
                        child.SetSiblingIndex(i);
                        break;
                    }
                }
            }
        }
        CheckIfQueueIsFull();
    }

    public void FillPrefabToQueue()
    {
        foreach (Transform child in objectPoolManager.objectPoolParent.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                for (int i = 0; i < queuePositions.Length; i++)
                {
                    if (!queuePositions[i].isOccupied)
                    {
                        child.gameObject.SetActive(true);
                        child.SetParent(queuePositionParent.transform);
                        child.localPosition = new Vector3(queuePositions[i].x, queuePositions[i].y, 0);
                        child.localScale = new Vector3(queuePositions[i].scale, queuePositions[i].scale, 1);
                        
                        // Set the sorting layer for all child GameObjects
                        SetSortingLayerForAllChildren(child, queuePositions[i].sortingLayerName);

                        queuePositions[i].isOccupied = true;
                        Debug.Log($"Moved prefab to queue position {i} at ({queuePositions[i].x}, {queuePositions[i].y}).");
                        
                        // Ensure the child is at the correct index in the hierarchy
                        child.SetSiblingIndex(i);
                        CheckIfQueueIsFull();
                        return;
                    }
                }
            }
        }
    }

    private void FillFirstQueuePosition()
    {
        foreach (Transform child in objectPoolManager.objectPoolParent.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.SetParent(queuePositionParent.transform);
                child.localPosition = new Vector3(queuePositions[0].x, queuePositions[0].y, 0);
                child.localScale = new Vector3(queuePositions[0].scale, queuePositions[0].scale, 1);
                
                // Set the sorting layer for all child GameObjects
                SetSortingLayerForAllChildren(child, queuePositions[0].sortingLayerName);

                queuePositions[0].isOccupied = true;
                
                // Ensure the child is at the correct index in the hierarchy
                child.SetSiblingIndex(0);
                break;
            }
        }
    }

    private void ShufflePrefabs()
    {
        int childCount = objectPoolManager.objectPoolParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = objectPoolManager.objectPoolParent.transform.GetChild(i);
            int randomIndex = Random.Range(0, childCount);
            child.SetSiblingIndex(randomIndex);
        }
    }

    private void CheckIfQueueIsFull()
    {
        isQueueFull = true;
        for (int i = 0; i < queuePositions.Length; i++)
        {
            if (!queuePositions[i].isOccupied)
            {
                isQueueFull = false;
                break;
            }
        }
    }

    public void SwipePerson()
    {
        int lastOccupiedIndex = -1;

        for (int i = 0; i < queuePositions.Length; i++)
        {
            if (queuePositions[i].isOccupied)
            {
                lastOccupiedIndex = i;
            }
        }

        if (lastOccupiedIndex != -1)
        {
            Transform lastOccupied = queuePositionParent.transform.GetChild(lastOccupiedIndex);
            lastOccupied.gameObject.SetActive(false);
            lastOccupied.SetParent(objectPoolManager.objectPoolParent.transform);
            queuePositions[lastOccupiedIndex].isOccupied = false;

            for (int i = lastOccupiedIndex; i > 0; i--)
            {
                Transform current = queuePositionParent.transform.GetChild(i - 1);
                current.localPosition = new Vector3(queuePositions[i].x, queuePositions[i].y, 0);
                current.localScale = new Vector3(queuePositions[i].scale, queuePositions[i].scale, 1);
                
                // Set the sorting layer for all child GameObjects
                SetSortingLayerForAllChildren(current, queuePositions[i].sortingLayerName);

                queuePositions[i].isOccupied = true;
                queuePositions[i - 1].isOccupied = false;
            }

            // Set the first position to inactive
            if (queuePositions[0].isOccupied)
            {
                Transform first = queuePositionParent.transform.GetChild(0);
                first.gameObject.SetActive(false);
                queuePositions[0].isOccupied = false;
            }
        }
    }

    private void SetSortingLayerForAllChildren(Transform parent, string sortingLayerName)
    {
        foreach (Transform child in parent)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = sortingLayerName;
            }
        }
    }
}
