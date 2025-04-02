using UnityEngine;
using System.Runtime.InteropServices;
using Mygame.NetInput;
using TMPro;

public class SignalRBridge
{
    SignalRManager signalRManager;
    //[DllImport("__Internal")]
    //private static extern void setUnityInstance(string objectName);
    //[DllImport("__Internal")]
    //private static extern void LoadScript(string url);

    void Start()
    {
        //#if UNITY_WEBGL && !UNITY_EDITOR
        //    setUnityInstance(gameObject.name);
        //    LoadScript("Build/signalr.js");
        //#endif
    }

    public void OnReceiveDirection(string direction, string id)
    {
        //signalRManager.OnReceiveDirection(direction, id);
    }
    public void OnAddPlayerToGame(string id)
    {
        //signalRManager.AddPlayer(id);
    }
}