using UnityEngine;
using System.Collections;
using System;

public class AdsWatch : MonoBehaviour {
    public event Action<bool> ShowAds;
    public bool isShow = false;
    public static AdsWatch Instance = null;
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
            Instance = this;
        else Destroy(this.gameObject);
	}

    public void Show_Ads(bool show)
    {
        isShow = show;
        if (ShowAds != null)
            ShowAds(show);
    }
	
}
