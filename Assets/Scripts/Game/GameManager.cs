using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// 单例（Awake函数里面进行赋值了）
    /// </summary>
    public static GameManager Instance;

    /// <summary>
    /// 是否GameStart
    /// </summary>
    public bool isGameStarted { get; set; }
    /// <summary>
    /// 是否GameOver
    /// </summary>
    public bool isGameOvered { get; set; }
    /// <summary>
    /// 是否Pause
    /// </summary>
    public bool isGamePaused { get; set; }
    /// <summary>
    /// 玩家是否开始移动
    /// </summary>
    public bool isPlayerMoved { get; set; }

    private ManagerVars vars;
    /// <summary>
    /// 游戏Data
    /// </summary>
    private GameData gameData;
    //数据
    private bool isFirstGame;
    private bool isMusicOn;
    private bool[] skinUnlocked;
    //数据
    private int selectSkin;
    private int diamondCount;
    private int[] bestGameScoreArr;

    /// <summary>
    /// 游戏成绩
    /// </summary>
    private int gameScore;
    /// <summary>
    /// 钻石个数
    /// </summary>
    private int gameDiamond;
    /// <summary>
    /// 音效播放器
    /// </summary>
    private AudioSource mAudioSource;
    private void Awake()
    {
        Instance = this;//单例赋值
        vars = ManagerVars.GetManagerVars();
        mAudioSource = transform.GetComponent<AudioSource>();
        EventCenter.AddListener(EventType.AddScore, AddGameScore);
        EventCenter.AddListener(EventType.AddDiamond, AddGameDiamond);
        EventCenter.AddListener(EventType.PlayerMoved, PlayerMoved);
        EventCenter.AddListener(EventType.PlayButtonClip, PlayButtonClip);
        if (GameData.IsAgainGame)
        {
            isGameStarted = true;
        }
        InitGame();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventType.AddDiamond, AddGameDiamond);
        EventCenter.RemoveListener(EventType.PlayerMoved, PlayerMoved);
        EventCenter.RemoveListener(EventType.PlayButtonClip, PlayButtonClip);

    }

    #region Save and Read
    /// <summary>
    /// 保存本地游戏数据
    /// </summary>
    private void Save()
    {
        //写入写出数据，最好使用try？
        try
        {
            //定义一个用来序列化的BinaryFormatter，需要引入命名空间：using System.Runtime.Serialization.Formatters.Binary;
            BinaryFormatter bf = new BinaryFormatter();
            //using会自动释放数据流，不使用即需要手动释放：fs.Close
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                //设置写入数据类                
                gameData.SetIsFirstGame(isFirstGame);
                gameData.SetIsMusicOn(isMusicOn);
                gameData.SetSkinUnlocked(skinUnlocked);
                gameData.SetDiamondCount(diamondCount);
                gameData.SetSelectSkin(selectSkin);
                gameData.SetBestGameScoreArr(bestGameScoreArr);
                //序列化写入数据类，保存到本地文件fs中
                bf.Serialize(fs, gameData);
            }
        }
        catch (Exception e)
        {
            //报错
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// 读取本地游戏数据
    /// </summary>
    private void Read()
    {
        //写入写出数据，最好使用try？
        try
        {
            //定义一个用来序列化的BinaryFormatter，需要引入命名空间：using System.Runtime.Serialization.Formatters.Binary;
            BinaryFormatter bf = new BinaryFormatter();
            //using会自动释放数据流，不使用即需要手动释放：fs.Close
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data",FileMode.Open))
            {
                //反序列化读取到的本地文件fs，并赋值给gameData
                gameData = (GameData)bf.Deserialize(fs);
            }
        }
        catch (Exception e)
        {
            //报错
            Debug.Log(e.Message);
        }
    }
    #endregion

    /// <summary>
    /// 初始化游戏
    /// </summary>
    private void InitGame()
    {
        //读取本地数据
        Read();
        //本地数据不为空：第一次游戏的值直接读取
        if (gameData != null)
        {
            isFirstGame = gameData.GetIsFirstGame();
        }
        //本地数据为空：第一次游戏的值true
        else
        {
            isFirstGame = true;
        }
        //第一次游戏：手动设置一下
        if (isFirstGame)
        {
            //如果不设置成false，那么每次开始游戏都是第一次
            isFirstGame = false;
            isMusicOn = true;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            selectSkin = 0;
            bestGameScoreArr = new int[3];

            //保存之前，数据类gameData需要new出来，不然Save方法会报错
            gameData = new GameData();
            Save();
        }
        //不是第一次游戏，读取即可
        else
        {
            isMusicOn = gameData.GetIsMusicOn();
            skinUnlocked = gameData.GetSkinUnlocked();
            diamondCount = gameData.GetDiamondCount();
            selectSkin = gameData.GetSelectSkin();
            bestGameScoreArr = gameData.GetBestGameScoreArr();
        }
    }

    /// <summary>
    /// 记录最好的成绩
    /// </summary>
    /// <param name="score">需要比较的分数</param>
    public void SaveScore(int score)
    {
        //Tolist需要引入命名空间System.linq
        List<int> list = bestGameScoreArr.ToList();
        //排序（-x表示用-x与y比）== 从大到小排序
        list.Sort((x, y) => (-x.CompareTo(y)));
        //返回来
        bestGameScoreArr = list.ToArray();

        int index = -1;//标记分数
        for (int i = 0; i < bestGameScoreArr.Length; i++)
        {
            //如果传入的分数比当前位数大，则标记index为当前的下标
            if (score > bestGameScoreArr[i])
            {
                index = i;
            }
        }
        //没换到index的话，则不用换
        if (index == -1) return;
        //否则得判断下
        //从后面往前判断，在标记下标处开始，把后面的分数用前面代替，最前面被标记的数用传入的分数代替即可，保存一下
        //从后面往前判断，在标记下标处开始
        for (int i = bestGameScoreArr.Length - 1; i < index; i++)
        {
            //把后面的分数用前面代替
            bestGameScoreArr[i] = bestGameScoreArr[i - 1];
        }
        //最前面被标记的数用传入的分数代替即可
        bestGameScoreArr[index] = score;
        //保存一下
        Save();
    }
    /// <summary>
    /// 获取最高分数
    /// </summary>
    public int GetBestScore()
    {
        //返回最高的成绩分数即可
        return bestGameScoreArr.Max();
    }
    /// <summary>
    /// 获取最高分数数组
    /// </summary>
    public int[] GetBestScoreArr()
    {
        //Tolist需要引入命名空间System.linq
        List<int> list = bestGameScoreArr.ToList();
        //排序（-x表示用-x与y比）== 从大到小排序
        list.Sort((x, y) => (-x.CompareTo(y)));
        //返回来
        bestGameScoreArr = list.ToArray();

        //排序完成后，返回成绩分数数组即可
        return bestGameScoreArr;
    }

    /// <summary>
    /// 增加游戏分数
    /// </summary>
    private void AddGameScore()
    {
        if (isGameStarted == false || isGameOvered || isGamePaused)
            return;
        gameScore++;
        EventCenter.BroadCast(EventType.UpdateScoreText, gameScore);
    }
    /// <summary>
    /// 增加钻石个数
    /// </summary>
    private void AddGameDiamond()
    {
        if (isGameStarted == false || isGameOvered || isGamePaused)
            return;
        gameDiamond++;
        EventCenter.BroadCast(EventType.UpdateDiamondText, gameDiamond);
    }
    /// <summary>
    /// 设置解锁的皮肤
    /// </summary>
    public void SetUnlockSkin(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }
    /// <summary>
    /// 设置当前选择的皮肤下标
    /// </summary>
    public void SetSelectSkin(int index)
    {
        selectSkin = index;
        Save();
    }
    /// <summary>
    /// 设置当前的声音开关
    /// </summary>
    public void SetIsMusicOn(bool value)
    {
        isMusicOn = value;
        Save();
    }

    /// <summary>
    /// 获取当前的声音开关
    /// </summary>
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
    /// <summary>
    /// 获取当前选择的皮肤下标
    /// </summary>
    public int GetSelectSkin()
    {
        return selectSkin;
    }
    /// <summary>
    /// 获取解锁的皮肤
    /// </summary>
    public bool GetUnlockSkin(int index)
    {
        return skinUnlocked[index];
    }
    /// <summary>
    /// 获取当前游戏分数
    /// </summary>
    /// <returns>当前游戏分数</returns>
    public int GetGameScore()
    {
        return gameScore;
    }
    /// <summary>
    /// 获取当前钻石个数
    /// </summary>
    /// <returns>当前钻石个数</returns>
    public int GetGameDiamond()
    {
        return gameDiamond;
    }
    /// <summary>
    /// 获取全部钻石个数
    /// </summary>
    /// <returns>全部钻石个数</returns>
    public int GetAllDiamond()
    {
        return diamondCount;
    }
    /// <summary>
    /// 更新全部钻石个数
    /// </summary>
    public void UpdateAllDiamond(int value)
    {
        diamondCount += value;
        Save();
    }
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        //r如果不设置成false，那么每次开始游戏都是第一次
        isFirstGame = false;
        isMusicOn = true;
        skinUnlocked = new bool[vars.skinSpriteList.Count];//new 默认设置为false
        skinUnlocked[0] = true;
        diamondCount = 10;
        selectSkin = 0;
        bestGameScoreArr = new int[3];

        Save();
    }
    /// <summary>
    /// 播放Button音效
    /// </summary>
    private void PlayButtonClip()
    {
        AudioPlay(vars.buttonClip);
    }
    /// <summary>
    /// 音效播放
    /// </summary>
    /// <param name="clip">需要播放的clip</param>
    public void AudioPlay(AudioClip clip)
    {
        if (isMusicOn)
        {
            mAudioSource.PlayOneShot(clip);
        }
    }
    /// <summary>
    /// 玩家已经开始移动，isPlayerMoved设置为true
    /// 平台掉落计时器会检测此值
    /// </summary>
    private void PlayerMoved()
    {
        isPlayerMoved = true;
    }
}
