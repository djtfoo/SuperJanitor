using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Overall game manager
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
    [SerializeField]
    private ScoreManager scoreManager;

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI trashCounterText;

    private int numTrashPicked = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Function to call when wanting to start the game. Will initialise the necessary components.
    /// </summary>
    public void StartGame()
    {
        // Find the floor plane and spawn trash
        enemyManager.SpawnTrash();
    }

    /// <summary>
    /// Function to call when player has picked up trash successfully.
    /// </summary>
    public void PickedUpTrash()
    {
        // increment no. trash picked up
        ++numTrashPicked;
        trashCounterText.text = numTrashPicked.ToString();

        // increment game score
        /// TODO: determine if x2 score or x1 score
        scoreManager.IncrementScore(100);
    }
}
