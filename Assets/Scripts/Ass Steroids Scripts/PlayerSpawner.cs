using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;
    RandomColour randomColour = new();
    public bool doit = false;
    public string id;
    public string playerName;
    PlayerNameTextFactory playerNameTextFactory;
    HpBarFactory hpBarFactory;
    void Start()
    {
        playerNameTextFactory = transform.GetComponent<PlayerNameTextFactory>();
        hpBarFactory = transform.GetComponent<HpBarFactory>();
    }
    void Update()
    {
        if (doit)
        {
            Debug.Log("spawned player");
            GameObject thing = Instantiate(playerPrefab);
            PlayerInfo thingInfo = thing.GetComponent<PlayerInfo>();
            thingInfo.Init(id, randomColour.GetRandomHueColor(), playerName);
            hpBarFactory.CreateHPBar(thing);
            playerNameTextFactory.CreatePlayerText(thing);
            Debug.Log("nothing happened");
        }
        doit = false;
    }
    public void SpawnPlayer(string id)
    {
        GameObject spawnedPlayer = Instantiate(playerPrefab);
        spawnedPlayer.GetComponent<PlayerInfo>().Init(id, randomColour.GetRandomHueColor(), playerName);
        playerNameTextFactory.CreatePlayerText(spawnedPlayer);
    }
    public void TellHimToDoIt(string id, string name)
    {
        doit = true;
        this.id = id;
        this.playerName = name;
    }
}
