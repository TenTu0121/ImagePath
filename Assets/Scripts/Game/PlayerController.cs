using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;//提供检测是否点击UI的方法:EventSystem.current.IsPointerOverGameObject()
using System;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// 玩家是否开始移动
    /// </summary>
    private bool isMoved = false;
    /// <summary>
    /// 是否向左移动，反之向右
    /// </summary>
    private bool isMoveLeft = false;
    /// <summary>
    /// 是否正在跳跃
    /// </summary>
    private bool isJumping = false;

    private Vector3 nextPlatformLeft, nextPlatformRight;

    /// <summary>
    /// 分数增加相关，用于储存Player上次踩中的平台，防止二次加分
    /// </summary>
    private GameObject lastHitObj = null;
    /// <summary>
    /// 射线检测相关
    /// </summary>
    [Header("Ray Check")]
    public LayerMask platformLayer, obstacleLayer;
    private Rigidbody2D playerRigiBody;
    private Transform rayDown, rayLeft, rayRight;
    private SpriteRenderer playerSpriteRenderer;

    private ManagerVars vars;

    private void Awake()
    {
        EventCenter.AddListener<int>(EventType.SelectSkin, SelectSkin);

        vars = ManagerVars.GetManagerVars();
        Init();
    }
    private void Start()
    {
        //开始时设置下人物皮肤
        SelectSkin(GameManager.Instance.GetSelectSkin());
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventType.SelectSkin, SelectSkin);

    }
    /// <summary>
    /// 选择皮肤
    /// </summary>
    /// <param name="selectIndex">选择的皮肤下标</param>
    private void SelectSkin(int selectIndex)
    {
        //直接设置人物的皮肤：playerSpriteRenderer的sprite
        playerSpriteRenderer.sprite = vars.characterSkinSpriteList[selectIndex];
    }

    private void Init()
    {
        rayDown = transform.Find("RayDown").transform;
        rayLeft = transform.Find("RayLeft").transform;
        rayRight = transform.Find("RayRight").transform;
        playerRigiBody = transform.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = transform.GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 实时绘制射线的可视化线条
    /// </summary>
    private void DrawRay()
    {
        Debug.DrawRay(rayDown.position, Vector2.down * 1, Color.blue);//down
        Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);//left
        Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);//right
    }
    /// <summary>
    /// 检测点击传入的位置是否为UI
    /// </summary>
    /// <param name="mousePos">检测点击的位置</param>
    /// <returns>bool</returns>
    private bool IsPointerOverGameObject(Vector2 mousePos)
    {
        //在当前的事件系统内创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePos;
        //定义一个List<RaycastResult> raycastResults用来存放所有检测结果
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击的位置发射一条射线，检测是否点击到UI，返回所有结果到一个List集合里
        EventSystem.current.RaycastAll(eventData, raycastResults);
        //大于0，表示点击到UI上面了
        return raycastResults.Count > 0;
    }
    private void Update () {

        DrawRay();//绘制射线的可视化

        //检测当前是否点击到UI上，是则不进行跳跃（Text可以取消射线检测，表示不用检测此组件）
        //IsPointerOverGameObject在手机上就失效了，这里自己封装一个检测ui的方法
        if (IsPointerOverGameObject(Input.mousePosition))
            return;
        //如果没有开始游戏或者游戏结束了，就跳过后面的代码，直接返回
        if (GameManager.Instance.isGameStarted == false || GameManager.Instance.isGameOvered || GameManager.Instance.isGamePaused)
            return;

        //按下鼠标，并且不在跳跃状态时，才能跳。不然快速按下鼠标会一直执行
        if (Input.GetMouseButtonDown(0) && isJumping == false && nextPlatformLeft != Vector3.zero)
        {
            if (isMoved == false)
            {
                EventCenter.BroadCast(EventType.PlayerMoved);
                isMoved = true;
            }
            //广播（发布）生成路径的事件码
            EventCenter.BroadCast(EventType.DecidePath);

            isJumping = true;//设置成true，避免多次执行
            Vector3 mousePos = Input.mousePosition;
            //点击的是左边的屏幕
            if (mousePos.x <= (Screen.width / 2))
            {
                isMoveLeft = true;
                Debug.Log("left");
            }
            //点击的是右边的屏幕
            else if (mousePos.x > (Screen.width / 2))
            {
                isMoveLeft = false;
                Debug.Log("right");
            }

            Jump();//跳跃函数
        }
        if (playerRigiBody.velocity.y < 0 && RayDownPlatformCast() == false && GameManager.Instance.isGameOvered == false)
        {
            playerSpriteRenderer.sortingLayerName = "Default";
            transform.GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.isGameOvered = true;
            Debug.Log("游戏是否结束：" + GameManager.Instance.isGameOvered + "，原因：RayDownPlatformCast（没踩到平台，掉落）");
            //播放掉落音效
            GameManager.Instance.AudioPlay(vars.fallClip);
            //调用结束面板
            StartCoroutine(DealyShowGameOverPanel());
        }
        if (playerRigiBody.velocity.y < 0 && RayLRObstacleCast() && GameManager.Instance.isGameOvered == false)
        {
            GameManager.Instance.isGameOvered = true;
            Debug.Log("游戏是否结束：" + GameManager.Instance.isGameOvered + "，原因：RayLRObstacleCast（碰到障碍）");
            //播放受伤音效
            GameManager.Instance.AudioPlay(vars.hitClip);
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.transform.position = transform.position;
            go.SetActive(true);
            //gameObject.SetActive(false);
            //Destroy(transform.gameObject);
            //销毁gameobject或者取消gameobject之后都没办法调用协程了，解决方法：取消SpriteRenderer的渲染
            playerSpriteRenderer.enabled = false;//取消渲染就不会显示了
            //调用结束面板
            StartCoroutine(DealyShowGameOverPanel());
        }
        //防止万一Player已经超出MainCamera的范围，都还没判断出游戏结束
        if (transform.position.y - Camera.main.transform.position.y < -6 && GameManager.Instance.isGameOvered == false)
        {
            GameManager.Instance.isGameOvered = true;
            Debug.Log("游戏是否结束：" + GameManager.Instance.isGameOvered + "，原因：超出摄像机MainCamera范围");
            //播放掉落音效
            GameManager.Instance.AudioPlay(vars.fallClip);
            //调用结束面板
            StartCoroutine(DealyShowGameOverPanel());
        }
    }
    /// <summary>
    /// 延迟显示GameOverPanel
    /// </summary>
    /// <returns>延迟的时间</returns>
    private IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(0.5f);
        EventCenter.BroadCast(EventType.ShowGameOverPanel);
    }

    /// <summary>
    /// 检测Player脚下是否有Platform
    /// </summary>
    /// <returns>true/false</returns>
    private bool RayDownPlatformCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null && hit.collider.CompareTag("Platform"))
        {
            if (lastHitObj == null)
            {
                lastHitObj = hit.collider.gameObject;
                return true;
            }
            if (hit.collider.gameObject != lastHitObj)
            {
                EventCenter.BroadCast(EventType.AddScore);
                lastHitObj = hit.collider.gameObject;
            }
            return true;
        }

        return false;
    }
    /// <summary>
    /// 检测Player的左右方向是否有Obstacle
    /// </summary>
    /// <returns>true/false</returns>
    private bool RayLRObstacleCast()
    {
        RaycastHit2D hitLift = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);
        if (hitLift.collider != null && hitLift.collider.CompareTag("Obstacle"))
        {
            return true;
        }
        if (hitRight.collider != null && hitRight.collider.CompareTag("Obstacle"))
        {
            return true;
        }

        return false;
    }

    private void Jump()
    {
        //向左跳跃时
        if (isMoveLeft)
        {
            //face direction：人物朝向
            transform.localScale = new Vector3(-1, 1, 1);
            //move动画，营造跳跃的感觉
            transform.DOMoveX(nextPlatformLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
        }
        //向右跳跃时
        else
        {
            //face direction：人物朝向
            transform.localScale = Vector3.one;
            //move动画
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
        }
        //播放掉落音效
        GameManager.Instance.AudioPlay(vars.jumpClip);
    }
    /// <summary>
    /// 检测是否碰到平台：碰撞双方须有一个勾选isTrigger
    /// </summary>
    /// <param name="collision">平台</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isJumping = false;//碰到新平台后，isJumping设置成false，避免不能跳跃

        if (collision.CompareTag("Platform"))
        {
            //获取当前所在的平台位置
            Vector3 currentPlatformPos = collision.gameObject.transform.position;

            //计算下一个平台的位置（分为左右两个平台，都要计算）
            //为跳跃提供位置
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
        }
    }
    /// <summary>
    /// 检测是否碰到钻石：碰撞双方都没有勾选isTrigger
    /// </summary>
    /// <param name="collision">钻石</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //玩家吃到钻石
        if (collision.gameObject.CompareTag("Pickup"))
        {
            EventCenter.BroadCast(EventType.AddDiamond);
            //播放钻石音效
            GameManager.Instance.AudioPlay(vars.diamondClip);
            collision.gameObject.SetActive(false);
        }
    }
}