using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    public Bulb spawnObj;
    public List<Transform> bulbList = new List<Transform>();

    private float spawnTime;

    void Start()
    {
        // TODO: 오브젝트풀링
        PlayerMgr.In.bulbTrList = this.bulbList;
        StartCoroutine(StartSpawn());
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
            if (spawnTime >= 1)
            {
                var bulb = Instantiate(spawnObj, this.transform);
                bulb.spawner = this;
                bulbList.Add(bulb.transform);
                spawnTime -= 1;
            }
            yield return null;
        }
    }
}
