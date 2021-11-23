using System;
using UnityEngine;

public struct ScoreChangedEventArgs
{
    public int score;
};

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int comboMultiplier = 1;

    [SerializeField]
    private Timer comboTimer;
    [SerializeField]
    private float comboTimerDuration = 5f;
    [SerializeField]
    private float comboTimerDurationPerMultiplier = -0.1f;

    // delegate for callback when time is up
    public event Action<ScoreChangedEventArgs> scoreChanged;
    public event Action<int> comboChanged;

    // Start is called before the first frame update
    void Start()
    {
        // Set combo to default (timer for meter will be disabled)
        SetComboMultiplier(1);
        // Set score to default
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public void IncrementScore(int pointsGained)
    {
        // update score
        score += pointsGained * comboMultiplier;

        // update combo multiplier and timer
        SetComboMultiplier(comboMultiplier + 1);   /// TODO: +=1 or *=2?

        // trigger score updated event
        ScoreChangedEventArgs args;
        args.score = score;
        scoreChanged.Invoke(args);
    }

    private void SetComboMultiplier(int newMultiplier)
    {
        // Set multiplier amount
        comboMultiplier = newMultiplier;
        // Set timer
        if (newMultiplier == 1) // combo is reset
            // Disable timer
            comboTimer.SetTimerDuration(comboTimerDuration, false);
        else
        {
            // calculate new combo timer
            float newTimer = Math.Max(1f, comboTimerDuration + (comboMultiplier - 1) * comboTimerDurationPerMultiplier);
            comboTimer.SetTimerDuration(newTimer, true);
        }
        // trigger combo updated event
        comboChanged.Invoke(comboMultiplier);
    }

    public void ResetComboMultiplier()
    {
        SetComboMultiplier(1);
    }
}
