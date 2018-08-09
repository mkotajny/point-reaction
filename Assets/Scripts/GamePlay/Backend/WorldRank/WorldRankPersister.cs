using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public static class WorldRankPersister  {

    static List<WorldRankItem> _worldRank = new List<WorldRankItem>();
    //static WorldRankItem _currentPlayerItem;

    public static List<WorldRankItem> WorldRank { get { return _worldRank; }  }

    public static void LoadWorldRank(bool updateSingleUserLocalScores = false)
    {
        bool userFound = false;
        _worldRank.Clear();
        FirebasePR.FirebaseDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                        foreach (var childSnapshot2 in childSnapshot.Children)
                        {
                            WorldRankItem worldRankItem = new WorldRankItem(childSnapshot2.Child("PlayerId").Value.ToString()
                                , childSnapshot2.Child("PlayerName").Value.ToString()
                                , System.Convert.ToInt32(childSnapshot2.Child("LevelNo").Value)
                                , System.Convert.ToInt32(childSnapshot2.Child("PointsHit").Value)
                                , System.Convert.ToDouble(childSnapshot2.Child("ReactionAvg").Value));

                            _worldRank.Add(worldRankItem);

                            if (updateSingleUserLocalScores && worldRankItem.PlayerId == CurrentPlayer.PlayerId)
                            {
                                CurrentPlayer.UpdateScores(worldRankItem);
                                userFound = true;
                            }
                        }
                    if (updateSingleUserLocalScores && !userFound)
                        CurrentPlayer.UpdateScores(new WorldRankItem(CurrentPlayer.PlayerId, CurrentPlayer.PlayerName, 0, 0, 0));
                }
            });
    }


    public static void UpdateCurrentPlayer()
    {
        string playerId, playerName;

        if (PlayerPrefs.GetInt("ScoreServerUpdated") == 0
            && (!string.IsNullOrEmpty(CurrentPlayer.PlayerId)
                || (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerId")))))

        {
            Debug.Log("debug: Update player in WR !!!");
            PlayerPrefs.SetInt("ScoreServerUpdated", 1); // to prevent from running again in SceneControler.Update
            if (!string.IsNullOrEmpty(CurrentPlayer.PlayerId))
            {
                playerId = CurrentPlayer.PlayerId;
                playerName = CurrentPlayer.PlayerName;
            }
            else
            {
                playerId = PlayerPrefs.GetString("PlayerId");
                playerName = PlayerPrefs.GetString("PlayerName");
            }

            string json = JsonUtility.ToJson(new WorldRankItem(playerId
            , playerName
            , PlayerPrefs.GetInt("LevelNo")
            , PlayerPrefs.GetInt("PointsHit")
            , System.Convert.ToDouble(PlayerPrefs.GetFloat("ReactionAvg").ToString("0.00"))));

            FirebasePR.FirebaseDbReference.Child("Top Reactors")
                .Child(playerName).SetRawJsonValueAsync(json);
        }
    }
}
