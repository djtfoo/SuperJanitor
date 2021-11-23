using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct ScoreTier {
    public int score;
    public Sprite backgroundImageSprite;
}

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private ScoreManager scoreManager;
    [Tooltip("Reference to the score tier Image")]
    [SerializeField]
    private Image scoreTierImage;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private ScoreTier[] scoreTiers;

    // Start is called before the first frame update
    void Awake()
    {
        // sort Score Tiers in ascending order as there is no guarantee that they are
        SortScoreTiers();

        // subscribe to scoreChanged event
        scoreManager.scoreChanged += UpdateScoreText;
        scoreManager.scoreChanged += UpdateScoreTier;

        // load current score tier image
        UpdateScoreTierImage(scoreManager.GetScore());
    }

    private void SortScoreTiers()
    {
        for (int i = 0; i < scoreTiers.Length; ++i)
        {
            for (int j = 0; j < i - 1; ++j)
            {
                if (scoreTiers[j].score > scoreTiers[j + 1].score)
                {
                    ScoreTier temp = scoreTiers[j];
                    scoreTiers[j] = scoreTiers[j + 1];
                    scoreTiers[j + 1] = temp;
                }
            }
        }
    }

    private void UpdateScoreTier(ScoreChangedEventArgs args)
    {
        // Get current score
        int score = args.score;

        // Update score tier image
        UpdateScoreTierImage(score);
    }

    private void UpdateScoreTierImage(int newScore)
    {
        // Find current score tier
        int i;
        for (i = 0; i < scoreTiers.Length; ++i)
        {
            if (newScore < scoreTiers[i].score)
                break;
        }
        // take the image for the previous score tier
        i = Math.Max(0, i - 1);

        // Update score display background image
        scoreTierImage.sprite = scoreTiers[i].backgroundImageSprite;
    }

    private void UpdateScoreText(ScoreChangedEventArgs args)
    {
        scoreText.text = args.score.ToString();
    }
}
