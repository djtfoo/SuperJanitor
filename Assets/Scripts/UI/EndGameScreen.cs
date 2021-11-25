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
    public TextMeshProUGUI bossScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEndgameScreen()
    {
        // Update game stats breakdown for trash items picked up
        for (int i = 0; i < scoreItems.Length; ++i)
        {
            scoreItems[i].quantityText.text = gameManager.GetTrashQuantity(scoreItems[i].name).ToString();
        }

        // Update game stats breakdown for boss

        // Update total score
        totalScoreText.text = scoreManager.GetScore().ToString();
        // Update no. stars earned display

    }
}
