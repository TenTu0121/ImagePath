using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class QuitPanel : MonoBehaviour {

    private Button btn_Yes, btn_No;
    private Image bg;
    private GameObject dialog;

    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowQuitPanel, ShowQuitPanel);

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
        EventCenter.RemoveListener(EventType.ShowQuitPanel, ShowQuitPanel);
    }

    private void OnYesButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //退出游戏
        Application.Quit();
    }

    private void OnNoButtonClick()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        //消失动画播放完之后，隐藏gameobject
        bg.DOColor(new Color(bg.color.r, bg.color.g, bg.color.b, 0.4f), 0.2f);
        dialog.transform.DOScale(Vector3.one, 0.2f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }

    private void ShowQuitPanel()
    {
        EventCenter.BroadCast(EventType.PlayButtonClip);
        gameObject.SetActive(true);
        bg.DOColor(new Color(bg.color.r, bg.color.g, bg.color.b, 0.4f), 0.2f);
        dialog.transform.DOScale(Vector3.one, 0.2f);
    }
}
