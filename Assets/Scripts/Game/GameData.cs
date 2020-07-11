using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏数据类：存放游戏数据
/// </summary>
[System.Serializable]//序列化
public class GameData
{
    /// <summary>
    /// 
    /// </summary>
    public static bool IsAgainGame = false;

    //数据
    private bool isFirstGame;
    private bool isMusicOn;
    private bool[] skinUnlocked;
    //数据
    private int selectSkin;
    private int diamondCount;
    private int[] bestGameScoreArr;

    #region Set方法
    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }
    public void SetIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }
    public void SetSkinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
    }
    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }
    public void SetDiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }
    public void SetBestGameScoreArr(int[] bestGameScoreArr)
    {
        this.bestGameScoreArr = bestGameScoreArr;
    }
    #endregion

    #region Get方法
    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
    public bool[] GetSkinUnlocked()
    {
        return skinUnlocked;
    }
    public int GetSelectSkin()
    {
        return selectSkin;
    }
    public int GetDiamondCount()
    {
        return diamondCount;
    }
    public int[] GetBestGameScoreArr()
    {
        return bestGameScoreArr;
    }
    #endregion
}
