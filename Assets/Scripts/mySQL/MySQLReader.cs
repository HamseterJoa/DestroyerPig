using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerDB
{
    public int id;
    public string name;
    public int score;
}

[System.Serializable]
public class PlayerList
{
    public PlayerDB[] players;
}

public class MySQLReader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetPlayers());
    }

    IEnumerator GetPlayers()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/get_players.php");
        yield return www.SendWebRequest();

        if (!www.isNetworkError && !www.isHttpError)
        {
            string rawJson = www.downloadHandler.text;
            string wrappedJson = "{\"players\":" + rawJson + "}";

            PlayerList playerList = JsonUtility.FromJson<PlayerList>(wrappedJson);
            foreach (PlayerDB p in playerList.players)
            {
                Debug.Log($"ID: {p.id}, Name: {p.name}, Score: {p.score}");
            }
        }
        else
        {
            Debug.LogError("요청 실패: " + www.error);
        }
    }
}
