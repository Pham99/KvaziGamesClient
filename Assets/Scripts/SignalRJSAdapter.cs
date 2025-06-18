using SimpleJSON;
using UnityEngine;

public class SignalRJSAdapter : MonoBehaviour
{
    public SignalRManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnReceiveDirection(string jsonString)
    {
        JSONNode jsonNode = JSON.Parse(jsonString);
        manager.OnReceiveDirection(jsonNode[0], jsonNode[1]);
    }
    public void AddPlayer(string jsonString)
    {
        JSONNode jsonNode = JSON.Parse(jsonString);
        manager.AddPlayer(jsonNode[0], jsonNode[1]);
    }
    public void RemovePlayer(string id)
    {
        manager.OnPlayerRemove(id);
    }
    public void NotifyConnectionSuccess()
    {
        manager.NotifyConnectionSuccess();
    }
    public void DisplayQRCodeFromBase64(string base64String)
    {
        Debug.Log("from adapter: " + base64String);
        manager.OnReceiveQRCode(base64String);
    }
}
