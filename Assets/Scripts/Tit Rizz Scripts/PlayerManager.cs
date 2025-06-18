using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject titrizzPlayerPrefab; // Assign a prefab with TitrizzPlayer attached
    public BoardController boardController; // Reference to the BoardController
    public List<TitrizzPlayer> players = new List<TitrizzPlayer>(8);
    public int MaxPlayers => 8;
    private bool OnPlayerConnectCalled = false;
    private string pendingName;
    private string pendingId;
    private bool OnPlayerDisconnectCalled = false;
    private string pendingDisconnectId;

    public float fallInterval = 1.0f; // seconds, configurable in Inspector
    private float fallTimer = 0f;

    public Transform playerNamePanel; // Assign in inspector (the Vertical Layout Group panel)
    public GameObject playerNamePrefab; // Assign in inspector (the PlayerName prefab)
    private Dictionary<string, GameObject> playerNameUIs = new Dictionary<string, GameObject>();

    public void OnPlayerConnect(string id, string name)
    {
        Debug.Log("attempeed to make player");
        if (players.Count >= MaxPlayers)
        {
            Debug.LogWarning("Player limit reached!");
            return;
        }
        // Find the lowest available player number
        int playerNumber = 1;
        var takenNumbers = new HashSet<int>();
        foreach (var p in players) takenNumbers.Add(p.PlayerNumber);
        while (takenNumbers.Contains(playerNumber)) playerNumber++;
        GameObject playerObj = Instantiate(titrizzPlayerPrefab);
        TitrizzPlayer newPlayer = playerObj.GetComponent<TitrizzPlayer>();
        newPlayer.Init(id, name, playerNumber, boardController);
        players.Add(newPlayer);
        Debug.Log($"Player connected: {name} (ID: {id}) as Player {playerNumber}");
        // Instantiate player name UI
        if (playerNamePanel != null && playerNamePrefab != null)
        {
            GameObject nameUI = Instantiate(playerNamePrefab, playerNamePanel);
            var text = nameUI.GetComponent<TMPro.TextMeshProUGUI>();
            if (text != null)
            {
                text.text = name;
                text.color = PLayerColours.GetColour(playerNumber); // or your color logic
            }
            playerNameUIs[id] = nameUI;
        }
    }
    public void OnPlayerConnectDelayed(string id, string name)
    {
        Debug.Log("OnPlayerConnectDelayed called");
        pendingId = id;
        pendingName = name;
        OnPlayerConnectCalled = true;
    }
    public void OnPlayerDisconnect(string id)
    {
        // Find the player by id
        var player = players.Find(p => p.Id == id);
        if (player != null)
        {
            int freedNumber = player.PlayerNumber;
            // Remove the player's piece from the board if it exists
            if (player.CurrentPiece != null && boardController != null)
            {
                boardController.RemovePiece(player.CurrentPiece);
            }
            Destroy(player.gameObject);
            players.Remove(player);
            Debug.Log($"Player disconnected: {id}, freed playerNumber {freedNumber}");
        }
        else
        {
            Debug.LogWarning($"Tried to disconnect player {id} but not found.");
        }
        // Remove player name UI
        if (playerNameUIs.TryGetValue(id, out var nameUI))
        {
            Destroy(nameUI);
            playerNameUIs.Remove(id);
        }
    }
    public void OnPlayerDisconnectDelayed(string id)
    {
        pendingDisconnectId = id;
        OnPlayerDisconnectCalled = true;
    }
    void Update()
    {
        // Timed fall for all active pieces
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallInterval)
        {
            fallTimer = 0f;
            foreach (var player in players)
            {
                if (player.CurrentPiece != null)
                {
                    boardController.MovePiece(player.CurrentPiece, new Vector2Int(0, -1));
                }
            }
        }
        if (OnPlayerConnectCalled)
        {
            OnPlayerConnectCalled = false;
            OnPlayerConnect(pendingId, pendingName);
        }
        if (OnPlayerDisconnectCalled)
        {
            OnPlayerDisconnectCalled = false;
            OnPlayerDisconnect(pendingDisconnectId);
        }
    }
}
