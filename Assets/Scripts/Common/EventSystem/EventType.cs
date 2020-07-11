/* ***********************************************
* Discribe(说明)：监听与广播系统之关于事件码的枚举类型
* Author(作者)：TenTu
* CreateTime(时间)：2020-03-24 14:42:20
* Email：1939093693@qq.com
* Copyright：@TenTu
* ************************************************/
/// <summary>
/// 每个事件都有它唯一的事件码
/// ：用于广播和监听
/// </summary>
public enum EventType
{
    /// <summary>
    /// 显示GamePanel
    /// </summary>
    ShowGamePanel,
    /// <summary>
    /// 生成路径
    /// </summary>
    DecidePath,
    /// <summary>
    /// 分数增加
    /// </summary>
    AddScore,
    /// <summary>
    /// 刷新分数
    /// </summary>
    UpdateScoreText,
    /// <summary>
    /// 玩家开始移动
    /// </summary>
    PlayerMoved,
    /// <summary>
    /// 钻石个数增加
    /// </summary>
    AddDiamond,
    /// <summary>
    /// 刷新钻石个数
    /// </summary>
    UpdateDiamondText,
    /// <summary>
    /// 显示GameOverPanel
    /// </summary>
    ShowGameOverPanel,
    /// <summary>
    /// 显示MainPanel
    /// </summary>
    ShowMainPanel,
    /// <summary>
    /// 显示ShopPanel
    /// </summary>
    ShowShopPanel,
    /// <summary>
    /// 显示ResetPanel
    /// </summary>
    ShowResetPanel,
    /// <summary>
    /// 显示RankPanel
    /// </summary>
    ShowRankPanel,
    /// <summary>
    /// 选择皮肤
    /// </summary>
    SelectSkin,
    /// <summary>
    /// Hint提示
    /// </summary>
    ShowHint,
    /// <summary>
    /// 播放Button音效
    /// </summary>
    PlayButtonClip,
    /// <summary>
    /// 显示QuitPanel
    /// </summary>
    ShowQuitPanel,
}
