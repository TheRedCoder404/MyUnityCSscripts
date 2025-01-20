using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;

    [Header("Game Over Screen")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text HighScoreText;

    [Header("Misc")]
    [SerializeField] private TMP_Text mscScoreTxt;

    private void Start()
    {
        instance = this;
    }

    // Is called when the game is lost, saves new Highscore and displays current score and highscore
    public void Gameover()
    {
        if (Stats.Instance.score > PlayerPrefs.GetInt("1000101")) { PlayerPrefs.SetInt("1000101", Stats.Instance.score); }
        mscScoreTxt.gameObject.SetActive(false);
        scoreText.text = "Score: " + Stats.Instance.score.ToString();
        HighScoreText.text = "Highscore: " + PlayerPrefs.GetInt("1000101").ToString();
    }

    // Exit Button
    public void Exit()
    {
        Application.Quit();
    }

    // Try Again Button
    public void TryAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
