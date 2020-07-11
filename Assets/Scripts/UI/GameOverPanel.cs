using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverPanel : MonoBehaviour {

    private Button btn_Restart, btn_Rank, btn_Home;
    private Text txt_Score, txt_BestScore, txt_AddDiamondCount;
    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowGameOverPanel, ShowGameOverPanel);

        Init();
    }
    private void OnDestroy()
    {
		EventCenter.RemoveListener(EventType.ShowGameOverPanel, ShowGameOverPanel);

    }
    /// <summary>
    /// GameOverPanel初始化方法
    /// </summary>
    void Init()
    {
        gameObject.SetActive(false);
        btn_Restart = transform.Find("btn_Restart").GetComponent<Button>();
        btn_Restart.onClick.AddListener(OnRestartButtonClick);
        btn_Rank = transform.Find("btn_Rank").GetComponent<Button>();
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Home = transform.Find("btn_Home").GetComponent<Button>();
        btn_Home.onClick.AddListener(OnHomeButtonClick);
        txt_Score = transform.Find("txt_Score").GetComponent<Text>();
        txt_BestScore = transform.Find("txt_BestScore").GetComponent<Text>();
        txt_AddDiamondCount = transform.Find("Diamond/txt_AddDiamondCount").GetComponent<Text>();
    }
    /// <summary>
    /// 显示GameOverPanel
    /// </summary>
    private void ShowGameOverPanel()
    {
        //判断成绩是否比最好成绩大
        if (GameManager.Instance.GetGameScore() > GameManager.Instance.GetBestScore())
        {
            txt_Score.transform.GetChild(0).gameObject.SetActive(true);
            txt_BestScore.text = "最高分  "+ GameManager.Instance.GetGameScore();
        }
        else
        {
            txt_Score.transform.GetChild(0).gameObject.SetActive(false);
            txt_BestScore.text = "最高分  " + GameManager.Instance.GetBestScore();
        }
        //更新分数Text
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        //保存下分数
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());
        //更新获得的钻石text
        txt_AddDiamondCount.text = "+" + GameManager.Instance.GetGameDiamond().ToString();
        //更新获得的钻石到总钻石里面去
        GameManager.Instance.UpdateAllDiamond(GameManager.Instance.GetGameDiamond());
        gameObject.SetActive(true);
    }
    /// <summary>
    /// ReStart按钮
    /// </summary>
    private void OnRestartButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;
    }
    /// <summary>
    /// 排行榜按钮
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        EventCenter.BroadCast(EventType.ShowRankPanel);
    }
    /// <summary>
    /// 主页按钮
    /// </summary>
    private void OnHomeButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;
    }
}
