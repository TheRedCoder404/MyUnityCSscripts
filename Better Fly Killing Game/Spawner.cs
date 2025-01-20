using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    public GameObject enemy;
    [SerializeField] 
    private string entityName;
    [SerializeField]
    private float maxSpawn;
    [SerializeField]
    private float startTime;
    [SerializeField]
    private float maxConcurrent;
    [SerializeField]
    private float spawnAcceleration;
    [SerializeField]
    private float maxSpawnSpeed;
    
    public int curSpawn;

    private float spawnSpeed;
    private float timeUntilNextSpawn;

    Vector2 spawnPos;
    Vector2 screenSize;

    void Awake()
    {
        timeUntilNextSpawn = startTime;
        spawnSpeed = startTime;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        DynamicSpawnCap();
    }

    private void DynamicSpawnCap()
    {
        switch (Stats.Instance.killed[entityName])
        {
            case 1:
                maxConcurrent = 10;
                break;

            case 20:
                maxConcurrent = 12;
                break;

            case 30:
                maxConcurrent = 15;
                break;

            case 40:
                maxConcurrent = 18;
                break;

            case 50:
                maxConcurrent = 25;
                break;

            case 60:
                maxConcurrent = 30;
                break;
        }
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeUntilNextSpawn);

        if (maxSpawn > curSpawn && Stats.Instance.movementAllowed && Stats.Instance.alive[entityName] < maxConcurrent)
        {
            spawnPos = new Vector2(Random.Range(screenSize.x * -1f, screenSize.x), Random.Range(screenSize.y * -1f, screenSize.y));
            Instantiate(enemy, spawnPos, Quaternion.identity);
            curSpawn += 1;

            setNewSpawnTime();
        }

        StartCoroutine(Spawn());
    }

    void setNewSpawnTime()
    {
        spawnSpeed = Mathf.Clamp((spawnSpeed * (1 / spawnAcceleration)), (1 / maxSpawnSpeed), startTime);
        
        timeUntilNextSpawn = spawnSpeed;
    }
}
