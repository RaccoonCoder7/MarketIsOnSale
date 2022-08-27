using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public List<RectTransform> targetRectTrList = new List<RectTransform>();
    public float scrollSpeed;

    private float screenWidth;

    void Start()
    {
        screenWidth = GetComponent<CanvasScaler>().referenceResolution.x;
        for (int i = 1; i < targetRectTrList.Count; i++)
        {
            var pos = targetRectTrList[i].localPosition;
            pos.x = screenWidth * i;
            targetRectTrList[i].localPosition = pos;
        }
    }

    void Update()
    {
        if (GameMgr.In.gameState != GameMgr.GameState.Play) return;

        foreach (var rect in targetRectTrList)
        {
            var pos = rect.localPosition;
            if (pos.x <= -screenWidth)
            {
                pos.x += screenWidth * targetRectTrList.Count;
            }
            pos.x -= Time.deltaTime * scrollSpeed * 100;
            rect.localPosition = pos;
        }
    }
}
