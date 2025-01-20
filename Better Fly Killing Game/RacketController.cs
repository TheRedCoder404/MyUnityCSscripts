using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RacketController : MonoBehaviour
{
    [SerializeField]
    private float swingTime;
    [SerializeField]
    private float followSpeed;

    private bool swingTimer;
    
    Vector2 mousePos;

    GameObject[] enemys;
    GameObject[] nonEnemys;

    private void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("StatHandler").GetComponent<Stats>().movementAllowed)
        {
            SyncToCursor();

            // Check for leftclick so that the swing routine gets started
            if (Mouse.current.leftButton.wasPressedThisFrame && !swingTimer)
            {
                StartCoroutine(Swing());
                swingTimer = true;
            }
        }        
    }

    private void SyncToCursor()
    {
        // Moves the Racket hitbox with the Mouse
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mousePos;
    }

    // Swing routine
    private IEnumerator Swing()
    {
        gameObject.GetComponentInChildren<Animator>().Play("Swing");
        yield return new WaitForSeconds(swingTime * 1.1f); // waits until the swing is complete
        swingTimer = false;
        DelHitObjects();
    }

    // Says to every GameObject with the right Tag to look id they are possibly hit
    void DelHitObjects()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        nonEnemys = GameObject.FindGameObjectsWithTag("NonEnemy");

        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<EnemyBehavior>().IsTouchingRacket();
        }

        foreach (GameObject nonEnemy in nonEnemys)
        {
            nonEnemy.GetComponent<NonEnemyBehavior>().IsTouchingRacket();
        }
    }
}
