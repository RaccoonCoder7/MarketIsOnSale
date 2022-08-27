using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public List<RectTransform> targetRectTrList = new List<RectTransform>();
    public float scrollSpeed;

    void Update()
    {
        if (GameMgr.In.gameState != GameMgr.GameState.Play) return;

        foreach (var rect in targetRectTrList)
        {
            var pos = rect.localPosition;
            if (pos.x <= -Screen.width)
            {
                pos.x += Screen.width * targetRectTrList.Count;
            }
            pos.x -= Time.deltaTime * scrollSpeed * 100;
            rect.localPosition = pos;
        }
    }
}
