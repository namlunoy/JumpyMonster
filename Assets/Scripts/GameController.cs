using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/*
 * Tăng level bằng cách tăng speed của ground và các tốc độ khác nên tính theo tốc độ của ground
 * 
 * Nên xử lý các công việc liên quan đến giao diện và giao tiếp với người dùng ở ngoài này!
 * Vì Nếu player là 1 prefap thì nó ko thể khởi tạo các thuộc tính đang có ngoài Scene được!
 * Chỉ có object controller là lúc nào cũng ở ngoài scene này thôi!
 * */
public class GameController : MonoBehaviour
{
    public RectTransform namePanel;
    private AdsController ads;
    //Âm thanh
    public AudioClip au_HighScore;
    public AudioClip au_SoundWin;

    public float SpeedGround = 0;
    public float SpeedSky = 0;
    public Transform genneratePoint;

    //Các Runner của ta
    public GameObject[] Runners;

    //Các thành phần giao diện
    public GameObject WinPanle;
    public GameObject LossPanel;
    public GameObject PausePanel;
    // public GameObject namePanel;
    public Text txt_Coin;
    public Text txt_HighScore;
    public Text txt_Score;
    public Text txt_username;

    //Đối tượng
    public static GameController Instance = null;
    private RunnerController runnerController = null;
    public RunnerController RunnerController { get { return runnerController; } }


    private string username;
    void Start()
    {
        WinPanle.SetActive(false);
        LossPanel.SetActive(false);

        username = PlayerPrefs.GetString(Config.USERNAME, "");
        bt_sound.image.sprite = Config.Sound_On ? sound_on : sound_off;
        namePanel.gameObject.SetActive(false);
        ads = AdsController.Instance;
        Instance = this;
        print("select : " + ListviewRunners.GetSelectedIndex);
        GameObject theRunner = (GameObject)Instantiate(Runners[ListviewRunners.GetSelectedIndex], genneratePoint.position, Quaternion.identity);
        runnerController = theRunner.GetComponent<RunnerController>();
        ads.Show_Banner(false);
        Config.count2++;

        AdsWatch.Instance.Show_Ads(false);
    }

    #region Update và xử lý giao diện hiện thị thông tin khi chơi
    public void Update_Coin(int coin) { txt_Coin.text = coin.ToString(); }
    public void Update_Score(int score) { txt_Score.text = string.Format("{0:D6}", score); }
    public void Update_HighScore(int h_score) { txt_HighScore.text = h_score.ToString(); }
    int score2;
    public void Show_Winpanel(int score)
    {
        score2 = score;
        AdsWatch.Instance.Show_Ads(true);
        ads.Show_Banner(true);
        WinPanle.SetActive(true);
        WinPanle.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
        //Xem có âm thanh chiến thắng gì không thì kêu lên
        if (Config.count2 % 3 == 0)
            AdsController.Instance.Show_Billboard();

        if (username.Length < 1)
            namePanel.gameObject.SetActive(true);
        else
        {
            MyLeaderBoard leaderboard = new MyLeaderBoard(null, null);
            leaderboard.SaveScore(username, score);
        }
    }

    public void Click_SaveName()
    {
        MyLeaderBoard leaderboard = new MyLeaderBoard(null, null);
        PlayerPrefs.SetString(Config.USERNAME, txt_username.text);
        leaderboard.SaveScore(txt_username.text, score2);
        namePanel.gameObject.SetActive(false);
    }
    public void Show_LossPanel(int score)
    {
        if (username.Length < 1)
            namePanel.gameObject.SetActive(true);
        if (Config.count2 % 3 == 0)
            AdsController.Instance.Show_Billboard();

        AdsWatch.Instance.Show_Ads(true);

        ads.Show_Banner(true);
        LossPanel.SetActive(true);
        LossPanel.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
    }
    public void GetHighScore()
    {
        audio.volume = Config.Sound_On ? 1 : 0;
        audio.clip = au_HighScore;
        audio.Play();
    }
    #endregion


    IEnumerator TangLevel()
    {
        while (runnerController.IsAlive)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(30, 40));
            SpeedGround += 3;
            SpeedSky += 2;
        }
    }


    #region sự kiện người dùng
    public void Click_Resume()
    {
        AdsWatch.Instance.Show_Ads(false);
        ads.Show_Banner(false);
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void Click_Menu()
    {
        Time.timeScale = 1;
        Application.LoadLevel("menu");
    }
    public void Click_Quit()
    {
        Application.Quit();
    }

    public void ClickPause()
    {
        AdsWatch.Instance.Show_Ads(true);
        ads.Show_Banner(true);
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public Button bt_sound;
    public Sprite sound_on;
    public Sprite sound_off;
    public void Click_Mute()
    {
        Config.Sound_On = !Config.Sound_On;
        bt_sound.image.sprite = Config.Sound_On ? sound_on : sound_off;
    }

    public void Click_Share()
    {
        string facebookshare = "https://www.facebook.com/sharer/sharer.php?u=https://play.google.com/store/apps/details?id=com.mgamestudio.jumpymonster";
        Application.OpenURL(facebookshare);
    }

    public void Click_Like()
    {
        Application.OpenURL("https://www.facebook.com/mgamestudio6");
    }

    public void ClickReplay()
    {
        AdsWatch.Instance.Show_Ads(false);
        ads.Show_Banner(false);
        Application.LoadLevel(Application.loadedLevel);
    }
    #endregion

    public GameObject txt_BatDau;
    internal void batdau()
    {
        txt_BatDau.SetActive(false);
    }
}
