using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;

public class PlayerData
{

    public string plummie_tag;
    public int collisions;
    public int steps;

    //Question5 pour la sauvegarde des scores
    public string Game { get; set; }
    public int Score { get; set; }
    public string Timestamp { get; set; }

    public PlayerData(string game, int score)
    {
        Game = game;
        Score = score;
        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }


    public string Stringify() {
        return JsonUtility.ToJson(this);
    }

    public static PlayerData Parse(string json)
    {
        return JsonUtility.FromJson<PlayerData>(json);
    }

    public void FetchPlayerData()
    {
        
    }

    public void SavePlayerData()
    {
        
    }

}
