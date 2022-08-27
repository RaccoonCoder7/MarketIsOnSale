using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    public List<SpawnObject> objPrefabList = new List<SpawnObject>();
    public List<SpawnObject> itemPrefabList = new List<SpawnObject>();
    public TextAsset objText;
    public TextAsset itemText;
    public float objSpawnTerm = 1.2f;
    public float itemSpawnTerm = 0.6f;
    [HideInInspector]
    public List<Transform> bulbTrList = new List<Transform>();
    // public List<Transform> spawnObjList = new List<Transform>();
    // [HideInInspector]
    // public List<Transform> spawnItemList = new List<Transform>();

    private float objSpawnTime;
    private float itemSpawnTime;
    private int spawnTextCnt;
    private string spawnTrimText;
    private int itemTextCnt;
    private string itemTrimText;

    void Start()
    {
        spawnTrimText = objText.text.Trim();
        objSpawnTime = objSpawnTerm;
        itemTrimText = itemText.text.Trim();
        itemSpawnTime = itemSpawnTerm;

        PlayerMgr.In.bulbTrList = this.bulbTrList;
        StartCoroutine(StartObjSpawn());
        StartCoroutine(StartItemSpawn());
    }

    private int GetPatternNumber()
    {
        int num = 0;
        System.Int32.TryParse(spawnTrimText[spawnTextCnt].ToString(), out num);
        spawnTextCnt++;
        return num;
    }

    private int GetItemPatternNumber()
    {
        int num = 0;
        System.Int32.TryParse(itemTrimText[itemTextCnt].ToString(), out num);
        itemTextCnt++;
        return num;
    }

    private SpawnObject Spawn(int patternNum)
    {
        var obj = Instantiate(objPrefabList[patternNum], this.transform);
        obj.spawner = this;
        return obj;
    }

    private void ItemSpawn(int patternNum)
    {
        Instantiate(itemPrefabList[patternNum], this.transform);
    }

    private IEnumerator StartObjSpawn()
    {
        while (true)
        {
            while (GameMgr.In.gameState != GameMgr.GameState.Play)
            {
                yield return null;
            }

            objSpawnTime += Time.deltaTime;
            if (objSpawnTime >= objSpawnTerm)
            {
                int patternNum = GetPatternNumber();
                if (patternNum != 0 && patternNum < objPrefabList.Count)
                {
                    Spawn(patternNum);
                }
                var bulb = Spawn(0);
                bulbTrList.Add(bulb.transform);
                objSpawnTime -= objSpawnTerm;

                if (spawnTextCnt >= spawnTrimText.Length)
                {
                    yield break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator StartItemSpawn()
    {
        while (true)
        {
            while (GameMgr.In.gameState != GameMgr.GameState.Play)
            {
                yield return null;
            }

            itemSpawnTime += Time.deltaTime;
            if (itemSpawnTime >= itemSpawnTerm)
            {
                int patternNum = GetItemPatternNumber();
                if (patternNum != 0 && patternNum < itemPrefabList.Count)
                {
                    ItemSpawn(patternNum);
                }
                itemSpawnTime -= itemSpawnTerm;

                if (itemTextCnt >= itemTrimText.Length)
                {
                    yield break;
                }
            }
            yield return null;
        }
    }
}
