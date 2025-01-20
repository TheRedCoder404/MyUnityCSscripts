using System;
using System.Linq;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats Instance;
    public bool playerTurn = false;
    public bool isPlayable;

    public int[] pressedByP1 = { };
    public int[] pressedByP2 = { };

    private int[] test = { 2, 4, 5, 6};

    private int[,] winCons = new int[8,3] 
    { 
        {1, 2, 3},
        {4, 5, 6}, 
        {7, 8, 9},
        {1, 5, 9},
        {3, 5, 7},
        {1, 4, 7},
        {2, 5, 8},
        {3, 6, 9}
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        isPlayable = true;
    }

    public void CheckWinCons()
    {

        if ((playerTurn && pressedByP2.Length > 2) || (!playerTurn && pressedByP1.Length > 2))
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (playerTurn)
                    {
                        if (pressedByP2.Contains(winCons[i, j]) && j == 2)
                        {
                            Player2Win();
                            break;
                        }
                        else if (!pressedByP2.Contains(winCons[i, j]))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (pressedByP1.Contains(winCons[i, j]) && j == 2)
                        {
                            Player1Win();
                            break;
                        }
                        else if (!pressedByP1.Contains(winCons[i, j]))
                        {
                            break;
                        }
                    }
                }
            }
        }

        playerTurn = !playerTurn;
    }

    private void Player1Win()
    {
        Debug.Log("Player 1 Wins");
        isPlayable = false;
    }

    private void Player2Win()
    {
        Debug.Log("Player 2 Wins");
        isPlayable = false;
    }
}
