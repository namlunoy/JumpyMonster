using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public delegate void OnFail(Exception ex);
public delegate void OnSuccess(object response);

public class MyLeaderBoard : App42CallBack
{
    public event OnFail OnnFail;
    public event OnSuccess OnnSuccess;

    //Leaderboard Service
    private const string API_KEY = "2d4dfa4b3add90b8ac1bcf1540f53895811bc07c8afc6833e06af458bb512f64";
    private const string SECRET_KEY = "3a296b9be61e021af1efb18de756c7f25a60f67cfc0485848bf938e9f71952a8";
    private const string GAME_NAME = "JumpyJumpy";
    ScoreBoardService scoreBoardService;

    public MyLeaderBoard(OnFail onfail, OnSuccess onsuccess)
    {
        this.OnnFail = onfail;
        this.OnnSuccess = onsuccess;
        App42Log.SetDebug(true);
        App42API.Initialize(API_KEY, SECRET_KEY);
        scoreBoardService = App42API.BuildScoreBoardService();
    }

    //Lưu điểm
    public void SaveScore(string username, int score)
    {
        scoreBoardService.SaveUserScore(GAME_NAME, username, score, this);
    }

    //Lấy danh sách
    public void GetLeaderBoard(int count)
    {
        if (scoreBoardService == null)
            scoreBoardService = App42API.BuildScoreBoardService();
        scoreBoardService.GetTopNRankers(GAME_NAME, count, this);
    }


    public void OnException(Exception ex)
    {
        if (this.OnnFail != null)
            this.OnnFail(ex);
    }
    public void OnSuccess(object response)
    {
        if (this.OnnSuccess != null)
            this.OnnSuccess(response);
    }
}
