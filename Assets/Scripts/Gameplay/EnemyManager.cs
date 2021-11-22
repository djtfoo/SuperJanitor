using UnityEngine;
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
    [SerializeField]
    private Camera arCamera;    // for billboarding 

    [Header("Spawn Parameters")]
    [SerializeField]
    private int numSpawns = 5;
    [SerializeField]
    private float floorSizeX;
    [SerializeField]
    private float floorSizeZ;
    [SerializeField]
    private GameObject worldRootPrefab;
    [SerializeField]
    private GameObject[] trashPrefabs;

    ///  TODO: take this out
    [SerializeField]
    private GameObject debugText;

    private Transform worldRootTransform;
    private bool trashHasSpawned = false;   // initial wave of trash when game starts

    // Start is called before the first frame update
    void Start()
    {
        planeManager.planesChanged += DisablePlaneRendering;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (trashHasSpawned)
        {
            // continuously update orientation of trash items
            foreach (Transform child in worldRootTransform)
            {
                //child.LookAt(arCamera.transform);
                sessionOrigin.MakeContentAppearAt(child, child.rotation);
            }
        }
    }

    private void DisablePlaneRendering(ARPlanesChangedEventArgs args)
    {
        if (trashHasSpawned)
        {
            foreach (ARPlane plane in args.added)
            {
                plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
            }
        }
    }

    public void SpawnTrash()
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
            SpawnTrash(floorPlane);
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

    private void SpawnTrash(ARPlane plane)
    {

        // instantiate root of the gameplay area
        GameObject worldRoot = Instantiate(worldRootPrefab);
        worldRootTransform = worldRoot.transform;

        // spawn some trash
        for (int i = 0; i < numSpawns; ++i)
        {
            // randomly spawn a trash object
            int randIdx = Random.Range(0, trashPrefabs.Length);
            GameObject newTrash = Instantiate(trashPrefabs[randIdx]);
            // add as child of the root object
            newTrash.transform.SetParent(worldRootTransform);
            // randomly set relative position of trash object
            float randomX = Random.Range(-0.4f * floorSizeX, 0.4f * floorSizeX);    // trash will be within 80% of the play area
            float randomZ = Random.Range(-0.4f * floorSizeZ, 0.4f * floorSizeZ);    // trash will be within 80% of the play area
            newTrash.transform.localPosition = new Vector3(randomX, 0f, randomZ);
        }

        // spawn the gameplay area
        sessionOrigin.MakeContentAppearAt(worldRootTransform, plane.center);
        trashHasSpawned = true;

        // Debug text for looking at stuff
        //debugText.SetActive(true);
        //debugText.GetComponent<TextMeshProUGUI>().text = plane.center.x + "," + plane.center.y + " || " + plane.size.x + ", " + plane.size.y;
    }
}
