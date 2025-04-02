using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CycleFonts : MonoBehaviour
{
    public List<TMP_FontAsset> fonts;
    TextMeshProUGUI tmp;
    float timer = 0.2f;
    public float time;
    TextMeshProUGUI otherText;
    // Start is called before the first frame update
    void Start()
    {
        tmp = gameObject.GetComponent<TextMeshProUGUI>();
        time = timer;
        otherText = transform.parent.Find("Text").GetComponent<TextMeshProUGUI>();
        tmp.text = otherText.text;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            tmp.font = fonts[Random.Range(0, fonts.Count)];
            time = timer;
        }
    }
}
