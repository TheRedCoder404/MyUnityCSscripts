using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PipeSpawner PipeSpawner;

    private float rotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.gravityScale = 0;
        rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rb.gravityScale == 0) { rb.gravityScale = 2; PipeSpawner.StartSpawner(); }
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpForce));
            rotation = rotationForce;
        }

        float rotAmount = rotation * Time.deltaTime;
        float curRot = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot - rotAmount));

        if (rotation > 0) { rotation -= Time.deltaTime * rotation; }
    }
}
