using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionMenu;
    public TMP_InputField nameInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        nameInput.text = PlayerPrefs.GetString("NewPlayer");
    }

    public void PlayButton()
    {
        PlayerPrefs.SetString("NewPlayer", nameInput.text);
        SceneManager.LoadScene("Level");
    }

    public void OptionButton()
    {
        OptionMenu.SetActive(!OptionMenu.activeSelf);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
