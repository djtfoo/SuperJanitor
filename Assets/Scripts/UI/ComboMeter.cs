using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Kill streak meter display
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
    private TextMeshProUGUI comboXLabel;
    [SerializeField]
    [Tooltip("Image fill amount that will change over time")]
    private float fillStartAmount = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        // subscribe to scoreChanged event
        scoreManager.killStreakChanged += UpdateComboText;
    }

    // Update is called once per frame
    void Update()
    {
        // update fill amount
        meter.fillAmount = fillStartAmount * timer.GetTimeRemainingRatio();
    }

    private void UpdateComboText(int args)
    {
        if (args == 0)
        {
            comboText.text = "";
            comboXLabel.gameObject.SetActive(false);
        }
        else
        {
            comboText.text = args.ToString();
            comboXLabel.gameObject.SetActive(true);
        }
    }
}
