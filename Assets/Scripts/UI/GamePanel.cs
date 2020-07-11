using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

	private Button btn_Pause, btn_Play;
	private Text txt_Score, txt_DiamondCount;

	void Awake() {
		EventCenter.AddListener(EventType.ShowGamePanel,ShowGamePanel);
		EventCenter.AddListener<int>(EventType.UpdateScoreText, UpdateScoreText);
		EventCenter.AddListener<int>(EventType.UpdateDiamondText, UpdateDiamondText);
        Init();
	}
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowGamePanel, ShowGamePanel);
        EventCenter.RemoveListener<int>(EventType.UpdateScoreText, UpdateScoreText);
        EventCenter.RemoveListener<int>(EventType.UpdateDiamondText, UpdateDiamondText);
    }
    /// <summary>
    /// GamePanel初始化方法
    /// </summary>
    void Init()
	{
		btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
		btn_Pause.onClick.AddListener(OnPauseButtonClick);
		btn_Play = transform.Find("btn_Play").GetComponent<Button>();
		btn_Play.onClick.AddListener(OnPlayButtonClick);
		txt_Score = transform.Find("txt_Score").GetComponent<Text>();
		txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();
		btn_Play.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
    /// <summary>
    /// 显示GamePanel
    /// </summary>
    private void ShowGamePanel()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 更新玩家得分文本
    /// </summary>
    /// <param name="score">玩家得分</param>
    private void UpdateScoreText(int score)
    {
        txt_Score.text = score.ToString();
    }
    /// <summary>
    /// 更新玩家获得钻石个数文本
    /// </summary>
    /// <param name="diamond">玩家获得的钻石个数</param>
    private void UpdateDiamondText(int diamond)
    {
        txt_DiamondCount.text = diamond.ToString();
    }
    /// <summary>
    /// Play按钮点击后触发
    /// </summary>
    private void OnPlayButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
		btn_Play.gameObject.SetActive(false);
        btn_Pause.gameObject.SetActive(true);
        //Game Start
        GameManager.Instance.isGamePaused = false;
        Time.timeScale = 1;
    }
    /// <summary>
    /// Pause按钮点击后触发
    /// </summary>
    private void OnPauseButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
		btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);
        //Game Pause
        GameManager.Instance.isGamePaused = true;
        Time.timeScale = 0;
    }
}
