using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Hint : MonoBehaviour {

    /// <summary>
    /// Hint背景图
    /// </summary>
    private Image hintImage;
    /// <summary>
    /// Hint的Text
    /// </summary>
    private Text txt_Tips;

    private void Awake()
    {
        EventCenter.AddListener<string>(EventType.ShowHint, ShowHint);

        hintImage = transform.GetComponent<Image>();
        txt_Tips = transform.GetChild(0).GetComponent<Text>();
        txt_Tips.color = new Color(txt_Tips.color.r, txt_Tips.color.g, txt_Tips.color.b, 0);
        hintImage.color = new Color(hintImage.color.r, hintImage.color.g, hintImage.color.b, 0);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventType.ShowHint, ShowHint);
    }
    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="text">提示文本</param>
    private void ShowHint(string text)
    {
        txt_Tips.text = text;
        StopCoroutine("Dealy");
        transform.localPosition = new Vector3(0, -70, 0);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(() =>
        {
            StartCoroutine("Dealy");//用String类型开启协程可以停止
        });
        hintImage.DOColor(new Color(hintImage.color.r, hintImage.color.g, hintImage.color.b, 0.5f), 0.1f);
        txt_Tips.DOColor(new Color(txt_Tips.color.r, txt_Tips.color.g, txt_Tips.color.b, 1), 0.1f);
    }
    /// <summary>
    /// 提示延迟消失
    /// </summary>
    /// <returns>延迟的时间</returns>
    private IEnumerator Dealy()
    {
        yield return new WaitForSeconds(0.5f);
        txt_Tips.color = new Color(txt_Tips.color.r, txt_Tips.color.g, txt_Tips.color.b, 0);
        hintImage.color = new Color(hintImage.color.r, hintImage.color.g, hintImage.color.b, 0);
    }
}
