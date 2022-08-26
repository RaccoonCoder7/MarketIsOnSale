using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyToPlay : MonoBehaviour
{
    public float setTime = 3.0f;
    public GameObject howToPlayWindow;
    public GameObject remainTime;
    public GameObject score;
    public GameObject countdown;
    public Text countdownText;

    // When close window
    public void CloseWindow()
    {
        howToPlayWindow.SetActive(false);
        remainTime.SetActive(true);
        score.SetActive(true);
        countdown.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        remainTime.SetActive(false);
        score.SetActive(false);
        countdown.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown
        if (countdown.activeSelf)
        {
            setTime -= Time.deltaTime;
            countdownText.text = ((int)setTime + 1).ToString();
            if (setTime <= 0.0f)
            {
                countdown.SetActive(false);
            }
        }
    }
}
