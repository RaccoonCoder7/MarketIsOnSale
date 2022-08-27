using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneMgr : MonoBehaviour
{
    public GameObject scoreBoard;
    public Image portrait;
    public Text talkerName;
    public Text quote;
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
            portrait.enabled = true;
            talkerName.text = "아주머니";
            quote.text = "그러게 총각 빨리 오지 그랬어~";
        }
        else
        {
            portrait.enabled = false;
            talkerName.text = "피치";
            quote.text = "이 정도면 충분하겠지? 이제 배송을 하러 가볼까?";
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
