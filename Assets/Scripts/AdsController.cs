using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsController
{
    private static AdsController _instance = null;
    public bool bannerIsShowing = false;
    private RuntimePlatform platform;

    //Google
    BannerView bannerView;
    InterstitialAd interstitial;
#if UNITY_ANDROID
    private string admob_banner_id = "ca-app-pub-6921176146936171/8989165849";
    private string admob_billboard_id = "ca-app-pub-6921176146936171/7512432647";
#else
    //Windows Phone
    private string admob_banner_id = "ca-app-pub-6921176146936171/4419365446";
    private string admob_billboard_id = "ca-app-pub-6921176146936171/2942632248";
#endif


    public static AdsController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AdsController();
            return _instance;
        }
    }
    private AdsController()
    {
        platform = Application.platform;
        if (platform == RuntimePlatform.Android)
        {
            bannerView = new BannerView(admob_banner_id, AdSize.Banner, AdPosition.Top);
            AdRequest request = new AdRequest.Builder().Build();
            bannerView.LoadAd(request);

            interstitial = new InterstitialAd(admob_billboard_id);
            AdRequest request2 = new AdRequest.Builder().Build();
            interstitial.LoadAd(request);
        }

    }


    public void Show_Banner(bool isShow)
    {
        bannerIsShowing = isShow;
        if (platform == RuntimePlatform.Android)
        {
            if (isShow)
                bannerView.Show();
            else bannerView.Hide();
        }
    }

    public void Show_Billboard()
    {
        if (platform == RuntimePlatform.Android)
        {
            if (interstitial.IsLoaded())
                interstitial.Show();
        }
    }
}

