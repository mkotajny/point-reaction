using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public static class WorldRankPersister  {

    static List<WorldRankItem> _worldRank = new List<WorldRankItem>();
    static WorldRankItem _currentPlayerItem;

    public static List<WorldRankItem> WorldRank { get { return _worldRank; }  }

    public static void LoadWorldRank()
    {

        FirebasePR.FirebaseDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    _worldRank.Clear();
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                        foreach (var childSnapshot2 in childSnapshot.Children)
                            _worldRank.Add(new WorldRankItem(childSnapshot2.Child("PlayerId").Value.ToString()
                                , childSnapshot2.Child("PlayerName").Value.ToString()
                                , 2
                                , 3
                                , 2.5
                                , 2.7
                                ));
                }
            });

        
    }

    public static void CreateCurrentPlayerItem()
    {
        _currentPlayerItem = new WorldRankItem(CurrentPlayer.PlayerId
            , CurrentPlayer.PlayerName
            , PlayerPrefs.GetInt("LevelNo")
            , PlayerPrefs.GetInt("PointsHit")
            , PlayerPrefs.GetFloat("ReactionAvg")
            , PlayerPrefs.GetFloat("ReactionFastest"));
    }

    public static void UpdateCurrentPlayer()
    {
        string json;

        CreateCurrentPlayerItem();
        json = JsonUtility.ToJson(_currentPlayerItem);

        //FirebasePR.FirebaseDbReference.Child("Top Reactors").Child()
    }
}
