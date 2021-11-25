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

    [SerializeField]
    private Timer gameTimer;

    ///  TODO: tbh, this shld be in ScoreManager instead
    private int numTrashPicked = 0; // total no. trash picked up
    private Dictionary<string, int> trashQuantity;  // breakdown of quantities for each trash type

    // Start is called before the first frame update
    void Awake()
    {
        trashQuantity = new Dictionary<string, int>();
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

        // Start timer
        gameTimer.StartTimer();
    }

    /// <summary>
    /// Function to call when player has picked up trash successfully.
    /// </summary>
    public void PickedUpTrash(Trash trash)
    {
        // increment game score
        /// TODO: determine if x2 score (gold shadow) or x1 score
        scoreManager.IncrementScore(trash.GetScore());

        // increment no. trash picked up
        ++numTrashPicked;
        trashCounterText.text = numTrashPicked.ToString();

        // update the trash collected breakdown
        if (!trashQuantity.ContainsKey(trash.GetName())) {
            trashQuantity.Add(trash.GetName(), 0);
        }
        trashQuantity[trash.GetName()] = trashQuantity[trash.GetName()] + 1;
    }

    public int GetTrashQuantity(string name)
    {
        if (!trashQuantity.ContainsKey(name))
            return 0;
        else
            return trashQuantity[name];
    }
}
