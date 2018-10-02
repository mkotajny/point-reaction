﻿using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public static class FirebasePR
{
    static bool _googlePlayInitialized, _firebaseInitialized;
    static Firebase.Auth.FirebaseAuth _firebaseAuth;
    static DatabaseReference _campaignDbReference, _campaignsHistoryDbReference, _worldRankDbReference;

    public static Firebase.Auth.FirebaseAuth FirebaseAuth{ get { return _firebaseAuth; }}
    public static DatabaseReference CampaignDbReference { get { return _campaignDbReference; } }
    public static DatabaseReference WorldRankDbReference { get { return _worldRankDbReference; } }
    public static DatabaseReference CampaignsHistoryDbReference { get { return _campaignsHistoryDbReference; } }

    public static void InitializeGooglePlay()
    {
        if (_googlePlayInitialized || !CheckInternet.IsConnected())
            return;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
         .RequestServerAuthCode(false /*forceRefresh*/)
         .RequestIdToken()
         .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Debug.LogFormat("debug: SignInOnClick: Play Games Configuration initialized");
        _googlePlayInitialized = true;
    }

    public static void InitializeFireBaseDb()
    {
        if (_firebaseInitialized || !CheckInternet.IsConnected())
            return;
        SetDatabaseReferences();
        _firebaseInitialized = true;

    }

    static void SetDatabaseReferences()
    {
        FirebaseApp app;

        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;

                app.SetEditorDatabaseUrl("https://point-reaction-44fca.firebaseio.com/");
                if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
                _firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;

                app.SetEditorP12FileName("point-reaction-44fca8.p12");
                app.SetEditorServiceAccountEmail("f6b8d282bea8e75e5a665ddad81fd8dd446bf3d8@point-reaction-44fca.iam.gserviceaccount.com");
                app.SetEditorP12Password("notasecret");


                _firebaseAuth.SignInWithEmailAndPasswordAsync("10softpl@gmail.com", "ikarbeskidy2001").ContinueWith(task2 => {
                    if (task2.IsCanceled)
                    {
                        Debug.LogError("debug: SignInWithEmailAndPasswordAsync was canceled.");
                        return;
                    }
                    if (task2.IsFaulted)
                    {
                        Debug.LogError("debug: SignInWithEmailAndPasswordAsync encountered an error: " + task2.Exception);
                        return;
                    }
                    Firebase.Auth.FirebaseUser newUser = task2.Result;
                    Debug.LogFormat("debug: User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                    
                    _campaignDbReference = FirebaseDatabase.DefaultInstance.GetReference("campaigns");
                    _campaignsHistoryDbReference = FirebaseDatabase.DefaultInstance.GetReference("campaigns_history");
                    _worldRankDbReference = FirebaseDatabase.DefaultInstance.GetReference("world_rank");
                });

            }
            else
                Debug.LogError("debug: Could not resolve all Firebase dependencies: " + dependencyStatus);
        });
    }

    public static void SignOutFireBase()
    {
        _firebaseAuth.SignOut();
    }
}