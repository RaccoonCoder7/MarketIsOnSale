using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;
    [HideInInspector]
    public PatternSpawner spawner;

    /// </summary>
    private void Start()
    {
        StartCoroutine(DestroyBulb(lifeTime));
    }

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed, Space.World);
    }

    private IEnumerator DestroyBulb(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        spawner.bulbList.Remove(this.transform);
        Destroy(gameObject, lifeTime);
    }
}
