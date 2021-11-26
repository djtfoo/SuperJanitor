using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    [Header("AR Managers")]
    [SerializeField]
    private ARPlaneManager planeManager;
    [SerializeField]
    private ARSessionOrigin sessionOrigin;

    [Header("Spawn Parameters")]
    [SerializeField]
    private int numSpawnsPerWave = 5;
    [SerializeField]
    private float floorSizeX;
    [SerializeField]
    private float floorSizeZ;
    [SerializeField]
    private GameObject worldRootPrefab;
    [SerializeField]
    private GameObject[] trashPrefabs;

    [Header("Debug Text")]
    [SerializeField]
    private GameObject debugText;

    private Transform worldRootTransform;
    private bool gameWorldSpawned = false;

    private List<Transform> trashList;

    void Awake()
    {
        trashList = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        planeManager.planesChanged += DisablePlaneRendering;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisablePlaneRendering(ARPlanesChangedEventArgs args)
    {
        if (gameWorldSpawned)
        {
            foreach (ARPlane plane in args.added)
            {
                plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
            }
        }
    }

    public void SpawnGameWorld()
    {
        ARPlane floorPlane = null;
        // find the largest plane, assume it's the floor (PlaneClassification.Floor does not work)
        float largestSize = 0f;
        foreach (ARPlane plane in planeManager.trackables)
        {
            float planeSize = plane.size.x * plane.size.y;
            if (planeSize > largestSize)
            {
                floorPlane = plane;
                largestSize = planeSize;
            }
        }

        if (floorPlane != null) // there is at least 1 ARPlane detected
        {
            SpawnGameWorld(floorPlane);
            gameWorldSpawned = true;
        }
        else
        {
            debugText.SetActive(true);
        }

        // disable all plane rendering
        ARPlane[] planes = (ARPlane[])GameObject.FindObjectsOfType(typeof(ARPlane));
        foreach (ARPlane plane in planes)
        {
            plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
        }
    }

    private void SpawnGameWorld(ARPlane plane)
    {
        // instantiate root of the gameplay area
        GameObject worldRoot = Instantiate(worldRootPrefab);
        worldRootTransform = worldRoot.transform;

        // Trigger enemy's throwing animation
        worldRootTransform.GetComponent<GameWorld>().BossEnemy.PlayThrowAnim();

        // spawn the gameplay area
        sessionOrigin.MakeContentAppearAt(worldRootTransform, plane.center);

        // spawn initial trash wave
        //SpawnTrashWave();
    }

    /// <summary>
    /// Spawn a wave of trash and returns the List of Trash.
    /// (Allows the trash to be moved along its thrown trajectory)
    /// </summary>
    /// <returns>List of spawned Trash</returns>
    public List<Transform> SpawnTrashWave()
    {
        // spawn some trash
        for (int i = 0; i < numSpawnsPerWave; ++i)
        {
            SpawnTrash();
        }

        // return trash list to be worked on
        return trashList;
    }

    public Transform SpawnTrash()
    {
        // randomly spawn a trash object
        int randIdx = Random.Range(0, trashPrefabs.Length);
        GameObject newTrash = Instantiate(trashPrefabs[randIdx]);
        // add as child of the root object
        newTrash.transform.SetParent(worldRootTransform);
        // randomly set relative position of trash object
        newTrash.transform.localPosition = GetRandomLocalPosition();

        // add to trash list
        trashList.Add(newTrash.transform);

        return newTrash.transform;
    }

    public void TrashIsRemoved(Trash trash)
    {
        // Remove Trash from List
        trashList.Remove(trash.transform);

        // If all Trash is removed, make boss vulnerable
        if (trashList.Count == 0)
        {
            worldRootTransform.GetComponent<GameWorld>().BossEnemy.SetVulnerableToHits();
        }
    }

    private Vector3 GetRandomLocalPosition()
    {
        // randomly set relative position of trash object
        float randomX = Random.Range(-0.4f * floorSizeX, 0.4f * floorSizeX);    // trash will be within 80% of the play area
        float randomZ = Random.Range(-0.4f * floorSizeZ, 0.4f * floorSizeZ);    // trash will be within 80% of the play area

        return new Vector3(randomX, 0f, randomZ);
    }

    /* For boss to move to random position in world, but not in use anymore
    public Vector3 GetRandomPosition()
    {
        Vector3 randomLocalPos = GetRandomLocalPosition();
        return worldRootTransform.position + randomLocalPos;
    }*/
}
