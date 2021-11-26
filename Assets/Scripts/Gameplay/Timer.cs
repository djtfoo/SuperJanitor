using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    // Time, in seconds
    [SerializeField]
    [Tooltip("Duration of timer, in seconds")]
    private float timerDuration = 60.0f;
    private float timeRemaining = 0f;

    [SerializeField]
    private bool autoStart = false;
    private bool timerRunning = false;

    // Unity Event when time is up
    [SerializeField]
    private UnityEvent onTimerUp;

    // Start is called before the first frame update
    void Awake()
    {
        timeRemaining = timerDuration;
    }

    void Start()
    {
        if (autoStart)
            StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f; // should not have negative time
                timerRunning = false;   // timer has completed
                // trigger on timer up event
                if (onTimerUp != null)
                    onTimerUp.Invoke();
            }
        }
    }

    /// <summary>
    /// Start the timer.
    /// </summary>
    public void StartTimer()
    {
        timeRemaining = timerDuration;
        timerRunning = true;
    }

    /// <summary>
    /// Stop the timer from running and keep its previous time.
    /// </summary>
    public void PauseTimer()
    {
        timerRunning = false;
    }

    /// <summary>
    /// Stop the timer and set the time to 0.
    /// </summary>
    public void StopTimer()
    {
        timeRemaining = 0f;
        timerRunning = false;
    }
    
    /// <summary>
    /// Set a new timer duration, while specifying if the timer should be started/reset immediately.
    /// </summary>
    /// <param name="newDuration">The new duration for the timer</param>
    /// <param name="startTimer">Whether to start/reset the timer from the new duration</param>
    public void SetTimerDuration(float newDuration, bool startTimer)
    {
        timerDuration = newDuration;
        if (startTimer)
            StartTimer();
    }

    /// <summary>
    /// Get the time remaining on the timer, expressed as a ratio of the total timer duration.
    /// </summary>
    /// <returns>The time remaining in seconds expressed as a ratio</returns>
    public float GetTimeRemainingRatio()
    {
        return timeRemaining / timerDuration;
    }

    /// <summary>
    /// Get the time remaining on the timer.
    /// </summary>
    /// <returns>The time remaining in seconds (float)</returns>
    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    /// <summary>
    /// Get time remaining in a minutes, seconds and 2 d.p. format. (TBD)
    /// </summary>
    /// <returns>Time remaining in formatted string</returns>
    public string GetTimeRemainingStr()
    {
        /// TODO: determine format
        return ((int)timeRemaining).ToString();
    }

    public float GetTimerDuration()
    {
        return timerDuration;
    }
}
