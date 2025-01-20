using UnityEngine;

public class AppleDestroy : MonoBehaviour
{
    // If a Trigger is registered this function will spawn a new apple, destroy this one, add a point as well as making the snake longer
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Stats.Instance.AddPoint();
        GameObject.Find("Snake").GetComponent<SnakeMovement>().MakeChild();
        Stats.Instance.applesOnScreen--;
        Stats.Instance.AppleLimitControl();
        AppleSpawner.Instance.SpawnApple();
        Destroy(gameObject);
    }
}
