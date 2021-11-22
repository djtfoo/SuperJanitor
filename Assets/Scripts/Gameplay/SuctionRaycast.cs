using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionRaycast : MonoBehaviour
{
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private Camera arCamera;

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

    public void TrySucking()
    {
        // get screen position of cursor
        Vector2 screenPosition = arCamera.ViewportToScreenPoint(new Vector2(cursorX, cursorY));

        /*
        /// TODO: raycast and see hits with trash
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            /// TODO: remove trash

            /// TODO: increment score
            scoreManager.IncrementScore(100);
        }*/

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, 100))
        {
            /// TODO: remove trash

            /// TODO: increment score
            scoreManager.IncrementScore(100);
        }
    }
}
