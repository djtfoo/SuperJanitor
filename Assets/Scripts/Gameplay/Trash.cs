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
    [Tooltip("Reference to the Trash Sprite object")]
    private SpriteRenderer trashSprite;
    [SerializeField]
    [Tooltip("Reference to the shadow Sprite object")]
    private SpriteRenderer shadowSprite;
    [SerializeField]
    [Tooltip("Legendary trash dropped by enemy")]
    private bool isBonus = false;

    [SerializeField]
    private Sprite bonusShadowSprite;

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
        return isBonus ? 2*score : score;
    }

    public void EnableBonusScore()
    {
        isBonus = true;
        // change the shadow to golden shadow
        shadowSprite.sprite = bonusShadowSprite;
    }

    public void FireProjectile(Vector3 startPos, float duration)
    {
        StartCoroutine(MoveProjectile(startPos, duration));
    }

    private IEnumerator MoveProjectile(Vector3 startPos, float duration)
    {
        float timer = 0f;
        trashSprite.transform.position = startPos;

        while (timer < duration)
        {
            // Update time
            timer += Time.deltaTime;
            if (timer > duration)
                timer = duration;

            // Interpolate Trash position
            trashSprite.transform.position = Vector3.Lerp(startPos, transform.position, timer / duration);
            yield return null;
        }
    }
}
