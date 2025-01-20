using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float speed;

    private float upSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        gameObject.GetComponent<Rigidbody2D>().AddRelativeForceY(jumpForce);
    }
}
