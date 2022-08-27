using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneMgr : MonoBehaviour
{
    public string NarrationSceneName;

    public void MoveToNarrationScene()
    {
        SceneMgr.In.ChangeScene(NarrationSceneName);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
