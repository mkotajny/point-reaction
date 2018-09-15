using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public static class WorldRankPersister  {

    static List<WorldRankItem> _worldRank = new List<WorldRankItem>();
    static bool _loadInProgress = false;

    public static List<WorldRankItem> WorldRank { get { return _worldRank; }  }
    public static bool LoadInProgress {
        get { return _loadInProgress; }
        set { _loadInProgress = value; }
    } 


    public static void LoadWorldRank()
    {
        if (!CheckInternet.IsConnected()) return;
        _worldRank.Clear();
        /*FirebasePR.FirebaseDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    _loadInProgress = true;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        WorldRankItem worldRankItem = new WorldRankItem(childSnapshot.Child("PlayerId").Value.ToString()
                            , childSnapshot.Child("PlayerName").Value.ToString()
                            , System.Convert.ToInt32(childSnapshot.Child("LevelNo").Value)
                            , System.Convert.ToInt32(childSnapshot.Child("PointsHit").Value)
                            , System.Convert.ToDouble(childSnapshot.Child("ReactionAvg").Value));

                        _worldRank.Add(worldRankItem);

                    }
                    if (_worldRank.Count > 1)
                        _worldRank.Sort((b, a) => a.FinalPoints.CompareTo(b.FinalPoints));
                    _loadInProgress = false;
                }
            });*/
    }



    public static void UpdateCurrentPlayer()
    {
        /*string json = JsonUtility.ToJson(new WorldRankItem(CurrentPlayer.PlayerId
            , CurrentPlayer.PlayerName
            , PlayerPrefs.GetInt("LevelNo")
            , PlayerPrefs.GetInt("PointsHit")
            , System.Convert.ToDouble(PlayerPrefs.GetFloat("ReactionAvg").ToString("0.00"))));

        FirebasePR.FirebaseDbReference.Child(CurrentPlayer.PlayerId).SetRawJsonValueAsync(json);*/
    }
}
