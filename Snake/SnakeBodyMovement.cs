using UnityEngine;

public class SnakeBodyMovement : MonoBehaviour
{
    [SerializeField] private GameObject snakeChild;
    
    private bool hasChild = false;
    private GameObject child;
    
    public int direction;
    public GameObject parent;
    public bool cantCauseDeath;

    // Function to move the child snake body part with the parent body part
    public void MoveChild(Transform pos, int dir)
    {
        if (hasChild)
        {
            child.GetComponent<SnakeBodyMovement>().MoveChild(transform, direction);
        }

        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        transform.position = new Vector3(pos.position.x, pos.position.y, 0);
        direction = dir;
        BodyRotation();
    }

    // Funtion to make the snake longer by spawning a new body part
    public void MakeChild()
    {
        if (!hasChild)
        {
            child = Instantiate(snakeChild, this.transform.position, Quaternion.identity);
            child.GetComponent<SnakeBodyMovement>().parent = gameObject;
            child.GetComponent<SnakeBodyMovement>().cantCauseDeath = false;
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

    // Calls Game Over if the snake head touches the body part
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "SnakeHead" && !cantCauseDeath)
        {
            Stats.Instance.StatsGameOver();
        }
    }

    // Rotates the body part depending on the given direction. Direction is passed down from the head to the last part via the MoveCild() funciton
    public void BodyRotation()
    {
        switch (direction)
        {
            case 1:
                transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 2:
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case 3:
                transform.eulerAngles = Vector3.zero;
                break;
            case 4:
                transform.eulerAngles = new Vector3(0, 0, 180);
                break;
        }
    }
}
