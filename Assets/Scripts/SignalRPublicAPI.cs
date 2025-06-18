using UnityEngine;
using UnityEngine.Events;

public class SignalRPublicAPI : MonoBehaviour
{
    public UnityEvent<string, string> onPlayerConnected = new UnityEvent<string, string>();
    public UnityEvent onPlayerDisconnected = new UnityEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InvokeTest()
    {
        Debug.Log("Test event invoked");
        onPlayerConnected.Invoke("testId", "testName");
    }
}
