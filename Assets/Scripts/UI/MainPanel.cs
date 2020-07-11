using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {

    /// <summary>
    /// 四个按钮
    /// </summary>
	private Button btn_Start;
	private Button btn_Shop;
	private Button btn_Rank;
	private Button btn_Sound;
	private Button btn_Reset;
	private Button btn_Quit;

    private ManagerVars vars;

	private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventType.ShowMainPanel, ShowMainPanel);
        EventCenter.AddListener<int>(EventType.SelectSkin, Changebtn_ShopImage);

        Init();
	}
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowMainPanel, ShowMainPanel);
        EventCenter.RemoveListener<int>(EventType.SelectSkin, Changebtn_ShopImage);
    }

    private void Start()
    {
        //再来一次时候的判断，true的时候直接ShowGamePanel
        if (GameData.IsAgainGame)
        {
            EventCenter.BroadCast(EventType.ShowGamePanel);
            gameObject.SetActive(false);
        }
        //一开始更改btnShop的Image为选中的皮肤
        Changebtn_ShopImage(GameManager.Instance.GetSelectSkin());
        SetSoundButtonSprite();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
	{
		btn_Start = transform.Find("btn_Start").GetComponent<Button>();
		btn_Start.onClick.AddListener(OnStartButtonClick);
		btn_Shop = transform.Find("Btns/btn_Shop").GetComponent<Button>();
		btn_Shop.onClick.AddListener(OnShopButtonClick);
		btn_Rank = transform.Find("Btns/btn_Rank").GetComponent<Button>();
		btn_Rank.onClick.AddListener(OnRankButtonClick);
		btn_Sound = transform.Find("Btns/btn_Sound").GetComponent<Button>();
		btn_Sound.onClick.AddListener(OnSoundButtonClick);
        btn_Reset = transform.Find("Btns/btn_Reset").GetComponent<Button>();
        btn_Reset.onClick.AddListener(OnResetButtonClick);
        btn_Quit = transform.Find("btn_Quit").GetComponent<Button>();
        btn_Quit.onClick.AddListener(OnQuitButtonClick);
    }

    /// <summary>
    /// 显示MainPanel
    /// </summary>
    private void ShowMainPanel()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 更换btn_Shop的Image
    /// </summary>
    /// <param name="selectIndex">选中的皮肤下标</param>
    private void Changebtn_ShopImage(int selectIndex)
    {
        //更改btnShop的Image为选中的皮肤
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite =
            vars.skinSpriteList[selectIndex];
    }

    /// <summary>
    /// 开始按钮点击后触发
    /// </summary>
    private void OnStartButtonClick()
    {
        GameManager.Instance.isGameStarted = true;//游戏开始，设置为true
        //GameManager.Instance.isGameOvered = false;//游戏开始，设置为false
        EventCenter.BroadCast(EventType.PlayButtonClip);
        EventCenter.BroadCast(EventType.ShowGamePanel);
        gameObject.SetActive(false);
    }
	/// <summary>
	/// 商店按钮点击后触发
	/// </summary>
    private void OnShopButtonClick()
    {
        Debug.Log(GameManager.Instance.GetSelectSkin());
        //发布显示ShopPanel的事件码
        EventCenter.BroadCast(EventType.PlayButtonClip);
        EventCenter.BroadCast(EventType.ShowShopPanel);
    }
	/// <summary>
	/// 排行榜按钮点击后触发
	/// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        EventCenter.BroadCast(EventType.ShowRankPanel);
    }
    /// <summary>
    /// 声音按钮点击后触发
    /// </summary>
    private void OnSoundButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());
        SetSoundButtonSprite();
    }
    public void SetSoundButtonSprite()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicButtonOn;
        }
        else
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicButtonOff;
        }
    }
    /// <summary>
    /// 重置按钮点击后触发
    /// </summary>
    private void OnResetButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //发布显示ResetPanel的事件码
        EventCenter.BroadCast(EventType.ShowResetPanel);
    }
    /// <summary>
    /// 退出按钮点击
    /// </summary>
    private void OnQuitButtonClick()
    {
        EventCenter.BroadCast(EventType.ShowQuitPanel);

    }
}
