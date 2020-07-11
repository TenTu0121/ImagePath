using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    /// <summary>
    /// SpriteRenderer的list，用于更改sprite
    /// </summary>
    public List<SpriteRenderer> spriteRenderers;

    public GameObject obstacle;

    //平台掉落相关
    private Rigidbody2D my_Body2D;
    private bool fallTimer;
    private float platformFallTime;

    private void Awake()
    {
        my_Body2D = transform.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //在某些情况下，是不需要再继续执行掉落代码的
        if (GameManager.Instance.isGameStarted == false || GameManager.Instance.isPlayerMoved == false || GameManager.Instance.isGamePaused)
            return;

        if (fallTimer)
        {
            platformFallTime -= Time.deltaTime;//进行倒计时
            if (platformFallTime < 0)//倒计时结束
            {
                //掉落
                fallTimer = false;
                if (my_Body2D.bodyType != RigidbodyType2D.Dynamic)
                {
                    my_Body2D.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DealyHide());
                }
            }
            //防止万一平台已经超出MainCamera的范围，都还没到时间DealyHide，造成平台资源的浪费
            if (transform.position.y - Camera.main.transform.position.y < -6)
            {
                StartCoroutine(DealyHide());
            }
        }
    }
    /// <summary>
    /// 初始化平台的Sprite
    /// </summary>
    /// <param name="sprite">需要设置的平台Sprite</param>
    public void Init(Sprite sprite, float fallTime, int DirValue)
    {
        my_Body2D.bodyType = RigidbodyType2D.Static;
        fallTimer = true;
        platformFallTime = fallTime;
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }
        //用于需要随机生成左右的陷阱平台（不需要就不用指定，留空即可）
        if (DirValue == 0)//陷阱朝右边
        {
            if (obstacle != null)
            {
                //因为有父物体，用position会不准确了，需要使用localposition才能准确反转
                obstacle.transform.position = new Vector3(-obstacle.transform.localPosition.x,
                    obstacle.transform.localPosition.y,
                    obstacle.transform.localPosition.z);
            }
        }
    }
    /// <summary>
    /// 延时设置物体为false状态
    /// </summary>
    /// <returns>等待时间</returns>
    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
