using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Name of this trash (needs to match GameManager)")]
    private string name;
    [SerializeField]
    [Tooltip("Score that this trash gives when picked up")]
    private int score;
    [SerializeField]
    [Tooltip("Trash sprite")]
    private SpriteRenderer trashSprite;
    [SerializeField]
    [Tooltip("Legendary trash dropped by enemy")]
    private bool isGoldShadow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return name;
    }
    public int GetScore()
    {
        return score;
    }

    public Transform GetSpriteTransform()
    {
        return trashSprite.transform;
    }
}
