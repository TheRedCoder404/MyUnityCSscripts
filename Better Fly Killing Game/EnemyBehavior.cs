using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
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
    [SerializeField]
    private float scorePossibility;

    public bool touchingRacket;

    private float realSpeed;

    private Camera cam;

    Vector2 waypoint;
    

    void Start()
    {
        cam = Camera.main;
        SetNewDestination(); // gets the first destination
        gameObject.transform.localScale = new Vector3(1 * size, 1 * size, 1 * size); // sets the scale of the object for easy callibration
    }


    void Update()
    {
        if (Stats.Instance.movementAllowed) { realSpeed = speed; } else { realSpeed = 0; }

        scorePossibility = Mathf.Clamp(scorePossibility - Time.deltaTime, 1, scorePossibility);

        MoveToWaypoint();
        CheckForBorder();
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

    private void CheckForBorder()
    {
        // Reverses the direction if the Enemy hits the screen
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

    // Getting a new destination
    void SetNewDestination()
    {
        waypoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
        if ((transform.position.x - waypoint.x) < 0) 
        { 
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent <SpriteRenderer>().flipX = false;
        }
    }

    // Function for doing everything that needs to be dong at the moment of death
    private IEnumerator Death()
    {
        gameObject.GetComponent<Animator>().Play("SplatEnemy");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Stats.Instance.enemysKilled += 1;
        Stats.Instance.score += Mathf.FloorToInt(scorePossibility) * Stats.Instance.scoreMultiplier;
    }

    // Checks if the enemy is touching the racket and executes it if it does
    public void IsTouchingRacket()
    {
        if (touchingRacket) { StartCoroutine(Death()); }
    }

    // Saves that enemy is currently touching the racket
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Racket") { touchingRacket = true; }
    }

    // saves that the enemy is not touching the racket anymore
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Racket") { touchingRacket = false; }
    }
}
