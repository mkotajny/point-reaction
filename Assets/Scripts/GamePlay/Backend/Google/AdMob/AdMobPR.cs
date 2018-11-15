using GoogleMobileAds.Api;
using System;
using UnityEngine;

public enum AdmobPRSatuses
{
    StartingPoint,
    AdLoaded,
    AdFailedToLoad,
    AdInProgress,
    AdClosedBeforeReward,
    AdClosedAfterReward
}

public static class AdMobPR  {
    static AdRequest _request;
    static GameMode_1 _gameMode_1;
    static bool _initialized = false;
    

#if UNITY_ANDROID
    static string _appId = "ca-app-pub-9423577850321975~7011202817";
    
    static string _adUnitId = "ca-app-pub-3940256099942544/5224354917";   //rewarded-video ANDROID test unit id
    //static string _adUnitId = "ca-app-pub-9423577850321975/6791996376";   //real PR add unit id

#else
    static string _appId = "unexpected_platform";
    static string _adUnitId = "unexpected_platform";
#endif

    public static AdmobPRSatuses AdmobPRSatuses { get; set; }
    public static Timer LoadingAdTimer { get; private set; }
    public static int LoadingAddAttempts { get; private set; }
    public static RewardBasedVideoAd RewardBasedVideo { get; private set; }

    public static void Initialize()
    {
        //Debug.Log
        if (!CheckInternet.IsConnected() || _initialized)
            return;

        LoadingAdTimer = new Timer(30, false);
        AdmobPRSatuses = AdmobPRSatuses.StartingPoint;
        MobileAds.Initialize(_appId);
        RewardBasedVideo = RewardBasedVideoAd.Instance;
        RewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        RewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        RewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        RewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        RewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        RewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        RewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
        RequestRewardBasedVideo(restartAttempts: true);
        _initialized = true;
    }


    public static void InjectGameode(GameMode_1 gameMode_1)
    {
        _gameMode_1 = gameMode_1;
    }

    public static void RequestRewardBasedVideo(bool restartAttempts = false)
    {
        if (!CheckInternet.IsConnected() || SessionVariables.TrialMode) return;

        if (restartAttempts) LoadingAddAttempts = 1;
        else LoadingAddAttempts++; LoadingAdTimer.Activate();

        _request = new AdRequest.Builder()
            .AddTestDevice("0CE9D0BDCFD6B8B96D3440ADC1D453EC")
            .Build();
        RewardBasedVideo.LoadAd(_request, _adUnitId);
    }


    public static void ShowRewardBasedVideo()
    {
        AdmobPRSatuses = AdmobPRSatuses.AdInProgress;
        RewardBasedVideo.Show();
    }

    static void RewardPlayer()
    {
        AdmobPRSatuses = AdmobPRSatuses.AdClosedAfterReward;
        CurrentPlayer.CampaignItem.BnsTaken++;
        _gameMode_1.SaveToFireBase(false);
}

    static void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        AdmobPRSatuses = AdmobPRSatuses.AdLoaded;
        LoadingAdTimer.Deactivate();
        LoadingAddAttempts = 0;
        Debug.Log("debug: AdMob: HandleRewardBasedVideoLoaded event received");
    }

    static void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        AdmobPRSatuses = AdmobPRSatuses.AdFailedToLoad;
        Debug.Log("debug: AdMob: HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
        SessionVariables.ActivityLog.Send(LogCategories.AdLoadFailed, "HandleRewardBasedVideoFailedToLoad raised with message: " + args.Message);
    }

    static void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("debug: AdMob: HandleRewardBasedVideoOpened event received");
    }

    static void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        Debug.Log("debug: AdMob: HandleRewardBasedVideoStarted event received");
    }

    static void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        if (AdmobPRSatuses != AdmobPRSatuses.AdClosedAfterReward)
            AdmobPRSatuses = AdmobPRSatuses.AdClosedBeforeReward;
        Debug.Log("debug: AdMob: HandleRewardBasedVideoClosed event received");
        RequestRewardBasedVideo(restartAttempts: true);
}

    static void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        RewardPlayer();
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("debug: AdMob: HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
    }

    static void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("debug: AdMob: HandleRewardBasedVideoLeftApplication event received");
    }
}
