using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankPanel : MonoBehaviour {

    private Button btn_Back;
    private Image img_Back;
    private GameObject scoreList;

    public Text[] txt_Score;
    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowRankPanel, ShowRankPanel);

        scoreList = transform.Find("ScoreList").gameObject;
        scoreList.transform.localScale = Vector3.zero;

        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);

        img_Back = btn_Back.transform.GetComponent<Image>();
        img_Back.color = new Color(img_Back.color.r, img_Back.color.g, img_Back.color.b, 0);

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowRankPanel, ShowRankPanel);
    }
    /// <summary>
    /// 显示RankPanel
    /// </summary>
    private void ShowRankPanel()
    {
        gameObject.SetActive(true);
        img_Back.color = new Color(img_Back.color.r, img_Back.color.g, img_Back.color.b, 0.4f);
        scoreList.transform.DOScale(Vector3.one, 0.2f);

        //更新排行榜
        int[] scoreArr = GameManager.Instance.GetBestScoreArr();
        for (int i = 0; i < scoreArr.Length; i++)
        {
            txt_Score[i].text = scoreArr[i].ToString();
        }
    }
    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnBackButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        img_Back.color = new Color(img_Back.color.r, img_Back.color.g, img_Back.color.b, 0);
        scoreList.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}
