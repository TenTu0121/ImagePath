using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ResetPanel : MonoBehaviour {

    private Button btn_Yes, btn_No;
    private Image bg;
    private GameObject dialog;

    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowResetPanel, ShowResetPanel);

        bg = transform.Find("bg").GetComponent<Image>();
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);

        dialog = transform.Find("Dialog").gameObject;
        dialog.transform.localScale = Vector3.zero;

        btn_Yes = transform.Find("Dialog/btn_Yes").GetComponent<Button>();
        btn_Yes.onClick.AddListener(OnYesButtonClick);
        btn_No = transform.Find("Dialog/btn_No").GetComponent<Button>();
        btn_No.onClick.AddListener(OnNoButtonClick);

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowResetPanel, ShowResetPanel);
    }
    /// <summary>
    /// 显示ResetPanel
    /// </summary>
    private void ShowResetPanel()
    {
        gameObject.SetActive(true);
        bg.DOColor(new Color(bg.color.r, bg.color.g, bg.color.b, 0.4f), 0.2f);
        dialog.transform.DOScale(Vector3.one, 0.2f);
    }

    /// <summary>
    /// Yes按钮点击触发
    /// </summary>
    private void OnYesButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //调用GameManager的重置游戏方法
        GameManager.Instance.ResetGame();
        //重新加载场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// No按钮点击触发
    /// </summary>
    private void OnNoButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //消失动画播放完之后，隐藏gameobject
        bg.DOColor(new Color(bg.color.r, bg.color.g, bg.color.b, 0.4f), 0.2f);
        dialog.transform.DOScale(Vector3.one, 0.2f).OnComplete(()=> {
            gameObject.SetActive(false);
        });
    }

}
