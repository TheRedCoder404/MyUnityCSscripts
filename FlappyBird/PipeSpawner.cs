using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] float startSpawningAfter;
    [SerializeField] float spawnEvery;
    [SerializeField] float pipeDistance;
    [SerializeField] float minDistanceFormLast;
    [SerializeField] float maxDistanceFormLast;

    [Header("Pipes")]
    [SerializeField] GameObject UpperPipe;
    [SerializeField] GameObject LowerPipe;
    [SerializeField] GameObject ScoreBox;

    private float screenSize;

    private Vector2 lastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
    }

    public void StartSpawner()
    {
        InvokeRepeating("SpawnPipe", startSpawningAfter, spawnEvery);
    }

    void SpawnPipe()
    {
        Vector3 spawnPos;
        spawnPos = RandomizeNextSpawn();
        Instantiate(UpperPipe, new Vector3(13, spawnPos.x, 0), Quaternion.identity);
        Instantiate(LowerPipe, new Vector3(13, spawnPos.y, 0), Quaternion.identity);
        Instantiate(ScoreBox, new Vector3(13, spawnPos.z, 0), Quaternion.identity);
    }

    Vector3 RandomizeNextSpawn()
    {
        Vector3 spawnPos = new Vector3();
        float randPos;

        randPos = Random.Range(-screenSize * 0.8f, screenSize * 0.8f);
        spawnPos.x = randPos + (pipeDistance / 2);
        spawnPos.y = randPos - (pipeDistance / 2);
        spawnPos.z = randPos;

        return spawnPos;
    }
}
