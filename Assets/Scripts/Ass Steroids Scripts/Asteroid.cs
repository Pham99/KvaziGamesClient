using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    public float expansionTime = 2f;
    public float speed = 1.5f;
    public float rotationSpeed = 100f;
    public int splitCount = 2;
    public bool doesFadeIn = true;
    public GameObject smallerAsteroidPrefab;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;
    private Rigidbody2D rb;
    public ParticleSystem explosionEffect;
    private Vector2 direction;

    void Start()
    {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Disable collider at the start
        hitbox.enabled = false;

        // Start the spawning effect
        if (doesFadeIn)
        {
            StartCoroutine(SpawnEffect());
        }
        SetRandonVelocityAndRotation();
    }

    IEnumerator SpawnEffect()
    {
        float elapsedTime = 0f;
        Color startColor = Color.gray;
        Color endColor = Color.white;
        Vector3 startScale = transform.localScale * 0.5f;
        Vector3 endScale = transform.localScale;

        transform.localScale = startScale;

        while (elapsedTime < expansionTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / expansionTime;

            // Lerp color and scale
            spriteRenderer.color = Color.Lerp(startColor, endColor, progress);
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);

            yield return null;
        }
    }

    private void SetRandonVelocityAndRotation()
    {
        // Enable collider after transformation is complete
        hitbox.enabled = true;

        // Assign a random direction for movement
        direction = Random.insideUnitCircle.normalized;

        // Apply movement
        rb.linearVelocity = direction * speed;
        rb.angularVelocity = Random.Range(-rotationSpeed, rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            for (int i = 0; i < splitCount; i++)
            {
                Instantiate(smallerAsteroidPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
    private void OnDestroy()
    {
        if (explosionEffect != null)
        {
            ParticleSystem ps = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            DontDestroyOnLoad(ps.gameObject);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration); // Destroy after it finishes
        }
    }
}
