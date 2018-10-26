using GoogleMobileAds.Api;
using System;
using UnityEngine;

public enum AdmobPRSatuses
{
    AdNotStarted,
    AdInProgress,
    AdClosedBeforeReward,
    AdClosedAfterReward
}

public static class AdMobPR  {

    static RewardBasedVideoAd _rewardBasedVideo;
    static AdRequest _request;
    static AdmobPRSatuses _admobPRSatuses;
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
    
    public static AdmobPRSatuses AdmobPRSatuses
    {
        get { return _admobPRSatuses; }
        set { _admobPRSatuses = value; }
    }

    public static RewardBasedVideoAd RewardBasedVideo { get { return _rewardBasedVideo; }  } 

    public static void Initialize()
    {
        //Debug.Log
        if (!CheckInternet.IsConnected() || _initialized)
            return;

        _admobPRSatuses = AdmobPRSatuses.AdNotStarted;
        MobileAds.Initialize(_appId);
        _rewardBasedVideo = RewardBasedVideoAd.Instance;
        _rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        _rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        _rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        _rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        _rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        _rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        _rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
        //RequestRewardBasedVideo();
        _initialized = true;
    }


    public static void InjectGameode(GameMode_1 gameMode_1)
    {
        _gameMode_1 = gameMode_1;
    }

    public static void RequestRewardBasedVideo()
    {
        if (!CheckInternet.IsConnected())
            return;

        _request = new AdRequest.Builder()
            //.AddTestDevice("0CE9D0BDCFD6B8B96D3440ADC1D453EC")
            .Build();
        _rewardBasedVideo.LoadAd(_request, _adUnitId);
    }


    public static void ShowRewardBasedVideo()
    {
        _admobPRSatuses = AdmobPRSatuses.AdInProgress;
        _rewardBasedVideo.Show();
    }

    static void RewardPlayer()
    {
        _admobPRSatuses = AdmobPRSatuses.AdClosedAfterReward;
        CurrentPlayer.CampaignItem.BnsTaken++;
        _gameMode_1.SaveToFireBase(false);
}

    static void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("debug: AdMob: HandleRewardBasedVideoLoaded event received");
    }

    static void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("debug: AdMob: HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
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
        if (_admobPRSatuses != AdmobPRSatuses.AdClosedAfterReward)
            _admobPRSatuses = AdmobPRSatuses.AdClosedBeforeReward;
        Debug.Log("debug: AdMob: HandleRewardBasedVideoClosed event received");
        RequestRewardBasedVideo();
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
