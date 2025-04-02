using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    float fireRate = 0.25f;
    [SerializeField]
    float fireRateTimer;
    [SerializeField]
    string shooterId;
    [SerializeField]
    Color color;
    bool canFireGun;
    Bullet boule;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireRateTimer = fireRate;
        shooterId = transform.parent.GetComponent<PlayerInfo>().id;
        color = transform.parent.GetComponent<PlayerInfo>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRateTimer >= fireRate)
        {
            fireRateTimer = fireRate;
        }
        fireRateTimer += Time.deltaTime;
        if (canFireGun)
        {
            GameObject thing = Instantiate(bullet, transform.position, transform.rotation);
            if (thing != null)
            {
                boule = thing.GetComponent<Bullet>();
                boule.Init(shooterId, color);
                Debug.Log(shooterId);
                Debug.Log(color);
            }
            else
            {
                Debug.Log("its empty");
            }
            fireRateTimer = 0;
            canFireGun = false;
        }
    }
    public void Shoot() 
    {
        if (fireRateTimer >= fireRate)
        {
            canFireGun = true;
        }
    }
}