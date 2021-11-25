using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Components on this object")]
    [SerializeField]
    private Animator animationController;
    [SerializeField]
    private PolygonCollider2D polygonCollider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("Trash throwing parameters")]
    [SerializeField]
    private float trajectoryDuration = 2f;
    [SerializeField]
    private Transform throwStartPosition;

    [Header("Walking parameters")]
    [SerializeField]
    private GameWorld gameWorld;
    [SerializeField]
    private float walkDuration = 3f;
    [SerializeField]
    private int numTrashToPoopPerWalk = 4;
    [SerializeField]
    private Transform poopStartPosition;
    [SerializeField]
    private int maxHitsAllowed = 3;

    private bool firstWave = true;
    private bool firstWaveCompleted = false;
    private bool vulnerableToHits = false;

    private int hitsTaken = 0;
    private float timePeriodPoop = 0f;

    // reference to enemy manager
    private EnemyManager enemyManager = null;

    void Awake()
    {
        // calculate period between poops
        timePeriodPoop = walkDuration / (float)numTrashToPoopPerWalk;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (animationController == null)
            animationController = this.GetComponent<Animator>();

        if (polygonCollider == null)
            polygonCollider = GetComponent<PolygonCollider2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        enemyManager = (EnemyManager)FindObjectOfType(typeof(EnemyManager));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (vulnerableToHits) {
            // Update Polygon Collider for raycasting
            Sprite sprite = spriteRenderer.sprite;
            for (int i = 0; i < polygonCollider.pathCount; i++) polygonCollider.pathCount = 0;
            polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

            List<Vector2> path = new List<Vector2>();
            for (int i = 0; i < polygonCollider.pathCount; i++)
            {
                path.Clear();
                sprite.GetPhysicsShape(i, path);
                polygonCollider.SetPath(i, path.ToArray());
            }
        }
    }

    public void PlayThrowAnim()
    {
        animationController.SetTrigger("Throw");
    }

    /// <summary>
    /// Can be called when the boss leaves invul. state, i.e. when all of the current trash has been picked up.
    /// </summary>
    public void SetVulnerableToHits()
    {
        // Check if first wave of trash has been picked up yet
        if (!firstWaveCompleted)
        {
            // Enable the Boss Enemy
            firstWaveCompleted = true;
            gameObject.SetActive(true);
        }
        
        // Set to be vulnerable to hits
        vulnerableToHits = true;
    }

    /// <summary>
    /// To be called when hit.
    /// </summary>
    public void GetHit()
    {
        // Set to invulnerable
        vulnerableToHits = false;
        // Increase no. hits taken
        ++hitsTaken;

        // Check if enemy has been hit enough times
        if (hitsTaken == maxHitsAllowed)
        {
            // Win the game!!!
            GameManager.Instance.WinGame();
        }
        else
        {
            // Play Hurt animation
            animationController.SetTrigger("Hurt");
        }
    }
    
    /// <summary>
    /// After Hurt animation completes, set waypoint to walk to
    /// </summary>
    public void AfterHurtAnimation()
    {
        // Reset animation trigger
        animationController.ResetTrigger("Hurt");

        // Get next destination waypoint
        Vector3 waypoint = gameWorld.BossWaypoints[hitsTaken-1].position;

        // Walk to destination and poop trash along the way
        StartCoroutine(WalkToDestination(waypoint));
    }

    private IEnumerator WalkToDestination(Vector3 dest)
    {
        float timer = 0f;
        int numPooped = 0;  // no. trash pooped out so far
        Vector3 startPos = transform.position;
        // Walk
        while (timer < walkDuration)   // have not reached destination
        {
            // Update position
            transform.position = Vector3.Lerp(startPos, dest, timer / walkDuration);

            // Update timer
            timer += Time.deltaTime;

            // Poop trash along the way
            if ((int)(timer / timePeriodPoop) > numPooped)
            {
                ++numPooped;
                Transform trash = enemyManager.SpawnTrash();
                trash.GetComponent<Trash>().EnableBonusScore();
                trash.GetComponent<Trash>().FireProjectile(poopStartPosition.position, trajectoryDuration);
            }

            yield return null;
        }

        // Finish walking
        transform.position = dest;
        animationController.SetTrigger("WalkComplete"); // return to idle animation
        animationController.ResetTrigger("WalkComplete");
    }

    /// <summary>
    /// Function to call to spawn a wave of trash from the throwing animation.
    /// </summary>
    public void ThrowTrashWave()
    {
        // spawn initial wave of trash
        List<Transform> trashList = enemyManager.SpawnTrashWave();

        // throw trash to its landing positions
        StartCoroutine(TrashTrajectory(trashList));
    }

    private IEnumerator TrashTrajectory(List<Transform> trashList)
    {
        // Throw each Trash object
        foreach (Transform trash in trashList)
        {
            trash.GetComponent<Trash>().FireProjectile(throwStartPosition.position, trajectoryDuration);
        }

        // Update time to start the game
        float timer = 0f;
        while (timer < trajectoryDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Temporarily "despawn" enemy
        gameObject.SetActive(false);
        animationController.ResetTrigger("Throw");

        // If this is the first wave, game has not started
        if (firstWave)
        {
            // Actually start the game only now
            GameManager gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
            gameManager.StartGame();

            firstWave = false;
        }
    }
}
