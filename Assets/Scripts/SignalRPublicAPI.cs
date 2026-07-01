using UnityEngine;
using UnityEngine.Events;

public class SignalRPublicAPI : MonoBehaviour
{
    public UnityEvent<string, string> onPlayerConnected = new UnityEvent<string, string>();
    public UnityEvent onPlayerDisconnected = new UnityEvent();
    public UnityEvent onServerStopped = new UnityEvent();
}
