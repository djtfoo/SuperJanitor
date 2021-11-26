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
    [SerializeField]
    private ParticleSystem particleSystem;

    [Header("Walking parameters")]
    [SerializeField]
    private GameWorld gameWorld;
    [SerializeField]
    private float walkDuration = 3f;
    [SerializeField]
    private int numTrashToPoopPerWalk = 4;
    [SerializeField]
    private Transform poopStartPosition;

    [Header("Other parameters")]
    [SerializeField]
    private int maxHitsAllowed = 3;
    public int MaxHitsAllowed
    {
        get { return maxHitsAllowed; }
    }
    [SerializeField]
    [Tooltip("Y-coordinate for standing sprites' height")]
    private float standYCoord = 0.357f;
    [SerializeField]
    [Tooltip("Y-coordinate for walking sprites' height")]
    private float walkYCoord = 0.12f;

    private bool firstWave = true;
    private bool firstWaveCompleted = false;
    public bool FirstWaveCompleted
    {
        get { return firstWaveCompleted; }
    }
    private bool vulnerableToHits = false;

    private int hitsTaken = 0;
    public int HitsTaken
    {
        get { return hitsTaken; }
    }
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
        // after completing first wave, throw trash and set them to be golden
        if (!firstWaveCompleted)
        {
            // Enable boss
            gameObject.SetActive(true);
            particleSystem.Play();
            PlayThrowAnim();
            firstWaveCompleted = true;
        }

        // Set to be vulnerable to hits
        vulnerableToHits = true;
    }

    /// <summary>
    /// To be called when hit.
    /// </summary>
    public void GetHit()
    {
        // check if enemy is vulnerable
        if (!vulnerableToHits)
            return;

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
    /// (Called via Animation Event)
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
        animationController.ResetTrigger("WalkComplete");

        // set walking height
        dest.y = walkYCoord;
        transform.position = new Vector3(transform.position.x, walkYCoord, transform.position.z);

        float timer = 0f;
        int numPooped = 0;  // no. trash pooped out so far
        Vector3 startPos = transform.position;

        // Flip walk animation depending on direction
        float xDir = dest.x - startPos.x;
        float initialXScale = transform.localScale.x;
        if (xDir < 0f)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

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

        // set standing height
        dest.y = standYCoord;
        // Finish walking
        transform.position = dest;
        animationController.SetTrigger("WalkComplete"); // return to idle animation
        // reset scale
        transform.localScale = new Vector3(initialXScale, transform.localScale.y, transform.localScale.z);

        // reset trigger after awhile
        yield return new WaitForSeconds(1f);
        animationController.ResetTrigger("WalkComplete");
        yield return null;
    }

    /// <summary>
    /// Function to call to spawn a wave of trash from the throwing animation.
    /// </summary>
    public void ThrowTrashWave()
    {
        // spawn initial wave of trash
        List<Transform> trashList = enemyManager.SpawnTrashWave();

        // for first wave, it is NOT bonus
        if (!firstWave)
        {
            // set trash to be bonus (golden shadow)
            foreach (Transform trash in trashList)
            {
                trash.GetComponent<Trash>().EnableBonusScore();
            }
        }

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

        animationController.ResetTrigger("Throw");

        // If this is the first wave, game has not started
        if (firstWave)
        {
            // Temporarily "despawn" enemy
            gameObject.SetActive(false);
            particleSystem.Play();
            // Actually start the game only now
            GameManager gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
            gameManager.StartGame();

            firstWave = false;
        }
    }
}
