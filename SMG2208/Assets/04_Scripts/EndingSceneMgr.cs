using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneMgr : MonoBehaviour
{
    public GameObject scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        Text scoreBoardText = scoreBoard.GetComponentInChildren<Text>();
        int totalScore = GameMgr.In.GetTotalScore();
        scoreBoardText.text = "Á¡¼ö : " + totalScore.ToString();
    }
}
