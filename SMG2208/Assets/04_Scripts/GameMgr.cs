using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : SingletonMono<GameMgr>
{
    public int score;
    public float currTime;
    public GameState gameState;

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
                break;
            case GameState.Play:
                break;
            case GameState.Pause:
                break;
        }
    }

    public void AddScore(int value)
    {
        score += value;
    }

    public void ClearScore()
    {
        score = 0;
    }

    public void ResetTime()
    {
        currTime = 0;
    }

    public void ResetGame()
    {
        gameState = GameState.None;
        ClearScore();
        ResetTime();
    }
}
