using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using System;
using com.shephertz.app42.paas.sdk.csharp.game;
using UnityEngine.UI;

public class ListviewLeaderboard : MonoBehaviour
{
    //Các thành phần linh tinh của người dùng
    public Text currentUsername;
    public Text score;
    public Text newUserName;

    //Các thành phần giao diện
    public RectTransform Khung;
    public RectTransform ItemsPanel;
    public RectTransform itemModel;
    public RectTransform thongBaoPanel;
    public RectTransform thayTenPanel;

    //Các thuộc tính cần quan tâm
    private float width;
    private float heigh;
    private int numberOfUser = 50;

    void Start()
    {
        try
        {
            thayTenPanel.gameObject.SetActive(false);
      
            currentUsername.text = PlayerPrefs.GetString(Config.USERNAME, "Your Name");
            score.text = PlayerPrefs.GetInt(Config.HIGHSCORE, 0).ToString();

            width = Math.Abs(itemModel.offsetMax.x - itemModel.offsetMin.x);
            heigh = Math.Abs(itemModel.offsetMax.y - itemModel.offsetMin.y);

            MyLeaderBoard leaderboard = new MyLeaderBoard(OnFail, OnSuccess);
            leaderboard.GetLeaderBoard(numberOfUser);
        }
        catch (Exception)
        {

        }
    }


    void OnFail(Exception ex)
    {
        if (thongBaoPanel != null)
            thongBaoPanel.GetChild(0).GetComponent<Text>().text = "Please check your internet connection!";
    }

    void OnSuccess(object response)
    {
        thongBaoPanel.gameObject.SetActive(false);

        Game game = (Game)response;
        App42Log.Console("gameName is " + game.GetName());

        System.Collections.Generic.IList<Game.Score> list = game.GetScoreList();

        ItemsPanel.offsetMax = new Vector2(width, 0);
        ItemsPanel.offsetMin = new Vector2(0, -heigh * list.Count);



        for (int i = 0; i < list.Count; i++)
        {
            MonoBehaviour.print("index__" + i);
            Game.Score score = list[i];
            App42Log.Console("userName is : " + score.GetUserName());
            App42Log.Console("score is : " + score.GetValue());

            RectTransform item = ((GameObject)MonoBehaviour.Instantiate(itemModel.gameObject)).GetComponent<RectTransform>();
            item.transform.parent = ItemsPanel.transform;

            item.offsetMax = new Vector2(width, -heigh * i);
            item.offsetMin = new Vector2(0, -heigh * (i + 1));

            item.GetChild(0).GetComponent<Text>().text = i.ToString();
            item.GetChild(1).GetComponent<Text>().text = score.GetUserName();
            item.GetChild(2).GetComponent<Text>().text = score.GetValue().ToString();

            item.localScale = new Vector3(1, 1, 1);
            item.gameObject.SetActive(true);
        }
    }


    #region sự kiện của người dùng
    public void ClickSave()
    {
        PlayerPrefs.SetString(Config.USERNAME, newUserName.text);
        currentUsername.text = PlayerPrefs.GetString(Config.USERNAME, "Username");
        thayTenPanel.gameObject.SetActive(false);
    }

    public void Click_ChangeName()
    {
        thayTenPanel.gameObject.SetActive(true);
        newUserName.text = PlayerPrefs.GetString(Config.USERNAME, "Username");
    }

    public void Click_Back()
    {
        AdsWatch.Instance.Show_Ads(false);
        Application.LoadLevel("menu");
    }
    #endregion

}
