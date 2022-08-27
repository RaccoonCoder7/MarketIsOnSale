using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;
    [HideInInspector]
    public PatternSpawner spawner;

    private float totalLifeTime;

    /// </summary>
    private void Start()
    {
        StartCoroutine(DestroyBulb(lifeTime));
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

    private IEnumerator DestroyBulb(float lifeTime)
    {
        while (totalLifeTime < lifeTime)
        {
            yield return null;
        }

        spawner.bulbList.Remove(this.transform);
        Destroy(gameObject);
    }
}
