using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : SingletonMono<GameMgr>
{
    public int score;
    public float totalTime;
    public GameState gameState;
    public int scorePerSecond;
    public int scorePerItem;
    public int gameLimitTime = 120;
    public int totalItemCnt;
    public GameObject menuSet;
    public GameObject howToPlayWindow;
    public GameObject timeBoard;
    public GameObject scoreBoard;
    public GameObject countdown;

    private float tempTime;
    private float count;

    public enum GameState
    {
        None,
        Play,
        Pause
    }

    void Start()
    {
        gameState = GameState.None;
        howToPlayWindow.SetActive(true);
        timeBoard.SetActive(false);
        scoreBoard.SetActive(false);
        countdown.SetActive(false);
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.Pause:
                // Press escape key to resume
                if (Input.GetButtonDown("Cancel"))
                {
                    GameResume();
                }
                break;
            case GameState.Play:
                // Countdown
                if (countdown.activeSelf)
                {
                    Text countdownText = countdown.GetComponentInChildren<Text>();
                    count -= Time.deltaTime;
                    countdownText.text = ((int)count + 1).ToString();
                    if (count <= 0.0f)
                    {
                        countdown.SetActive(false);
                    }
                    break;
                }

                // Press escape key to pause
                if (Input.GetButtonDown("Cancel"))
                {
                    GamePause();
                    break;
                }

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

    // Adding score
    public void AddScore(int value)
    {
        score += value;
    }

    // Adding item amount
    public void AddItem(int value)
    {
        totalItemCnt += value;
    }

    // Reset game
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
        return (totalItemCnt * scorePerItem) + (gameLimitTime * scorePerSecond);
    }

    // Start the game
    public void GameStart()
    {
        gameState = GameState.Play;
        howToPlayWindow.SetActive(false);
        timeBoard.SetActive(true);
        scoreBoard.SetActive(true);
        countdown.SetActive(true);
        count = 3.0f;
    }

    // Pause the game
    public void GamePause()
    {
        gameState = GameState.Pause;
        menuSet.SetActive(true);
    }

    // Resume the game
    public void GameResume()
    {
        gameState = GameState.Play;
        menuSet.SetActive(false);
    }

    // Quit the game and back to main menu
    public void GameExit()
    {
        ResetGame();
    }
}
