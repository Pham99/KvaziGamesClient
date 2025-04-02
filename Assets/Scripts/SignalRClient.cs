using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;
using Mygame.NetInput;
using TMPro;

public class SignalRClient
{
    public SignalRManager _manager;
    private HubConnection hubConnection;
    // Start is called before the first frame update
    public async void StartConnection(string serverURL)
    {
        await ConnectToHub(serverURL);
    }

    private async Task ConnectToHub(string serverURL)
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(serverURL)
            .WithAutomaticReconnect()
            .Build();

        // Listen for the "ReceiveDirection" event from the server
        hubConnection.On<string, string>("ReceivedDirection", (direction, id) =>
        {
            _manager.OnReceiveDirection(direction, id);
        });

        hubConnection.On<string, string>("AddPlayerToGame", (id, name) =>
        {
            _manager.AddPlayer(id, name);
        });

        hubConnection.On<byte[]>("SendQRCode", (qrCodeBytes) =>
        {
            Debug.Log("it was sent");
            _manager.OnReceiveQRCode(qrCodeBytes);
        });

        try
        {
            await hubConnection.StartAsync();
            Debug.Log("Connected to SignalR hub!");
            _manager.NotifyConnectionSuccess();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to connect to SignalR hub: {ex.Message}");
        }
    }

}
