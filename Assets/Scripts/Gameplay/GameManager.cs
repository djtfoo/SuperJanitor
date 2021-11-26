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
    [SerializeField]
    private EndGameScreen endGameScreen;

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI trashCounterText;
    [Header("To enable only when game starts")]
    [SerializeField]
    private GameObject[] gameplayUI;
    [SerializeField]
    private int bossDefeatedPoints = 5000;

    [SerializeField]
    private Timer gameTimer;

    private int gameTimeTakenSecs = 0;

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    ///  TODO: tbh, this shld be in ScoreManager instead
    private int numTrashPicked = 0; // total no. trash picked up
    private bool isBossDefeated = false;
    public bool IsBossDefeated
    {
        get { return isBossDefeated; }
    }
    private Dictionary<string, int> trashQuantity;  // breakdown of quantities for each trash type

    // Start is called before the first frame update
    void Awake()
    {
        // init singleton instance
        if (instance != null)
        {
            Debug.Log("Instance of GameManager already exists. Destroying this instance...");
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        // init Dictionary
        trashQuantity = new Dictionary<string, int>();
    }

    void Start()
    {
        // Disable gameplay UI
        for (int i = 0; i < gameplayUI.Length; ++i)
        {
            gameplayUI[i].SetActive(false);
        }
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
        // Start timer
        gameTimer.StartTimer();

        // Enable gameplay UI
        for (int i = 0; i < gameplayUI.Length; ++i)
        {
            gameplayUI[i].SetActive(true);
        }
    }

    /// <summary>
    /// Function to call when player has picked up trash successfully.
    /// </summary>
    public void PickedUpTrash(Trash trash)
    {
        // increment game score
        scoreManager.IncrementScore(trash.GetScore());

        // increment no. trash picked up
        ++numTrashPicked;
        trashCounterText.text = numTrashPicked.ToString();

        // update the trash collected breakdown
        if (!trashQuantity.ContainsKey(trash.GetName())) {
            trashQuantity.Add(trash.GetName(), 0);
        }
        trashQuantity[trash.GetName()] = trashQuantity[trash.GetName()] + 1;

        // trigger check for all trash picked up
        enemyManager.TrashIsRemoved(trash);

        // remove trash
        Destroy(trash.gameObject);
    }

    public int GetTrashQuantity(string name)
    {
        if (!trashQuantity.ContainsKey(name))
            return 0;
        else
            return trashQuantity[name];
    }

    /// <summary>
    /// Function to call to end the game.
    /// </summary>
    public void EndGame()
    {
        // Get time taken for the game
        float timeTaken = gameTimer.GetTimerDuration() - gameTimer.GetTimeRemaining();
        gameTimeTakenSecs = (int)timeTaken;
        // Stop timer
        gameTimer.StopTimer();
        // Enable endgame screen
        endGameScreen.DisplayEndgameScreen();
    }

    public void WinGame()
    {
        // The only possible win condition is that the boss is defeated
        isBossDefeated = true;
        // Increment score
        scoreManager.IncrementScore(bossDefeatedPoints);
        // end the game
        EndGame();
    }

    public int GetTimeTaken()
    {
        return gameTimeTakenSecs;
    }
}
