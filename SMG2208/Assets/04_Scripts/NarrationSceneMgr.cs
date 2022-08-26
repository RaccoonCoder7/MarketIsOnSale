using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NarrationSceneMgr : MonoBehaviour
{
    public Text narrationText;
    public TextAsset narrationTextAsset;
    public float textDelayTime;
    public string gameSceneName;
    private bool isTextFlowing;
    private bool skipLine;
    private List<string> lines = new List<string>();
    private int lineCnt = -1;
    private string prevNarrationText;

    void Start()
    {
        lines = narrationTextAsset.text.Split('\n').ToList();
        StartCoroutine(StartNarration());
        StartNextNarration();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (isTextFlowing)
            {
                SkipCurrNarration();
                StartNextNarration();
                return;
            }

            if (lines.Count <= lineCnt + 1)
            {
                MoveToGameScene();
                return;
            }

            StartNextNarration();
        }
    }

    public void MoveToGameScene()
    {
        SceneMgr.In.ChangeScene(gameSceneName);
    }

    private void SkipCurrNarration()
    {
        skipLine = true;
    }

    private void StartNextNarration()
    {
        lineCnt++;
    }

    private IEnumerator StartNarration()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Trim().Equals("/"))
            {
                prevNarrationText = string.Empty;
                lineCnt++;
                continue;
            }

            if (!string.IsNullOrEmpty(prevNarrationText))
            {
                prevNarrationText = prevNarrationText + "\n";
            }

            isTextFlowing = true;
            for (int j = 0; j < lines[i].Length; j++)
            {
                narrationText.text = prevNarrationText + lines[i].Substring(0, j + 1);
                yield return new WaitForSeconds(textDelayTime);

                if (skipLine)
                {
                    skipLine = false;
                    if (i + 1 >= lines.Count || lines[i + 1].Trim().Equals("/"))
                    {
                        lineCnt--;
                    }
                    break;
                }
            }
            prevNarrationText = prevNarrationText + lines[i];
            narrationText.text = prevNarrationText;
            isTextFlowing = false;

            while (i >= lineCnt)
            {
                yield return new WaitForSeconds(textDelayTime);
            }
        }
    }
}
