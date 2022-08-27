using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    public Bulb spawnObj;
    public List<Transform> bulbList = new List<Transform>();

    private int testNum = 0;

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
            var bulb = Instantiate(spawnObj, this.transform);
            bulb.spawner = this;
            bulb.name = testNum++.ToString();
            bulbList.Add(bulb.transform);
            yield return new WaitForSeconds(1f);
        }
    }
}
