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
        FirebasePR.WorldRankDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    _loadInProgress = true;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        WorldRankItem worldRankItem = new WorldRankItem(childSnapshot.Child("PlrId").Value.ToString()
                                , childSnapshot.Child("PlrName").Value.ToString()
                                , System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value)
                                , System.Convert.ToInt32(childSnapshot.Child("PtsHit").Value)
                                , System.Convert.ToDouble(childSnapshot.Child("ReacAvg").Value));

                        _worldRank.Add(worldRankItem);

                    }
                    if (_worldRank.Count > 1)
                        _worldRank.Sort((b, a) => a.FinalPts.CompareTo(b.FinalPts));
                    _loadInProgress = false;
                }
            });
    }
}
