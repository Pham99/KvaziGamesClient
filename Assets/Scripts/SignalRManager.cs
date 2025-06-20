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
    private string serverURL = "https://kvazi.online/gamehub?type=game";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [DllImport("__Internal")]
    private static extern void StartConnection(string serverURL);
    void Start()
    {
        textMeshPro = text.GetComponent<TextMeshProUGUI>();
        qrCodeDisplayer = transform.GetComponent<QRCodeDisplayer>();
        signalRClient._manager = this;
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
        NetInput.AddInput(id);
        //signalRPublicAPI.onPlayerConnected.Invoke(id, name);
        playerManager.OnPlayerConnectDelayed(id, name);
        //spawner.TellHimToDoIt(id, name);
    }
    public void OnPlayerRemove(string id)
    {
        Debug.Log("player disconnected" + id);
        playerManager.OnPlayerDisconnectDelayed(id);
    }
    public void OnReceiveQRCode(byte[] qr)
    {
        Debug.Log("it stalled");
        qrCodeDisplayer.DoDisplayQRCode(qr);
        //MainThreadDispatcher.Instance.Enqueue(() => qrCodeDisplayer.DisplayQRCode(qr));
        Debug.Log("this worked");
    }
    public void OnReceiveQRCode(string base64String)
    {
        Debug.Log("starded base64 QR");
        Debug.Log("from manager: " + base64String);
        qrCodeDisplayer.DoDisplayQRCode(base64String);
        //MainThreadDispatcher.Instance.Enqueue(() => qrCodeDisplayer.DisplayQRCode(qr));
        Debug.Log("this worked");
    }
    public void NotifyConnectionSuccess()
    {
        textMeshPro.text = "Connected";
    }

    public void ChangeServerURL(string serverURL)
    {
        this.serverURL = "https://" + serverURL + "/gamehub?type=game";
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
