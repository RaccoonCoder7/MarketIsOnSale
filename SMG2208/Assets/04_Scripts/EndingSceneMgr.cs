using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneMgr : MonoBehaviour
{
    public GameObject scoreBoard;
    public string GameSceneName;
    public string StartSceneName;

    // Start is called before the first frame update
    void Start()
    {
        Text scoreBoardText = scoreBoard.GetComponentInChildren<Text>();
        int totalScore = GameMgr.In.GetTotalScore();
        scoreBoardText.text = "Á¡¼ö : " + totalScore.ToString();
    }

    // Restart the Game
    public void GameRestart()
    {
        GameMgr.In.ResetGame();
        SceneMgr.In.ChangeScene(GameSceneName);
    }

    // Quit the game and back to main menu
    public void GameExit()
    {
        GameMgr.In.ResetGame();
        SceneMgr.In.ChangeScene(StartSceneName);
    }
}
