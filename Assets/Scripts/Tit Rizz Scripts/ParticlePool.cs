using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public GameObject particlePrefab;
    public int poolSize = 32;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(particlePrefab, Vector3.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetParticle(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(particlePrefab, Vector3.zero, Quaternion.identity, transform);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        var ps = obj.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
        return obj;
    }

    public void ReturnParticle(GameObject obj, float delay = 1.5f)
    {
        StartCoroutine(ReturnAfterDelay(obj, delay));
    }

    private System.Collections.IEnumerator ReturnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
