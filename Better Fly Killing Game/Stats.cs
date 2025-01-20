using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class Stats : MonoBehaviour
{
    public static Stats Instance { get; private set; }

    public GameObject gameOverScreen;
    public GameObject pauseMenu;
    
    public TMP_Text textBox;
    public TMP_Text scoreBox;

    public int enemysKilled;
    public int nonEnemysKilled;
    public int curAliveEnemy;
    public int curAliveNonEnemy;
    public int score;
    public int scoreMultiplier;

    public bool debugText;
    public bool movementAllowed;

    public Dictionary<string, int> alive = new Dictionary<string, int>();
    public Dictionary<string, int> killed = new Dictionary<string, int>();

    string prinText;

    Spawner[] spawners;

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("DebugStats") == 1) { debugText = true; } else { debugText = false; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawners = GameObject.Find("Spawner").GetComponents<Spawner>();  // To get every spawner script easily accessible
        alive["Racket"] = 0;
        killed["Racket"] = 0;
        killed["Enemy"] = 0;
        killed["NonEnemy"] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            movementAllowed = false;
            Cursor.visible = true;
        }
        
        if (debugText)
        {
            statPrinter();
        }
        else
        {
            textBox.text = "";
        }

        UpateAlive();
        UpdateKilled();
        Score();
    }

    void statPrinter()
    {
        prinText = "";  // Clearing the string

        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].enemy.name != "FlyRacket")  // Blacklisting the Racket because its useless information
            {
                prinText += (spawners[i].enemy.name + ": " + spawners[i].curSpawn + "<br>");  // Adding every stat to string with formatation
            }

            switch (spawners[i].enemy.name)
            {
                case "Enemy":
                    prinText += (spawners[i].enemy.name + "s killed: " + enemysKilled + "<br>");
                    break;
                case "NonEnemy":
                    prinText += (spawners[i].enemy.name + "s killed: " + nonEnemysKilled + "<br>");
                    break;
            }
        }

        prinText += ("Enemies currently Alive: " + curAliveEnemy + "<br>");
        prinText += ("NonEnemies currently Alive: " + curAliveNonEnemy + "<br>");

        textBox.text = prinText;  // Printing the stats on the screen
    }

    void UpateAlive() // update the counter of how many enteties are still alive
    {
        curAliveEnemy = spawners[0].curSpawn - enemysKilled;
        curAliveNonEnemy = spawners[1].curSpawn - nonEnemysKilled;
        alive["Enemy"] = curAliveEnemy;
        alive["NonEnemy"] = curAliveNonEnemy;
    }

    private void UpdateKilled()
    {
        killed["Enemy"] = enemysKilled;
        killed["NonEnemy"] = nonEnemysKilled;
    }

    void Score() // calculates the score and prints it
    {
        scoreBox.text = string.Format("{0:000000}", score);
    }

    public void DebugButtonPressed() // toggles the debug stats
    {
        debugText = !debugText;
    }

    public void CallGameOver()
    {
        gameOverScreen.GetComponent<GameOver>().CallGameOver();
    }
}
