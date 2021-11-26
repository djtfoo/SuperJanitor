using System;
using UnityEngine;

public struct ScoreChangedEventArgs
{
    public int score;
};

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int killStreak = 0;

    [SerializeField]
    private Timer killStreakTimer;
    [SerializeField]
    private float killStreakTimerMaxDuration = 7f;
    [SerializeField]
    private float killStreakTimerMinDuration = 4f;
    [SerializeField]
    private float killStreakTimerDurationPerKill = -0.1f;

    // delegate for callback when time is up
    public event Action<ScoreChangedEventArgs> scoreChanged;
    public event Action<int> killStreakChanged;

    private int highestCombo = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set combo to default (timer for meter will be disabled)
        SetKillStreak(0);
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
        float multiplier = 1f +         // base multiplier is 1x
            Mathf.Min(2f, (float)killStreak * 0.1f) +   // increase multiplier by 0.1x for every kill (caps at 20)
            ((int)Mathf.Max(0f, killStreak-20) / 5) * 0.5f;     // after 20 kills, every 5 kills will increase multiplier by 0.5x
        score += (int)Mathf.Round(pointsGained * multiplier);

        // update combo multiplier and timer
        SetKillStreak(killStreak + 1);

        // trigger score updated event
        ScoreChangedEventArgs args;
        args.score = score;
        scoreChanged.Invoke(args);
    }

    /// <summary>
    /// To be called only if incrementing the kill streak (combo) without wanting to update the score.
    /// </summary>
    /// <param name="increment"></param>
    public void IncrementKillStreak(int increment)
    {
        SetKillStreak(killStreak + increment);
    }

    private void SetKillStreak(int newStreak)
    {
        // Set multiplier amount
        killStreak = newStreak;
        // Update highest combo
        if (newStreak > highestCombo)
            highestCombo = newStreak;

        // Set timer
        if (newStreak == 0) // combo is reset
            // Disable timer
            killStreakTimer.StopTimer();
        else
        {
            // calculate new combo timer
            float newTimer = Math.Max(killStreakTimerMinDuration, killStreakTimerMaxDuration + (killStreak - 1) * killStreakTimerDurationPerKill);
            killStreakTimer.SetTimerDuration(newTimer, true);
        }
        // trigger combo updated event
        killStreakChanged.Invoke(killStreak);
    }

    public void ResetKillStreak()
    {
        SetKillStreak(0);
    }

    public int GetHighestCombo()
    {
        return highestCombo;
    }
}
