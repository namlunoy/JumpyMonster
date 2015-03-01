using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject thongBaoPanel;
    public Transform vitri;
    public Button bt_sound;
    public Sprite sound_on;
    public Sprite sound_off;
    public void Click_Mute()
    {
        Config.Sound_On = !Config.Sound_On;
        bt_sound.image.sprite = Config.Sound_On ? sound_on : sound_off;
        audio.volume = Config.Sound_On ? 1 : 0;
    }

    void Start()
    {
        AdsWatch.Instance.Show_Ads(false);
        AdsController.Instance.Show_Banner(false);
        Config.count++;

        AdsController.Instance.Show_Billboard();
        StartCoroutine(TatThongBao());

        bt_sound.image.sprite = Config.Sound_On ? sound_on : sound_off;
        audio.volume = Config.Sound_On ? 1 : 0;
    }

    IEnumerator TatThongBao()
    {
        yield return new WaitForSeconds(4);
        iTween.MoveTo(thongBaoPanel, iTween.Hash("time", 1f, "position", vitri.position));
    }

    #region Giao tiếp với người dùng
    public void Click_Start()
    {
        audio.Play();
        Application.LoadLevel("main");
    }

    public void Click_LeaderBoard()
    {
        audio.Play();
        Application.LoadLevel("leaderboard");
    }

    public void Click_Share()
    {
        audio.Play();
    }

    public void Click_Info()
    {
        audio.Play();
        infoPanel.SetActive(!infoPanel.activeSelf);

    }

    public void Click_Rate()
    {
        audio.Play();
        if (Application.platform == RuntimePlatform.Android)
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.mgamestudio.jumpymonster");
        else
            Application.OpenURL("http://www.windowsphone.com/en-US/store/publishers?publisherId=mGAME");
    }


    public void Click_Moregame()
    {
        if (Application.platform == RuntimePlatform.Android)
            Application.OpenURL("https://play.google.com/store/apps/developer?id=mGame+Studio");
        else
            Application.OpenURL("http://www.windowsphone.com/en-US/store/publishers?publisherId=mGAME");
    }

    public void Click_Exit()
    {
        Application.Quit();
    }

    public void Click_face()
    {
        Application.OpenURL("https://www.facebook.com/mgamestudio6");
    }

    #endregion
}
