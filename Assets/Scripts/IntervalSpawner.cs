using UnityEngine;

public class IntervalSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 2f; // Time between spawns

    private Camera mainCamera;
    private float timer;

    void Start()
    {
        mainCamera = Camera.main;
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnObject();
            timer = spawnInterval;
        }
    }

    void SpawnObject()
    {
        if (objectToSpawn == null || mainCamera == null)
            return;

        // Get screen bounds
        Vector2 spawnPosition = GetRandomScreenPosition();

        // Convert to world position
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(spawnPosition.x, spawnPosition.y, mainCamera.nearClipPlane + 1));

        Instantiate(objectToSpawn, worldPosition, Quaternion.identity);
    }

    Vector2 GetRandomScreenPosition()
    {
        float x = Random.Range(0, Screen.width);
        float y = Random.Range(0, Screen.height);
        return new Vector2(x, y);
    }
}
