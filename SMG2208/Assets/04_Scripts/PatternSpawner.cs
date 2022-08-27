using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    public List<SpawnObject> spawnPrefabList = new List<SpawnObject>();
    public TextAsset patternText;
    public float spawnTerm = 2.395f;
    public List<Transform> bulbTrList = new List<Transform>();
    [HideInInspector]
    public List<Transform> spawnObjList = new List<Transform>();

    private float spawnTime;
    private int textCnt;
    private string trimText;

    void Start()
    {
        trimText = patternText.text.Trim();
        spawnTime = spawnTerm;

        // TODO: 오브젝트풀링
        PlayerMgr.In.bulbTrList = this.bulbTrList;
        StartCoroutine(StartSpawn());
    }

    private int GetPatternNumber()
    {
        int num = 0;
        System.Int32.TryParse(trimText[textCnt].ToString(), out num);
        textCnt++;
        return num;
    }

    private SpawnObject Spawn(int patternNum)
    {
        var obj = Instantiate(spawnPrefabList[patternNum], this.transform);
        obj.spawner = this;
        return obj;
    }

    private IEnumerator StartSpawn()
    {
        while (true)
        {
            while (GameMgr.In.gameState != GameMgr.GameState.Play)
            {
                yield return null;
            }

            spawnTime += Time.deltaTime;
            if (spawnTime >= spawnTerm)
            {
                int patternNum = GetPatternNumber();
                if (patternNum != 0 && patternNum < spawnPrefabList.Count)
                {
                    var obj = Spawn(patternNum);
                    spawnObjList.Add(obj.transform);
                }
                var bulb = Spawn(0);
                bulbTrList.Add(bulb.transform);
                spawnTime -= spawnTerm;

                if (textCnt >= trimText.Length)
                {
                    Debug.Log("종료");
                    yield break;
                }
            }
            yield return null;
        }
    }
}
