using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneMgr : MonoBehaviour
{
    public GameObject scoreBoard;
    public GameObject talkingBox;
    public string GameSceneName;
    public string StartSceneName;

    // Start is called before the first frame update
    void Start()
    {
        Text scoreBoardText = scoreBoard.GetComponentInChildren<Text>();
        int totalScore = GameMgr.In.GetTotalScore();
        scoreBoardText.text = "점수: " + totalScore.ToString();
        if (PlayerMgr.In.hp <= 0)
        {
            talkingBox.SetActive(true);
        }
        else
        {
            talkingBox.SetActive(false);
        }
    }

    // Restart the Game
    public void GameRestart()
    {
        GameMgr.In.ResetGame();
        PlayerMgr.In.ResetPlayer();
        SceneMgr.In.ChangeScene(GameSceneName);
        PlayerMgr.In.InitPlayer();
        AudioMgr.In.Play(0);
    }

    // Quit the game and back to main menu
    public void GameExit()
    {
        GameMgr.In.ResetGame();
        PlayerMgr.In.ResetPlayer();
        SceneMgr.In.ChangeScene(StartSceneName);
    }
}
