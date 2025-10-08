// using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using UnityEditor;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    #region =========================== PROPERTIES ===========================

    [Header("Admob")] 
    // [SerializeField] private string _rewardedAdmobID = "";
    // [SerializeField] private string _interstitialAdmobID = "";
    // [SerializeField] private string _bannerAdmobID = "";
    // [SerializeField] private string _openAdsAdmobID = "";
    // [SerializeField] private string _nativeAdsAdmobID = "";

    [Header("Applovin")] 
    // [SerializeField] private string _bannerID = "";
    // [SerializeField] private string _openAdsID = "";
    // [SerializeField] private string _nativeAdsID = "";
    [SerializeField] private string _rewardedID = "";
    [SerializeField] private string _interstitialID = "";

    [Header("Other")]
    private bool _initialized;
    private bool _openAds;
    [SerializeField] private InterstitialApplovin _interstitial;
    [SerializeField] private RewardedApplovin _rewarded;
    // public event Action OnNativeAdLoaded;

    #endregion

    #region =========================== UNITY CORES ===========================

    private void Awake()
    {
        Init();
    }

    #endregion

    #region =========================== MAIN ===========================

    private void Init()
    {
        if (_initialized) return;

        // Remote.OnRemoteConfigFetched += OnRemoteConfigFetched;
        InitAdsID();
        MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[]
        {
            "5948dbcb-daf2-4012-9b25-d8110a3f32c6", "a098332a-53f2-4d83-a556-1b60e54561c1",
            "d13d9e78-313b-4c35-a08c-e4d1fc10306e"
        });
        MaxSdk.InitializeSdk();
        // MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        // {
        //     MaxSdk.ShowMediationDebugger();
        // };

        _interstitial.Init();
        _rewarded.Init();
        _initialized = true;

    }

    private void InitAdsID()
    {
        _interstitial.AdUnitId = _interstitialID;
        _rewarded.AdUnitId = _rewardedID;
    }

    public void ShowInters(Action<bool> completed = null)
    {
        if (ResourceManager.RemoveAds)
        {
            completed?.Invoke(false);
            return;
        }

        _interstitial.ShowInterstitial(success =>
        {
            completed?.Invoke(success);
        });
    }

    public void ShowRewarded(Action<bool> completed = null)
    {
        _rewarded.ShowReward(completed);
    }
    
    #endregion
}