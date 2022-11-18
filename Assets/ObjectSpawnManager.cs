using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;

public class ObjectSpawnManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;

    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    [SerializeField]
    List<GameObject> spawnablePrefabList;

    Camera arCam;
    Dictionary<GameObject, Vector3> spawnedObjectPositionMap = new Dictionary<GameObject, Vector3>();

    const float OBJECT_MIN_DISTANCE = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // check first if I have object to place 
        if (spawnablePrefabList.Count == 0)
            return;

        // pick random screen coordinates
        Vector2 randomScreenPosition = new Vector2(Random.Range(0.0f, Screen.height), Random.Range(0.0f, Screen.width));
        Debug.Log("Random screen pos " + randomScreenPosition);

        RaycastHit hit;
        if (m_RaycastManager.Raycast(randomScreenPosition, m_Hits))
        {
            // check distance between selected position and the ones already in the scene in order to distriute them all over the scene
            foreach (KeyValuePair<GameObject, Vector3> entry in spawnedObjectPositionMap)
            {
                float distance = Vector3.Distance(m_Hits[m_Hits.Count - 1].pose.position, entry.Value);
                Debug.Log(distance);
                if (distance < OBJECT_MIN_DISTANCE)
                    return;
            }

            Ray ray = arCam.ScreenPointToRay(m_Hits[m_Hits.Count - 1].pose.position);
            if (Physics.Raycast(ray, out hit))
            {
                var random = new System.Random();
                int index = random.Next(spawnablePrefabList.Count);
                SpawnPrefab(spawnablePrefabList[index], m_Hits[m_Hits.Count - 1].pose.position);
                spawnablePrefabList.RemoveAt(index);

            }

        }
    }

    private void SpawnPrefab(GameObject spawnablePrefab, Vector3 spawnPosition)
    {
        spawnedObjectPositionMap[spawnablePrefab] = spawnPosition;
        Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
    }
}

