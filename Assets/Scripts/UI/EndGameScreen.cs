using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TotalScoreItem
{
    public string name;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI scoreText;
}


public class EndGameScreen : MonoBehaviour
{
    [SerializeField]
    private TotalScoreItem[] scoreItems;

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
}
