using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : SingletonMono<GameMgr>
{
    public int score = 0;
    public float totalTime = 0;
    public GameState gameState = GameState.None;
    public int scorePerSecond = 100;
    public int scorePerItem = 500;
    public int gameLimitTime = 120;
    public int totalItemCnt = 0;

    private float tempTime;

    public enum GameState
    {
        None,
        Play,
        Pause
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.None:
            case GameState.Pause:
                break;
            case GameState.Play:
                // Update the time and score
                tempTime += Time.deltaTime;
                totalTime += Time.deltaTime;
                if (tempTime >= 1)
                {
                    AddScore(scorePerSecond);
                    tempTime -= 1;
                }
                if (totalTime >= gameLimitTime)
                {
                    gameState = GameState.None;
                }
                break;
        }
    }

    // Adding the score
    public void AddScore(int value)
    {
        score += value;
    }

    // Adding the item amount
    public void AddItem(int value)
    {
        totalItemCnt += value;
    }

    // Reset the game
    public void ResetGame()
    {
        gameState = GameState.None;
        score = 0;
        totalTime = 0;
        totalItemCnt = 0;
        tempTime = 0;
    }

    // Calculating the total score
    public int GetTotalScore()
    {
        return (totalItemCnt * scorePerItem) + ((int)totalTime * scorePerSecond);
    }
}
