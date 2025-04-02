using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int speed = 5;
    float rotationInRadians;
    float angleX;
    float angleY;
    [SerializeField]
    float timeToLive = 0.75f;
    public string shooterId;
    [SerializeField]
    private Color color;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        rotationInRadians = (transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
        angleX = Mathf.Cos(rotationInRadians);
        angleY = Mathf.Sin(rotationInRadians);
    }
    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        transform.position += new Vector3(angleX, angleY) * speed * Time.deltaTime;
        if (timeToLive < 0)
        {
            Destroy(gameObject);
        }
    }
    public void Init(string shooterId, Color color)
    {
        this.shooterId = shooterId;
        this.color = color;
        spriteRenderer.color = this.color;
    }
}
