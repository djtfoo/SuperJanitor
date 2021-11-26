using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuctionRaycast : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private TextMeshProUGUI debugText;

    [Header("Vacuum Animation")]
    [SerializeField]
    private string suckAnimTrigger = "Vacuum";
    [SerializeField]
    private Animator animator;

    // normalised position of cursor on screen
    private float cursorX = 0.5f;
    private float cursorY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        animator.ResetTrigger(suckAnimTrigger);
    }

    public void TrySucking()
    {
        // Play animation
        animator.SetTrigger(suckAnimTrigger);

        // 2D raycast from center of screen
        Vector2 screenPosition = arCamera.ViewportToScreenPoint(new Vector2(cursorX, cursorY));
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

        // Check if raycast hit a collider
        if (hit2D.collider != null)
        {
            // TEMP: Display debug text
            //debugText.gameObject.SetActive(true);
            //debugText.text = "RAYCAST!!!!";

            // Get the Trash item hit by the raycast
            Trash trash = hit2D.transform.GetComponent<Trash>();
            if (trash != null)
            {
                // update trash picked data and increment score
                gameManager.PickedUpTrash(trash);
            }
            else  // might be cockroach
            {
                BossEnemy enemy = hit2D.transform.GetComponent<BossEnemy>();
                if (enemy != null)
                {
                    enemy.GetHit();
                    scoreManager.IncrementKillStreak(1);
                }
            }
        }
        else
        {
            // TEMP: Display debug text
            //debugText.gameObject.SetActive(true);
            //debugText.text = "NO RAYCAST :(";
        }
    }
}
