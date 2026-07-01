using TMPro;
using UnityEngine;

public class PlayerNameTextFactory : MonoBehaviour
{
    public Canvas canvas;
    public GameObject textPrefab;

    public void CreatePlayerText(GameObject player)
    {
        GameObject textObj = Instantiate(textPrefab, canvas.transform);

        textObj.transform.localPosition = Vector3.zero;
        textObj.transform.localScale = Vector3.one;
        textObj.GetComponent<NameTag>().ownerGameObject = player;
        PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();

        TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = playerInfo.username;
        }
        else
        {
            Debug.LogWarning("The instantiated prefab does not have a Text component!");
        }
    }
}
