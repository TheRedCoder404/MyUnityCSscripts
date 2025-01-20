using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance;

    public HighScoreDisplay[] highScoreDisplayArray;
    public List<HighScoreEntry> scores = new List<HighScoreEntry>();

    private int maxLoadScores = 10;

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("ScoreLenght") != 0) { Load(); }
    }

    private void Start()
    {
        UpdateDisplay();
    }

    // Sort the Highscore board as well as providing the Textboxes with the right Text
    public void UpdateDisplay()
    {
        scores.Sort((HighScoreEntry x,  HighScoreEntry y) => y.score.CompareTo(x.score));

        for (int i = 0; i < highScoreDisplayArray.Length; i++)  // Looping through every Textbox (Name and Score) Array and proving the text for each
        {
            if (i < scores.Count)
            {
                highScoreDisplayArray[i].DisplayHighScore(scores[i].name, scores[i].score);
            }
            else
            {
                highScoreDisplayArray[i].HideEntryDisplay();
            }
        }
    }

    // A function to easily add a new score to the list
    public void AddNewScore(string entryName, int entryScore)
    {
        scores.Add(new HighScoreEntry { name = entryName, score = entryScore });
        UpdateDisplay();
    }

    // A function to save the current Leaderboard
    public void Save()
    {
        PlayerPrefs.SetInt("ScoreLenght", scores.Count);

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetString("ScoreName" + i, scores[i].name);
            PlayerPrefs.SetInt("Score" + i, scores[i].score);
        }
    }

    // A function to load the currenly saved Leaderboard
    public void Load()
    {
        List<HighScoreEntry> TempList = new List<HighScoreEntry>();
        int scoresAmount;
        if (PlayerPrefs.GetInt("ScoreLenght") > maxLoadScores) { scoresAmount = maxLoadScores; } else { scoresAmount = PlayerPrefs.GetInt("ScoreLenght"); }

        for (int i = 0; i < scoresAmount; i++)
        {
            TempList.Add(new HighScoreEntry { name = PlayerPrefs.GetString("ScoreName" + i), score = PlayerPrefs.GetInt("Score" + i) });
        }

        scores = TempList;
    }

    // A funtion to clear everything in the Playerprefs because the Leaderboard is the only thing I currently save in the Playerprefs
    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        scores.Clear();
        PlayerPrefs.SetInt("ScoreLenght", 0);
        UpdateDisplay();
    }
}


// A class to easly save the Score entries
public class HighScoreEntry
{
    public string name;
    public int score;
}