using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    float movementSpeed;

    private void Start()
    {
        movementSpeed = Stats.Instance.pipeSpeed; // Gets the movementspeed of the pipes
    }

    void Update()
    {
        if (transform.position.x > -13) // Constantly moves the pipe to the left
        {
            gameObject.transform.position = new Vector3(transform.position.x - movementSpeed * Time.deltaTime, transform.position.y, 0);

        }
        else // Destroys the Pipe if it is out of view
        {
            Destroy(gameObject);
        }
    }
}
