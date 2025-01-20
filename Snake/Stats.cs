using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private GameObject GameOverScreen; 

    public static Stats Instance;

    public int score = 0;
    public int appleLimit = 1;
    public int applesOnScreen = 0;

    public bool gameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    // Updates the score text
    void UpdateScore()
    {
        ScoreText.text = "Score: " + score.ToString();
    }

    // A function to add a specific amount of point if called
    public void AddPoint()
    {
        score++;
    }

    // A function to easily call the Gameover() funtion from the GameOver script
    public void StatsGameOver()
    {
        GameOverScreen.SetActive(true);
        gameOver = true;
        GameOverScreen.GetComponent<GameOver>().Gameover();
    }

    // A funtion to vary to amount of apples on the screen at the same time to make it more difficult a bit faster
    public void AppleLimitControl()
    {
        switch (score)
        {
            case 10:
                appleLimit = 2;
                AppleSpawner.Instance.SpawnApple();
                break;
            
            case 30:
                appleLimit = 3;
                AppleSpawner.Instance.SpawnApple();
                break;
            
            case 50:
                appleLimit = 4;
                AppleSpawner.Instance.SpawnApple();
                break;
        }
    }
}
