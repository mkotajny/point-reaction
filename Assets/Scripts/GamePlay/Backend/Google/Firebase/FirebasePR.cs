using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using UnityEngine;

public static class FirebasePR
{
    static bool _googlePlayInitialized;

    public static Firebase.Auth.FirebaseAuth FirebaseAuth { get; private set; }
    public static DatabaseReference CampaignDbReference { get; set; }
    public static DatabaseReference CampaignsHistoryDbReference { get; set; }
    public static DatabaseReference WorldRankDbReference { get; set; }
    public static DatabaseReference ActivityLogDbReference { get; set; }
    public static DatabaseReference GameSettingsReference { get; set; }
    public static bool FirebaseInitialized { get; private set; }

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
        if (FirebaseInitialized || !CheckInternet.IsConnected())
            return;
        SetNotSignedInDatabaseReferences();
        FirebaseInitialized = true;
    }

    public static void SetNotSignedInDatabaseReferences()
    {
        FirebaseApp app;

        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith((System.Threading.Tasks.Task<DependencyStatus> task) => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;

                app.SetEditorDatabaseUrl("https://point-reaction-44fca.firebaseio.com/");
                if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
                FirebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;

                app.SetEditorP12FileName("point-reaction-44fca8.p12");
                app.SetEditorServiceAccountEmail("f6b8d282bea8e75e5a665ddad81fd8dd446bf3d8@point-reaction-44fca.iam.gserviceaccount.com");
                app.SetEditorP12Password("notasecret");


                FirebaseAuth.SignInWithEmailAndPasswordAsync("10softpl@gmail.com", "ikarbeskidy2001").ContinueWith((System.Threading.Tasks.Task<Firebase.Auth.FirebaseUser> task2) => {
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
                    Debug.LogFormat("debug: Firebase user signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);

                    WorldRankDbReference = FirebaseDatabase.DefaultInstance.GetReference("world_rank");
                    GameSettingsReference = FirebaseDatabase.DefaultInstance.GetReference("game_settings");
                    ActivityLogDbReference = FirebaseDatabase.DefaultInstance.GetReference("activity_log/" + DateTime.Now.ToString("yyyy-MM"));
                    ProgressBarPR.AddProgress("Signed to Firebase without google play");

                    if (PlayerPrefs.GetInt("InGooglePlay") == 1) CurrentPlayer.SignInGooglePlay();
                    else WorldRankPersister.LoadWorldRank();
                    SessionVariables.GetGameSettingsData();
                });

            }
            else
                Debug.LogError("debug: Could not resolve all Firebase dependencies: " + dependencyStatus);
        });
    }

    public static void SignOutFireBase()
    {
        FirebaseAuth.SignOut();
    }
}