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
    public string EndingSceneName;

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
                totalTime += Time.deltaTime;
                if (totalTime >= gameLimitTime)
                {
                    gameState = GameState.None;
                    SceneMgr.In.ChangeScene(EndingSceneName);
                    AudioMgr.In.StopPlay();
                }
                break;
        }
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
        AudioMgr.In.StopPlay();
    }

    // Calculating the total score
    public int GetTotalScore()
    {
        return (totalItemCnt * scorePerItem) + ((int)totalTime * scorePerSecond);
    }
}
