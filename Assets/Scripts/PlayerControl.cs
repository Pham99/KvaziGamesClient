using Mygame.NetInput;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour   
{
    Rigidbody2D rb;
    [SerializeField]
    string id;
    InputMap map;
    Gun gun;
    public UnityEvent testEvent;

    [SerializeField]
    float rotationInRadians;
    // Start is called before the first frame update
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    float rotationSpeed = 1;
    [SerializeField]
    float speedMultiplier = 1;
    [SerializeField]
    float maxVelocity = 1;
    [SerializeField]
    Vector2 velocity;

    ParticleSystem boosterParticles;
    ParticleSystem shootParticles;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        boosterParticles = transform.Find("BoosterParticle").GetComponent<ParticleSystem>();
        shootParticles = transform.Find("ShootParticle").GetComponent<ParticleSystem>();
        id = transform.GetComponent<PlayerInfo>().id;
        gun = transform.Find("Gun").GetComponent<Gun>();
        map = NetInput.GetInputMap(id);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || map.GetKey(NetKeyCode.Up))
        {
            rotationInRadians = (transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
            rb.AddForce(new Vector2(Mathf.Cos(rotationInRadians) * speedMultiplier * Time.deltaTime, Mathf.Sin(rotationInRadians) * speedMultiplier * Time.deltaTime));
            if(!boosterParticles.isEmitting)
            {
                boosterParticles.Play();
            }
        }
        else
        {
            if(boosterParticles.isEmitting)
            {
                boosterParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
        if (Input.GetKey(KeyCode.A) || map.GetKeyDown(NetKeyCode.Left))
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || map.GetKeyDown(NetKeyCode.Right))
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) || map.GetKeyDown(NetKeyCode.A))
        {
            gun.Shoot();
            shootParticles.Play();
        }

        // Speed dampening
        velocity = rb.linearVelocity;
        if (rb.linearVelocity.x > maxVelocity)
        {
            rb.linearVelocity = new Vector2(maxVelocity, rb.linearVelocity.y);
        }
        if (rb.linearVelocity.y > maxVelocity)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxVelocity);
        }
        if (rb.linearVelocity.x < -maxVelocity)
        {
            rb.linearVelocity = new Vector2(-maxVelocity, rb.linearVelocity.y);
        }
        if (rb.linearVelocity.y < -maxVelocity)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxVelocity);
        }
    }
}
