using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        // sort Score Tiers in ascending order as there is no guarantee that they are
        SortScoreTiers();

        // subscribe to scoreChanged event
        scoreManager.scoreChanged += UpdateScoreText;
        scoreManager.scoreChanged += UpdateScoreTier;
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

        // Find current score tier
        int i;
        for (i = 0; i < scoreTiers.Length; ++i)
        {
            if (score < scoreTiers[i].score)
                break;
        }

        // Update score display background image
        scoreTierImage.sprite = scoreTiers[i].backgroundImageSprite;
    }

    private void UpdateScoreText(ScoreChangedEventArgs args)
    {
        scoreText.text = args.score.ToString();
    }
}
