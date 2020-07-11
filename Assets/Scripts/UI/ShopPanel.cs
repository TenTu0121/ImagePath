using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPanel : MonoBehaviour {

    /// <summary>
    /// 资源容器的引用
    /// </summary>
    private ManagerVars vars;
    /// <summary>
    /// 皮肤的父物体：可滑动的范围
    /// </summary>
    private Transform skinParent;
    /// <summary>
    /// 当前选择的皮肤下标
    /// </summary>
    private int selectIndex;
    private Text txt_Name, txt_BestScore, txt_DiamondCount;
    private Button btn_Back, btn_Select, btn_Buy;


    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowShopPanel, ShowShopPanel);

        vars = ManagerVars.GetManagerVars();
        skinParent = transform.Find("ScrollRect/SkinParent");
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();

        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        btn_Select = transform.Find("btn_Select").GetComponent<Button>();
        btn_Select.onClick.AddListener(OnSelectButtonClick);
        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        btn_Buy.onClick.AddListener(OnBuyButtonClick);

    }

    private void Start()
    {
        //需要在Start方法里调用Init方法，因为要获取游戏数据，而游戏数据的InitGame实在Awake里面进行的
        gameObject.SetActive(false);
        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowShopPanel, ShowShopPanel);
    }


    private void Update()
    {
        //skinParent的localPosition的x是负数，因为它一开始是0，后面向左移，就是负数了，所以要除-160
        selectIndex = Mathf.RoundToInt(skinParent.transform.localPosition.x / -160.0f);

        //Debug.Log(currentIndex);
        if (Input.GetMouseButtonUp(0))
        {
            skinParent.transform.DOLocalMoveX(selectIndex * (-160), 0.2f);
        }
        SetItemSize(selectIndex);
        RefreshUI(selectIndex);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        skinParent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 160, 270.5f);
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go = Instantiate(vars.skinItemPre, skinParent);

            //判断当前皮肤是否解锁：解锁则为白色，否则为灰色
            ////Image在Item预制体的下面，得用GetComponentInChildren
            if (GameManager.Instance.GetUnlockSkin(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }

            //Image在Item预制体的下面，得用GetComponentInChildren
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }

        //更改skinParent的位置，令选中的皮肤位于中间（向左移动选择的下标 *（-160））
        skinParent.localPosition = new Vector3((GameManager.Instance.GetSelectSkin()) * (-160), 0, 0);
    }
    /// <summary>
    /// 显示ShopPanel
    /// </summary>
    private void ShowShopPanel()
    {
        skinParent.localPosition = new Vector3((GameManager.Instance.GetSelectSkin()) * (-160), 0, 0);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 返回按钮事件
    /// </summary>
    private void OnBackButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //发布显示MainPanel的事件码
        EventCenter.BroadCast(EventType.ShowMainPanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 购买按钮事件
    /// </summary>
    private void OnBuyButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //获取皮肤价格
        int price = int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
        //判断当前钻石是否充足
        if (price > GameManager.Instance.GetAllDiamond())
        {
            Debug.Log("钻石不足：" + GameManager.Instance.GetAllDiamond());
            EventCenter.BroadCast<string>(EventType.ShowHint, "钻石不足");
            Debug.Log(GameManager.Instance.GetSelectSkin());
            return;
        }
        //减少钻石数量
        GameManager.Instance.UpdateAllDiamond(-price);
        //设置成白色解锁状态
        GameManager.Instance.SetUnlockSkin(selectIndex);
        skinParent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
    }
    /// <summary>
    /// 选择按钮事件
    /// </summary>
    private void OnSelectButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //广播选择的皮肤下标事件码
        EventCenter.BroadCast(EventType.SelectSkin, selectIndex);
        //设置选择的皮肤下标
        GameManager.Instance.SetSelectSkin(selectIndex);
        Debug.Log(GameManager.Instance.GetSelectSkin());
        //广播显示MainPanel事件码
        EventCenter.BroadCast(EventType.ShowMainPanel);
        //隐藏
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置皮肤的名字
    /// </summary>
    /// <param name="selectIndex">当前选中的皮肤下标</param>
    private void RefreshUI(int selectIndex)
    {
        //设置皮肤名字
        txt_Name.text = vars.skinNameList[selectIndex];
        //设置钻石数量
        txt_DiamondCount.text = GameManager.Instance.GetAllDiamond().ToString();
        //判断皮肤是否为激活状态：调整Buy和Select的显示
        if (GameManager.Instance.GetUnlockSkin(selectIndex) == false)
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            btn_Buy.GetComponentInChildren<Text>().text = vars.skinPriceList[selectIndex].ToString();
        }
        else
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 设置皮肤的大小尺寸
    /// </summary>
    /// <param name="index">当前选中的皮肤下标</param>
    private void SetItemSize(int index)
    {
        for (int i = 0; i < skinParent.childCount; i++)
        {
            if (index == i)
            {
                skinParent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else
            {
                skinParent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }
}
