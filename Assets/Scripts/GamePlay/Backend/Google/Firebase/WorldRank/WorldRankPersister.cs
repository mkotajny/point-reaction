using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public static class WorldRankPersister
{

    static List<WorldRankItem> _worldRank = new List<WorldRankItem>();
    static bool _loadInProgress = false;
    static int _currentPlayerPosition;

    public static List<WorldRankItem> WorldRank { get { return _worldRank; } }
    public static bool LoadInProgress
    {
        get { return _loadInProgress; }
        set { _loadInProgress = value; }
    }
    public static int CurrentPlayerPosition
    {
        get { return _currentPlayerPosition; }
        set { _currentPlayerPosition = value; }
    }

    public static void Reset()
    {
        _worldRank = new List<WorldRankItem>();
    }

    public static void LoadWorldRank()
    {
        if (_worldRank.Count > 0)
        {
            SetCurrentPlayerPosition();
            return;
        }
        _loadInProgress = true;
        _worldRank.Clear();
        FirebasePR.WorldRankDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (_worldRank.Count > 0) return;
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        WorldRankItem worldRankItem = new WorldRankItem(childSnapshot.Child("PlrId").Value.ToString()
                                , childSnapshot.Child("PlrName").Value.ToString()
                                , System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value)
                                , System.Convert.ToInt32(childSnapshot.Child("PtsHit").Value)
                                , System.Convert.ToDouble(childSnapshot.Child("ReacAvg").Value));

                        _worldRank.Add(worldRankItem);
                    }
                    if (_worldRank.Count > 1) SortRank();
                    _loadInProgress = false;
                    ProgressBarPR.AddProgress("Load full World rank");
                }
            });
    }

    public static void UpdateCurrentPlayer()
    {
        if (_currentPlayerPosition == 0)
        {
            _worldRank.Add(CurrentPlayer.WorldRankItem);
        }
        else
        {
            foreach (WorldRankItem eachWorldRankItem in _worldRank)
            {
                if (eachWorldRankItem.PlrId == CurrentPlayer.WorldRankItem.PlrId)
                {
                    eachWorldRankItem.LvlNo = CurrentPlayer.WorldRankItem.LvlNo;
                    eachWorldRankItem.PtsHit = CurrentPlayer.WorldRankItem.PtsHit;
                    eachWorldRankItem.ReacAvg = CurrentPlayer.WorldRankItem.ReacAvg;
                    eachWorldRankItem.CalculateFinalPoints();
                    break;
                }
            }
        }
        SortRank();
    }

    static void SortRank()
    {
        _worldRank.Sort((b, a) => a.FinalPts.CompareTo(b.FinalPts));
        if (CurrentPlayer.WorldRankItem != null)
            SetCurrentPlayerPosition();
    }

    static void SetCurrentPlayerPosition()
    {
        if (CurrentPlayer.WorldRankItem.ReacAvg == 0)
            return;
        int counter = 0;
        foreach (WorldRankItem eachWorldRankItem in _worldRank)
        {
            counter++;
            if (eachWorldRankItem.PlrId == CurrentPlayer.WorldRankItem.PlrId)
            {
                _currentPlayerPosition = counter;
                break;
            }
        }
    }
}
