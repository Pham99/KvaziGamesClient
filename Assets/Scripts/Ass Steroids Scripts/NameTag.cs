using TMPro;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    [SerializeField]
    public GameObject ownerGameObject;
    [SerializeField]
    private Vector3 positionOffset;
    void Awake()
    {
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( ownerGameObject != null)
        {
            transform.position = ownerGameObject.transform.position + positionOffset;
        }
        if ( ownerGameObject == null )
        {
            Destroy(gameObject);
        }
    }
}
