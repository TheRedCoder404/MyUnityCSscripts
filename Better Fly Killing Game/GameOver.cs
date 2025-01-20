using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private float fadeinTime;
    [SerializeField] 
    private Leaderboard leaderboard;

    // If called, starts the GameOver procedure
    public void CallGameOver()
    {
        gameObject.SetActive(true);
        leaderboard.AddNewScore(PlayerPrefs.GetString("NewPlayer"), Stats.Instance.score);
        Stats.Instance.movementAllowed = false;
        leaderboard.UpdateDisplay();
        Cursor.visible = true;
        leaderboard.Save();
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
