using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    [SerializeField] GameObject Apple;

    public static AppleSpawner Instance;

    private void Start()
    {
        Instance = this;
        SpawnApple();
    }

    // Spawn Apple at random location
    public void SpawnApple()
    {
        if (Stats.Instance.applesOnScreen < Stats.Instance.appleLimit) 
        {
            float locationY = (int)Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float locationX = (int)Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            if (locationY < Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y) { locationY += 0.5f; } else { locationY -= 0.5f; }
            if (locationX < Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) { locationX += 0.5f; } else { locationX -= 0.5f; }

            Vector2 spawnLocation = new Vector2(locationX, locationY);
            Instantiate(Apple, spawnLocation, Quaternion.identity);
            Stats.Instance.applesOnScreen++;
        }
    }
}
