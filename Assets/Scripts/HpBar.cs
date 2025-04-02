using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Image hpBar;
    PlayerInfo playerInfo;
    public GameObject ownerGameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpBar = transform.GetComponent<Image>();
        playerInfo = ownerGameObject.transform.GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        SetSize((float)playerInfo.hp / playerInfo.maxHp);
    }
    public void SetSize(float ratio)
    {
        hpBar.fillAmount = ratio;
    }

}
