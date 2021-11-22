using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Timer gameTimer;

    // Update is called once per frame
    void Update()
    {
        timerText.text = gameTimer.GetTimeRemainingStr();
    }
}
