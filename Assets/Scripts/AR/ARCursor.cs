using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    [SerializeField]
    private GameObject cursorObject;
    [SerializeField]
    private ARRaycastManager raycastManager;

    // normalised position of cursor on screen
    private float cursorX;
    private float cursorY;

    // Start is called before the first frame update
    void Start()
    {
        /// TODO: calculate normalised position of cursor
        cursorX = 0.5f;
        cursorY = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RaycastFromCursor()
    {
        // get screen position of cursor
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(cursorX, cursorY));

        /// TODO: raycast and see hits with trash
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
    }
}
