using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;
    public int damage;
    [HideInInspector]
    public PatternSpawner spawner;

    private float totalLifeTime;

    /// </summary>
    private void Start()
    {
        StartCoroutine(DestroyByLifetime(lifeTime));
    }

    void Update()
    {
        if (GameMgr.In.gameState != GameMgr.GameState.Play)
        {
            return;
        }

        totalLifeTime += Time.deltaTime;
        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed, Space.World);
    }

    public void DestroyObject()
    {
        if (spawner.bulbTrList.Contains(transform))
        {
            spawner.bulbTrList.Remove(transform);
        }

        if (spawner.spawnObjList.Contains(transform))
        {
            spawner.spawnObjList.Remove(transform);
        }

        Destroy(gameObject);
    }

    private IEnumerator DestroyByLifetime(float lifeTime)
    {
        while (totalLifeTime < lifeTime)
        {
            yield return null;
        }

        DestroyObject();
    }
}
