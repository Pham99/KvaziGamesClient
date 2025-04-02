using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    [SerializeField]
    public Color spriteColour;
    SpriteRenderer r1;
    SpriteRenderer r2;
    SpriteRenderer r3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteColour = transform.parent.GetComponent<PlayerInfo>().color;
        r1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
        r2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
        r3 = transform.GetChild(2).GetComponent<SpriteRenderer>();
        r1.color = spriteColour;
        r2.color = spriteColour;
        r3.color = spriteColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
