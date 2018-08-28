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
        try
        {
            if (!CheckInternet.IsConnected()) return;
            _worldRank.Clear();
            FirebasePR.FirebaseDbReference
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        foreach (var childSnapshot in snapshot.Children)
                        {
                            WorldRankItem worldRankItem = new WorldRankItem(childSnapshot.Child("PlayerId").Value.ToString()
                                , childSnapshot.Child("PlayerName").Value.ToString()
                                , System.Convert.ToInt32(childSnapshot.Child("LevelNo").Value)
                                , System.Convert.ToInt32(childSnapshot.Child("PointsHit").Value)
                                , System.Convert.ToDouble(childSnapshot.Child("ReactionAvg").Value));

                            _worldRank.Add(worldRankItem);

                            if (updateSingleUserLocalScores && worldRankItem.PlayerId == CurrentPlayer.PlayerId)
                            {
                                CurrentPlayer.UpdateScores(worldRankItem);
                                userFound = true;
                            }
                        }
                        if (updateSingleUserLocalScores && !userFound )
                        {
                            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerId")))
                            {
                                CurrentPlayer.UpdateScores(new WorldRankItem(CurrentPlayer.PlayerId, CurrentPlayer.PlayerName
                                    , 0, 0, 0));
                            }
                            else
                            {
                                PlayerPrefs.SetInt("ScoreServerUpdated", 0);
                                WorldRankPersister.UpdateCurrentPlayer();
                                CurrentPlayer.UpdateScores(new WorldRankItem(CurrentPlayer.PlayerId, CurrentPlayer.PlayerName
                                    , PlayerPrefs.GetInt("BestLevelNo")
                                    , PlayerPrefs.GetInt("PointsHit")
                                    , PlayerPrefs.GetFloat("ReactionAvg")));
                            }
                        }
                        if (!updateSingleUserLocalScores && _worldRank.Count > 1)
                            _worldRank.Sort((b, a) => a.FinalPoints.CompareTo(b.FinalPoints));
                    }
                });
        }
        catch (System.Exception e)
        {
            Debug.LogError("debug: " + e.Message + "::: " + e.StackTrace);
        }
    }


    public static void UpdateCurrentPlayer()
    {
        string playerId, playerName;

        /*Debug.Log("debug: UpdateCurrentPlayer: chk1: " + PlayerPrefs.GetInt("ScoreServerUpdated") 
            + "; " + CurrentPlayer.PlayerId + "; " + PlayerPrefs.GetString("PlayerId"));*/

        if (PlayerPrefs.GetInt("ScoreServerUpdated") == 0
            && (!string.IsNullOrEmpty(CurrentPlayer.PlayerId)
                || (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerId")))))

        {
            if (!CheckInternet.IsConnected()) return;

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

            FirebasePR.FirebaseDbReference.Child("topreactors")
                .Child(playerId).SetRawJsonValueAsync(json);
        }
    }
}
