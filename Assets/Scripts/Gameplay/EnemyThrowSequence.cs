using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowSequence : MonoBehaviour
{
    [SerializeField]
    private float trajectoryDuration = 2f;
    [SerializeField]
    private Transform throwStartPosition;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTrash()
    {
        EnemyManager enemyManager = (EnemyManager)FindObjectOfType(typeof(EnemyManager));

        // spawn trash
        List<Transform> trashList = enemyManager.SpawnTrash();

        // throw trash to its landing positions
        StartCoroutine(TrashTrajectory(trashList));
    }

    private IEnumerator TrashTrajectory(List<Transform> trashList)
    {
        // Initialize
        timer = 0f;
        foreach (Transform trash in trashList)
        {
            Transform sprite = trash.GetComponent<Trash>().GetSpriteTransform();
            sprite.position = throwStartPosition.position;
        }

        while (timer < trajectoryDuration)
        {
            // Update time
            timer += Time.deltaTime;

            // Interpolate positions
            foreach (Transform trash in trashList)
            {
                Transform sprite = trash.GetComponent<Trash>().GetSpriteTransform();
                Vector3 newPos = new Vector3();
                newPos.x = Mathf.Lerp(throwStartPosition.position.x, trash.position.x, timer / trajectoryDuration);
                newPos.y = Mathf.Lerp(throwStartPosition.position.y, trash.position.y, timer / trajectoryDuration);
                newPos.z = Mathf.Lerp(throwStartPosition.position.z, trash.position.z, timer / trajectoryDuration);
                sprite.position = newPos;
            }

            yield return null;
        }

        // "Despawn" enemy
        gameObject.SetActive(false);
        // Actually start the game only now
        GameManager gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
        gameManager.StartGame();
    }
}
