using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public static class FirebasePR
{
    static bool _googlePlayInitialized, _firebaseInitialized;
    static Firebase.Auth.FirebaseAuth _firebaseAuth;
    static DatabaseReference _firebaseDbReference;

    public static Firebase.Auth.FirebaseAuth FirebaseAuth { get { return _firebaseAuth; } }
    public static DatabaseReference FirebaseDbReference { get { return _firebaseDbReference; } }

    public static void InitializeGooglePlay()
    {
        if (_googlePlayInitialized)
            return;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
         .RequestServerAuthCode(false /*forceRefresh*/)
         .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Debug.LogFormat("debug: SignInOnClick: Play Games Configuration initialized");
        _firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        _googlePlayInitialized = true;
}

    public static void InitializeFireBaseDb()
    {
        if (_firebaseInitialized)
            return;

        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
        _firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                app.SetEditorDatabaseUrl("https://point-reaction-44fca.firebaseio.com/");
                if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
                _firebaseDbReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
                Debug.LogError("debug: Could not resolve all Firebase dependencies: " + dependencyStatus);
        });
        _firebaseInitialized = true;     
    }
}