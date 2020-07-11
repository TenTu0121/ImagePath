using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    //单例模式
    public static ObjectPool Instance;

    private Transform objParent;
    /// <summary>
    /// 需要提前初始化的各个平台数量
    /// </summary>
    public int initSpawnCount = 5;
    //各个平台的对象池
    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> winterPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();
    /// <summary>
    /// 死亡特效对象池
    /// </summary>
    private List<GameObject> deathEffectList = new List<GameObject>();
    /// <summary>
    /// 钻石对象池
    /// </summary>
    private List<GameObject> diamondList = new List<GameObject>();
    /// <summary>
    /// 资源容器的引用
    /// </summary>
    private ManagerVars vars;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        Init();
    }
    /// <summary>
    /// 实例化对象池
    /// </summary>
    private void Init()
    {
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.platformCommonGroupPreList.Count; j++)
            {
                InstantiateObject(vars.platformCommonGroupPreList[j], ref commonPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.platformGrassGroupPreList.Count; j++)
            {
                InstantiateObject(vars.platformGrassGroupPreList[j], ref grassPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.platformWinterGroupPreList.Count; j++)
            {
                InstantiateObject(vars.platformWinterGroupPreList[j], ref winterPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.deathEffect, ref deathEffectList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.diamondPre, ref diamondList);
        }
    }
    /// <summary>
    /// 获取普通平台GameObject
    /// </summary>
    /// <returns>普通平台GameObject</returns>
    public GameObject GetNormalPlatform()
    {
        for (int i = 0; i < normalPlatformList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }
        return InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
    }
    /// <summary>
    /// 获取通用组合平台GameObject
    /// </summary>
    /// <returns>通用组合平台GameObject</returns>
    public GameObject GetCommonPlatform()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.platformCommonGroupPreList.Count);
        return InstantiateObject(vars.platformCommonGroupPreList[ran], ref commonPlatformList);
    }
    /// <summary>
    /// 获取Grass组合平台GameObject
    /// </summary>
    /// <returns>Grass组合平台GameObject</returns>
    public GameObject GetGrassPlatform()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.platformGrassGroupPreList.Count);
        return InstantiateObject(vars.platformGrassGroupPreList[ran], ref grassPlatformList);
    }
    /// <summary>
    /// 获取Winter组合平台GameObject
    /// </summary>
    /// <returns>Winter组合平台GameObject</returns>
    public GameObject GetWinterPlatform()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.platformWinterGroupPreList.Count);
        return InstantiateObject(vars.platformWinterGroupPreList[ran], ref winterPlatformList);
    }
    /// <summary>
    /// 获取左边钉子组合平台GameObject
    /// </summary>
    /// <returns>左边钉子组合平台GameObject</returns>
    public GameObject GetLeftSpikePlatform()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
    }
    /// <summary>
    /// 获取右边钉子组合平台GameObject
    /// </summary>
    /// <returns>右边钉子组合平台GameObject</returns>
    public GameObject GetRightSpikePlatform()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
    }
    /// <summary>
    /// 获取钻石GameObject
    /// </summary>
    /// <returns>钻石预制体</returns>
    public GameObject GetDiamondPre()
    {
        for (int i = 0; i < diamondList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (diamondList[i].activeInHierarchy == false)
            {
                return diamondList[i];
            }
        }
        return InstantiateObject(vars.diamondPre, ref diamondList);
    }
    /// <summary>
    /// 获取玩家死亡特效GameObject
    /// </summary>
    /// <returns>玩家死亡特效预制体</returns>
    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            //检查当前物体在场景中是否Active
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }
        return InstantiateObject(vars.deathEffect, ref deathEffectList);
    }

    /// <summary>
    /// 用于对象池的GameObject实例化
    /// </summary>
    /// <param name="prefab">需要实例化GameObject</param>
    /// <param name="addList">需要添加到的对象池List</param>
    /// <returns>加载的GameObject</returns>
    private GameObject InstantiateObject(GameObject prefab, ref List<GameObject> addList)
    {
        objParent = null;
        if (transform.Find(prefab.gameObject.name))
        {
            objParent = transform.Find(prefab.gameObject.name);
        }
        else
        {
            objParent = new GameObject(prefab.gameObject.name).transform;
            objParent.SetParent(transform);
        }
        GameObject go = Instantiate(prefab, objParent);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }
}
