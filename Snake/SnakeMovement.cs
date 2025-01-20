using System.Collections;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private Transform movePoint;
    [SerializeField] private float stepTime;
    [SerializeField] private GameObject snakeChild;
    [SerializeField] private GameObject StatsGameObject;

    private int direction = 0;
    private bool hasChild = false;
    private GameObject child;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePoint.parent = null;

        StartCoroutine(moveSnake());
    }

    // Update is called once per frame. Checks for the desired direction
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0f && direction != 2)
        {
            direction = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0f && direction != 1) 
        {
            direction = 2;
        }

        if (Input.GetAxisRaw("Vertical") > 0f && direction != 4)
        {
            direction = 3;
        }
        else if (Input.GetAxis("Vertical") < 0f && direction != 3)
        {
            direction = 4;
        }
    }

    // A function to move the snake once after a specified time
    private IEnumerator moveSnake()
    {
        SpeedControl();
        switch (direction)
        {
            case 1:
                movePoint.position = transform.position + new Vector3(1f, 0f, 0f);
                break;
            case 2:
                movePoint.position = transform.position + new Vector3(-1f, 0f, 0f);
                break;
            case 3:
                movePoint.position = transform.position + new Vector3(0f, 1f, 0f);
                break;
            case 4:
                movePoint.position = transform.position + new Vector3(0f, -1f, 0f);
                break;
        }

        if (movePoint.position.x > Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x && movePoint.position.y > Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y && movePoint.position.x < Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x && movePoint.position.y < Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y) 
        {
            if (hasChild)
            {
                child.GetComponent<SnakeBodyMovement>().MoveChild(transform, direction);
            }
            transform.position = new Vector3(movePoint.position.x, movePoint.position.y, 0);
        }
        else // Game over when the snake travels off the screen
        {
            StatsGameObject.GetComponent<Stats>().StatsGameOver();
        }

        yield return new WaitForSeconds(stepTime);

        if (!StatsGameObject.GetComponent<Stats>().gameOver)
        {
            StartCoroutine(moveSnake());
        }
    }

    // A function to create a new child bodypart, but without having it as an actual child, so that it can move independent
    public void MakeChild()
    {
        if (!hasChild)
        {
            child = Instantiate(snakeChild, this.transform.position, Quaternion.identity);
            child.GetComponent<SnakeBodyMovement>().parent = gameObject;
            child.GetComponent<SnakeBodyMovement>().cantCauseDeath = true;
            child.GetComponent<SnakeBodyMovement>().direction = direction;
            child.GetComponent<SnakeBodyMovement>().BodyRotation();
            child.transform.parent = null;
            hasChild = true;
        }
        else
        {
            child.GetComponent<SnakeBodyMovement>().MakeChild();
        }
    }

    // A function to variate the speed of the snake depending on the current score
    private void SpeedControl()
    {
        switch (StatsGameObject.GetComponent<Stats>().score)
        {
            case < 20:
                stepTime = 1f - ((0.5f / 20f) * StatsGameObject.GetComponent<Stats>().score);
                break;

            case <= 40:
                stepTime = 0.5f - ((0.375f / 20) * (StatsGameObject.GetComponent<Stats>().score - 20));
                break;

            case > 40:
                stepTime = 0.125f;
                break;
        }
    }
}
