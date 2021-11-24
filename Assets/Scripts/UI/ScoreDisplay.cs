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
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [Header("Background image for score tiers")]
    [Tooltip("Reference to the score tier Image")]
    [SerializeField]
    private Image scoreTierImage;
    [SerializeField]
    private Image scoreTierImageMaxTier;    // for blending
    [SerializeField]
    private ScoreTier[] scoreTiers;
    [SerializeField]
    private float startPosY;
    [SerializeField]
    private float endPosY;

    private bool maxTier = false;

    // Start is called before the first frame update
    void Awake()
    {
        // sort Score Tiers in ascending order as there is no guarantee that they are
        SortScoreTiers();

        // subscribe to scoreChanged event
        scoreManager.scoreChanged += UpdateScore;
        scoreManager.scoreChanged += UpdateScoreTier;

        // load current score tier image
        UpdateScoreTierImage(scoreManager.GetScore());

        // disable scoreTierImageMaxTier first
        scoreTierImageMaxTier.color = new Color(scoreTierImageMaxTier.color.r, scoreTierImageMaxTier.color.g, scoreTierImageMaxTier.color.b, 0);
    }

    private float blendAlpha = 1f;
    private float speed = 2f;
    private float dir = 1f;
    void Update()
    {
        if (maxTier)
        {
            blendAlpha += dir * speed * Time.deltaTime;
            if (dir > 0f && blendAlpha >= 1f)
            {
                dir = -1f;
                blendAlpha = 1f;
            }
            else if (dir < 0f && blendAlpha <= 0f)
            {
                dir = 1f;
                blendAlpha = 0f;
            }
            //scoreTierImage.color = new Color(scoreTierImage.color.r, scoreTierImage.color.g, scoreTierImage.color.b, blendAlpha);
            scoreTierImageMaxTier.color = new Color(scoreTierImageMaxTier.color.r, scoreTierImageMaxTier.color.g, scoreTierImageMaxTier.color.b, 1 - blendAlpha);
        }
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

        // if max tier, blend the images
        if (i == scoreTiers.Length)
        {
            maxTier = true;
        }
        else
        {
            // take the image for the previous score tier
            i = Math.Max(0, i - 1);

            // Update score display background image
            scoreTierImage.sprite = scoreTiers[i].backgroundImageSprite;
        }
    }

    private void UpdateScore(ScoreChangedEventArgs args)
    {
        // Update text
        scoreText.text = args.score.ToString();

        // Update image position
        float yPos = Math.Min(endPosY, startPosY + ((float)args.score / (scoreTiers[scoreTiers.Length-1].score - scoreTiers[0].score) * (endPosY - startPosY)));
        scoreTierImage.transform.localPosition = new Vector3(scoreTierImage.transform.localPosition.x, yPos, scoreTierImage.transform.localPosition.z);
    }
}
