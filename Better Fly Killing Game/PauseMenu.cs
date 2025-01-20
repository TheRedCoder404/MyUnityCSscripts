using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ResumeButton()
    {
        Stats.Instance.movementAllowed = true;
        gameObject.SetActive(false);
        Cursor.visible = false;
    }
    
    public void BackToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
