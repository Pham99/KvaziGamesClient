using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string username;
    public string id;
    public Color color;
    public int hp = 5;
    public int maxHp = 5;
    public LayerMask targetLayer;

    ParticleSystem boostParticle;
    ParticleSystem shootParticle;
    // Start is called before the first frame update
    void Awake()
    {
    }
    private void Start()
    {
        boostParticle = transform.Find("BoosterParticle").GetComponent<ParticleSystem>();
        var boostParticleMain = boostParticle.main;
        boostParticleMain.startColor = color;
        shootParticle = transform.Find("ShootParticle").GetComponent<ParticleSystem>();
        var shootParticleMain = shootParticle.main;
        shootParticleMain.startColor = color;
    }
    public void Init(string id, Color color, string name)
    {
        this.id = id;
        this.color = color;
        username = name;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && !(collision.gameObject.GetComponent<Bullet>().shooterId == id))
        {
            hp -= 1;
            Destroy(collision.gameObject);
            Debug.Log("");
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
