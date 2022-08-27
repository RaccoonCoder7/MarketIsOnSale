using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public GameObject menuSet;
    public GameObject howToPlayWindow;
    public GameObject timeBoard;
    public GameObject scoreBoard;
    public GameObject countdown;

    private float count;
    private Text timeBoardText;
    private Text scoreBoardText;
    private Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        GameMgr.In.gameState = GameMgr.GameState.None;
        menuSet.SetActive(false);
        howToPlayWindow.SetActive(true);
        timeBoard.SetActive(false);
        scoreBoard.SetActive(false);
        countdown.SetActive(false);
        timeBoardText = timeBoard.GetComponentInChildren<Text>();
        scoreBoardText = scoreBoard.GetComponentInChildren<Text>();
        countdownText = countdown.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeBoardText.text = "남은 시간 : " + ((int)GameMgr.In.gameLimitTime - (int)GameMgr.In.totalTime);
        scoreBoardText.text = "점수 : " + GameMgr.In.GetTotalScore();
        switch (GameMgr.In.gameState)
        {
            case GameMgr.GameState.None:
                // Countdown
                if (countdown.activeSelf)
                {
                    count -= Time.deltaTime;
                    countdownText.text = ((int)count + 1).ToString();
                    if(count <= 0.0f)
                    {
                        GameMgr.In.gameState = GameMgr.GameState.Play;
                        countdown.SetActive(false);
                    }
                }
                break;
            case GameMgr.GameState.Pause:
                // Press escape key to resume
                if (Input.GetButtonDown("Cancel"))
                {
                    GameResume();
                }
                break;
            case GameMgr.GameState.Play:
                // Press escape key to pause
                if (Input.GetButtonDown("Cancel"))
                {
                    GamePause();
                }
                break;
        }
    }

    // Start the game
    public void GameStart()
    {
        howToPlayWindow.SetActive(false);
        timeBoard.SetActive(true);
        scoreBoard.SetActive(true);
        countdown.SetActive(true);
        count = 3.0f;
    }

    // Pause the game
    public void GamePause()
    {
        GameMgr.In.gameState = GameMgr.GameState.Pause;
        menuSet.SetActive(true);
    }

    // Resume the game
    public void GameResume()
    {
        GameMgr.In.gameState = GameMgr.GameState.Play;
        menuSet.SetActive(false);
    }

    // Quit the game and back to main menu
    public void GameExit()
    {
        GameMgr.In.ResetGame();
    }
}
