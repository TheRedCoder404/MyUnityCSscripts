using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NonEnemyBehavior : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float range;
    [SerializeField]
    float maxDistance;
    [SerializeField]
    float borderDistance;
    [SerializeField]
    float size;

    public GameObject gameOver;

    int xDir;
    int yDir;

    private float realSpeed;

    private Camera cam;

    public bool touchingRacket;

    Vector2 waypoint;
    Vector3 screenBorder;


    void Start()
    {
        cam = Camera.main;
        SetNewDestination(); // gets the first destination
        gameObject.transform.localScale = new Vector3(1 * size, 1 * size, 1 * size); // sets the scale of the object for easy callibration
    }


    void Update()
    {
        if (Stats.Instance.movementAllowed) { realSpeed = speed; } else { realSpeed = 0; }

        MoveToWaypoint();
        CheckForBorder();
    }

    private void CheckForBorder()
    {
        // Reverses the direction if the NonEnemy hits the screen
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        if (screenPos.x < 0 + borderDistance | screenPos.x > Screen.width - borderDistance)
        {
            waypoint.x = waypoint.x * -0.2f;
        }
        if (screenPos.y < 0 + borderDistance | screenPos.y > Screen.height - borderDistance)
        {
            waypoint.y = waypoint.y * -0.2f;
        }
    }

    private void MoveToWaypoint()
    {
        // Going to the newest destination and getting a new one after arrival
        transform.position = Vector2.MoveTowards(transform.position, waypoint, realSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoint) < range)
        {
            SetNewDestination();
        }
    }

    // Getting a new destination
    void SetNewDestination()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());  // gets mouse position
        // Uses Mouse position to stealthly menouver the NonEnemy to the cursor
        if (mousePos.y < transform.position.y)
        {
            yDir = -1;
        }
        else
        {
            yDir = 1;
        }
        if (mousePos.x < transform.position.x)
        {
            xDir = -1;
        }
        else
        {
            xDir = 1;
        }

        waypoint = new Vector2(Random.Range(0, maxDistance * xDir), Random.Range(0, maxDistance * yDir));
    }

    // Function for doing everything that needs to be dong at the moment of death
    public IEnumerator Death()
    {
        gameObject.GetComponent<Animator>().Play("SplatNonEnemy");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        GameObject.Find("StatHandler").GetComponent<Stats>().nonEnemysKilled += 1;
        GameObject.Find("StatHandler").GetComponent<Stats>().CallGameOver();
    }

    // Checks if the nonEnemy is touching the racket and executes it if it does
    public void IsTouchingRacket()
    {
        if (touchingRacket) { StartCoroutine(Death()); }
    }

    // Saves that nonEnemy is currently touching the racket
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Racket") { touchingRacket = true; }
    }

    // saves that the nonEnemy is not touching the racket anymore
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Racket") { touchingRacket = false; }
    }
}
