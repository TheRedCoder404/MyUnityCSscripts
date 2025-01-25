using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    public static Stats Instance;

    [SerializeField] public float pipeSpeed;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;

    public int score;
    public int highscore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        score = -1;
        highscore = PlayerPrefs.GetInt("Highscore");
        highScoreText.text = "Highscore: " + highscore.ToString();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        SceneManager.LoadScene("SampleScene");
    }

    public void AddScore()
    {
        score++;
    }
}
