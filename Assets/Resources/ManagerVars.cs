using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "CreateManagerVarsCountainer")]//创建AssetMenu，用来快捷生成bg需要的资源容器，后面会注释掉
public class ManagerVars : ScriptableObject {

	public static ManagerVars GetManagerVars()
	{
		return Resources.Load<ManagerVars>("ManagerVarsCountainer");
	}
    /// <summary>
    /// 背景图片List：用于更改背景，需要New出来，不然无法指定
    /// </summary>
	public List<Sprite> bgSpriteList = new List<Sprite>();
    /// <summary>
    /// 平台Sprite的List：用于修改平台Sprite，需要New出来，不然无法指定
    /// </summary>
	public List<Sprite> platformSpriteList = new List<Sprite>();
    /// <summary>
    /// 人物皮肤Sprite的List：用于ShopPanel修改Sprite，需要New出来，不然无法指定
    /// </summary>
    public List<Sprite> skinSpriteList = new List<Sprite>();
    /// <summary>
    /// 人物皮肤的价格List
    /// </summary>
    public List<int> skinPriceList = new List<int>();
    /// <summary>
    /// 人物皮肤的名字
    /// </summary>
    public List<string> skinNameList = new List<string>();

    /// <summary>
    /// 人物预制体
    /// </summary>
    public GameObject characterPre;
    /// <summary>
    /// 人物皮肤的预制体
    /// </summary>
    public GameObject skinItemPre;
    /// <summary>
    /// 人物皮肤Sprite的List：游戏时候
    /// </summary>
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();
    /// <summary>
    /// 钻石预制体
    /// </summary>
    public GameObject diamondPre;
    /// <summary>
    /// 普通的平台预制件
    /// </summary>
    public GameObject normalPlatformPre;
    /// <summary>
    /// 三个主题group预制体List
    /// </summary>
    public List<GameObject> platformCommonGroupPreList = new List<GameObject>();
    public List<GameObject> platformWinterGroupPreList = new List<GameObject>();
    public List<GameObject> platformGrassGroupPreList = new List<GameObject>();
    /// <summary>
    /// 左右两个钉子平台预制体
    /// </summary>
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    /// <summary>
    /// 玩家死亡特效预制体
    /// </summary>
    public GameObject deathEffect;

    /// <summary>
    /// 声音音效
    /// </summary>
    public AudioClip jumpClip, fallClip, hitClip, diamondClip, buttonClip;

    /// <summary>
    /// 声音按钮的sprite
    /// </summary>
    public Sprite musicButtonOn, musicButtonOff;

    /// <summary>
    /// 生成平台位置X和Y的相对偏移
    /// </summary>
    public float nextXPos = 0.554f, nextYPos = 0.645f;
}
