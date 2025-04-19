using UnityEngine;
using UnityEngine.Events;

public class SignalRPublicAPI : MonoBehaviour
{
    public UnityEvent<string, string> onPlayerConnected;
    public UnityEvent onPlayerDisconnected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
