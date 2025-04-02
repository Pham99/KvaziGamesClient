using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Image hpBar;
    private int maxHp = 10;
    private int hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }
    public void SetSize(float ratio)
    {
        hpBar.fillAmount = ratio;
    }
    public void GetAttacked()
    {
        if (hp > 0)
        {
            hp--;
            SetSize((float)hp / maxHp);
            float test = (float)hp / maxHp;
            Debug.Log(test);
        }
        else { Debug.Log("its dead"); }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
