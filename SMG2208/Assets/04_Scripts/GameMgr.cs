using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : SingletonMono<GameMgr>
{
    public int score;
    public float totalTime;
    public GameState gameState;
    public int scorePerSecond;
    public int scorePerItem;
    public int gameLimitTime;
    public int totalItemCnt;

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

    public void AddScore(int value)
    {
        score += value;
    }

    public void AddItem(int value)
    {
        totalItemCnt += value;
    }

    public void ResetGame()
    {
        gameState = GameState.None;
        score = 0;
        totalTime = 0;
        totalItemCnt = 0;
        tempTime = 0;
    }

    public int GetTotalScore()
    {
        return (totalItemCnt * scorePerItem) + (gameLimitTime * scorePerSecond);
    }
}
