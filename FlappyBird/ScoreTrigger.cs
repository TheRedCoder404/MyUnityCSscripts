using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Stats.Instance.AddScore();
        Destroy(gameObject);
    }
}
