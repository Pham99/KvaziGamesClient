using TMPro;
using UnityEngine;

public class HpBarFactory : MonoBehaviour
{
    public Canvas canvas;
    public GameObject hpBarPrefab;

    public void CreateHPBar(GameObject player)
    {
        GameObject hpBarObj = Instantiate(hpBarPrefab, canvas.transform);

        hpBarObj.transform.localPosition = Vector3.zero;
        hpBarObj.transform.localScale = Vector3.one;
        hpBarObj.GetComponent<NameTag>().ownerGameObject = player;
        hpBarObj.GetComponent<HpBar>().ownerGameObject = player;
    }
}
