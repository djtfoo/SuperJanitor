using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TotalScoreItem
{
    public string name;
    public TextMeshProUGUI quantityText;
}

public class EndGameScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject endgameCanvas;
    [Header("Parameters")]
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private TotalScoreItem[] scoreItems;

    [Header("Overall score display")]
    [SerializeField]
    private TextMeshProUGUI totalScoreText;
    [SerializeField]
    private GameObject[] starsEarned;

    [Header("Score display for boss kill")]
    [SerializeField]
    private TextMeshProUGUI bossQuantityText;

    [Header("TEMP: Indicate win or lost")]
    [SerializeField]
    private GameObject winText;
    [SerializeField]
    private GameObject loseText;

    // Start is called before the first frame update
    void Start()
    {
        endgameCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayEndgameScreen()
    {
        endgameCanvas.SetActive(true);

        UpdateEndgameScreen();
    }

    private void UpdateEndgameScreen()
    {
        // TEMP: indicate win or lost
        winText.SetActive(gameManager.IsBossDefeated);
        loseText.SetActive(!gameManager.IsBossDefeated);

        // Update game stats breakdown for trash items picked up
        for (int i = 0; i < scoreItems.Length; ++i)
        {
            scoreItems[i].quantityText.text = gameManager.GetTrashQuantity(scoreItems[i].name).ToString();
        }

        // Update game stats breakdown for boss
        if (gameManager.IsBossDefeated)
            bossQuantityText.text = "1";
        else
            bossQuantityText.text = "0";

        // Update total score
        totalScoreText.text = scoreManager.GetScore().ToString();

        // TODO: Update no. stars earned display

    }
}
