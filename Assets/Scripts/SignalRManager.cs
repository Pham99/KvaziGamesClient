using Mygame.NetInput;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class SignalRManager : MonoBehaviour
{
    [SerializeField]
    private GameObject text;
    private TMPro.TextMeshProUGUI textMeshPro;
    private SignalRClient signalRClient = new();
    [SerializeField]
    private SignalRPublicAPI signalRPublicAPI;
    [SerializeField]
    private PlayerManager playerManager;
    private QRCodeDisplayer qrCodeDisplayer;
    [SerializeField]
    private string serverURL = "http://192.168.0.93/gamehub";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [DllImport("__Internal")]
    private static extern void StartConnection(string serverURL);
    void Start()
    {
        signalRPublicAPI = FindFirstObjectByType<SignalRPublicAPI>();
        textMeshPro = text.GetComponent<TextMeshProUGUI>();
        qrCodeDisplayer = transform.GetComponent<QRCodeDisplayer>();
        signalRClient._manager = this;
        var initDispatcher = MainThreadDispatcher.Instance;
    }

    void Update()
    {
        NetInput.UpdateInputMaps();
    }
    public void OnReceiveDirection(string direction, string id)
    {
        Debug.Log($"Direction received: {direction}");
        NetInput.HandleInput(direction, id);
    }
    public void AddPlayer(string id, string name)
    {
        Debug.Log("new player spotted");
        try
        {
            Debug.Log("1. SignalR Callback Hit");

            // Force the execution into a lambda
            MainThreadDispatcher.Instance.Enqueue(() => 
            {
                Debug.Log("3. Dispatcher is executing the lambda on the Main Thread");
                
                if (signalRPublicAPI != null)
                {
                    signalRPublicAPI.onPlayerConnected.Invoke(id, name);
                    Debug.Log("4. UnityEvent successfully invoked");
                }
                else
                {
                    Debug.LogError("UnityEvent is null! Initialize it first.");
                }
            });

            Debug.Log("2. Action successfully Enqueued");
        }
        catch (System.Exception ex)
        {
            // This will catch the silent background thread crash and force it to the console
            Debug.LogError($"Background Thread Exception: {ex.Message}\n{ex.StackTrace}");
        }
        NetInput.AddInput(id);
        //playerManager.OnPlayerConnectDelayed(id, name);
        //spawner.TellHimToDoIt(id, name);
    }
    public void OnPlayerRemove(string id)
    {
        Debug.Log("player disconnected" + id);
        playerManager.OnPlayerDisconnectDelayed(id);
    }
    public void OnReceiveQRCode(byte[] qr)
    {
        MainThreadDispatcher.Instance.Enqueue(() => qrCodeDisplayer.DisplayQRCode(qr));
    }
    public void OnReceiveQRCode(string base64String)
    {
        Debug.Log("from manager: " + base64String);
        MainThreadDispatcher.Instance.Enqueue(() => qrCodeDisplayer.DisplayQRCodeFromBase64(base64String));
    }
    public void NotifyConnectionSuccess()
    {
        textMeshPro.text = "Connected";
    }

    public void ChangeServerURL(string serverURL)
    {
        this.serverURL = "http://" + serverURL + "/gamehub";
    }
    public void StartConnection()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartConnection(serverURL);
#else
        signalRClient.StartConnection(serverURL);
#endif
    }
}
