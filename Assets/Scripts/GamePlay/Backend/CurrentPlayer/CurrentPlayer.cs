 using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public static class CurrentPlayer
{
    static string _playerId, _playerName;
    static bool _signedIn;
    public static string PlayerId { get { return _playerId; } }
    public static string PlayerName { get { return _playerName; } }
    public static bool SignedIn { get { return _signedIn; } }

    public static void SignInGooglePlay()
    {
        if (!CheckInternet.IsConnected()) return;

        FirebasePR.InitializeGooglePlay();

        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                Debug.LogError("debug: SignInOnClick: Failed to Sign into Play Games Services.");
                return;
            }

            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            if (string.IsNullOrEmpty(authCode))
            {
                Debug.LogError("debug: SignInOnClick: Signed into Play Games Services but failed to get the server auth code.");
                return;
            }
            Debug.LogFormat("debug: SignInOnClick: Auth code is: {0}", authCode);

            Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
            FirebasePR.FirebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("debug: SignInOnClick was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("debug: SignInOnClick encountered an error: " + task.Exception);
                    return;
                }
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("debug: SignInOnClick: User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

                ResetPlayerAttributes(newUser.UserId, newUser.DisplayName);
                PlayerPrefs.SetInt("InGooglePlay", 1);
                _signedIn = true;
            });
        });
    }

    public static void SignOutGooglePlay()
    {
        FirebasePR.FirebaseAuth.SignOut();
        PlayGamesPlatform.Instance.SignOut();
        _playerId = null;
        _playerName = string.Empty;
        _signedIn = false;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = string.Empty;
        PlayerPrefs.SetInt("InGooglePlay", 0);
    }

    public static void UpdateScores(WorldRankItem worldRankItem)
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerId"))
            || PlayerPrefs.GetString("PlayerId") != _playerId)
        {
            PlayerPrefs.SetString("PlayerId", _playerId);
            PlayerPrefs.SetString("PlayerName", _playerName);
            PlayerPrefs.SetInt("LevelNo", worldRankItem.LevelNo);
            PlayerPrefs.SetInt("BestLevelNo", worldRankItem.LevelNo);
            PlayerPrefs.SetInt("PointsHit", worldRankItem.PointsHit);
            PlayerPrefs.SetFloat("ReactionAvg", float.Parse(worldRankItem.ReactionAvg.ToString("0.00")));
            GameLevelPersister.LevelLoad();
        }
    }

    static void ResetPlayerAttributes(string userId, string userName)
    {
        _playerId = userId;
        _playerName = userName;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = _playerName;

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerId"))
            || PlayerPrefs.GetString("PlayerId") != _playerId)    // new or change of signed in player
        {
            PlayerPrefs.SetInt("ScoreServerUpdated", 1);
            WorldRankPersister.LoadWorldRank(updateSingleUserLocalScores: true);  //for update of player's local scores
        }
    }
}