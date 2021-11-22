using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboMeter : MonoBehaviour
{
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    [Tooltip("Reference to Timer object")]
    private Timer timer;
    [Header("UI elements")]
    [SerializeField]
    private Image meter;
    [SerializeField]
    private TextMeshProUGUI comboText;
    [SerializeField]
    [Tooltip("Image fill amount that will change over time")]
    private float fillStartAmount = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to scoreChanged event
        scoreManager.comboChanged += UpdateComboText;
    }

    // Update is called once per frame
    void Update()
    {
        // update fill amount
        meter.fillAmount = fillStartAmount * timer.GetTimeRemainingRatio();
    }

    private void UpdateComboText(int args)
    {
        comboText.text = args.ToString() + "x";
    }
}
