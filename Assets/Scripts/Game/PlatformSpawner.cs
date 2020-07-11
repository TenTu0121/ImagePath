using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlatformGroupType
{
    /// <summary>
    /// 冬季主题
    /// </summary>
    Winter,
    /// <summary>
    /// 草地主题
    /// </summary>
    Grass,
}

public class PlatformSpawner : MonoBehaviour {

    [Header("About FallTime & 里程碑")]
    /// <summary>
    /// 里程碑的分数
    /// </summary>
    public int mileStoneCount;
    /// <summary>
    /// 初始的掉落时间
    /// </summary>
    public float fallTime;
    /// <summary>
    /// 最小的掉落时间
    /// </summary>
    public float minFallTime;
    /// <summary>
    /// 掉落时间的倍数
    /// </summary>
    public float multipleFallTime;

    [Header("开始生成平台的位置")]
    /// <summary>
    /// 开始生成平台的位置
    /// </summary>
    public Vector3 startSpawnPos;
    /// <summary>
    /// 平台生成数量
    /// </summary>
    private int spawnPlatformCount;
    /// <summary>
    /// 平台的生成位置
    /// </summary>
    private Vector3 platformSpawnPosition;
    /// <summary>
    /// 当前选择的平台Sprite
    /// </summary>
    private Sprite sceletPlatformSprite;
    /// <summary>
    /// 平台的group主题
    /// </summary>
    private PlatformGroupType themeGroupType;
    /// <summary>
    /// 是否向左边生成，反之向右
    /// </summary>
    private bool isLeftSpawn = false;

    /// <summary>
    /// 是否生成钉子平台
    /// </summary>
    private bool isSpikeSpawn;
    /// <summary>
    /// 钉子平台是否向左边生成，反之向右
    /// </summary>
    private bool isSpikeSpawnLeft = false;
    /// <summary>
    /// 钉子平台方向的平台生成位置
    /// </summary>
    private Vector3 spikePlatformPos;
    /// <summary>
    /// 需要在钉子平台方向生成的平台数量
    /// </summary>
    private int afterSpawnSpikePlatformCount;

    /// <summary>
    /// 资源容器的引用
    /// </summary>
    private ManagerVars vars;

    private void Awake()
    {
        EventCenter.AddListener(EventType.DecidePath, DecidePath);//注册（监听）生成路径的事件码
        vars = ManagerVars.GetManagerVars();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.DecidePath, DecidePath);//注销（移除）生成路径的事件码
    }
    private void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawnPos;
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }

        //生成人物
        GameObject go = Instantiate(vars.characterPre);
        go.transform.position = new Vector3(0, -1.8f, 0);
	}
    private void Update()
    {
        if (GameManager.Instance.isGameStarted && GameManager.Instance.isGameOvered == false)
        {
            UpdateFallTime();
        }
    }
    /// <summary>
    /// 更新平台掉落时间
    /// </summary>
    private void UpdateFallTime()
    {
        //里程碑设置，到分数到达一定时间时，掉落的速度会适当加快
        if (GameManager.Instance.GetGameScore() > mileStoneCount)
        {
            mileStoneCount *= 2;
            fallTime *= multipleFallTime;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if (isSpikeSpawn)
        {
            AfterSpawnSpikePlatform();//包括了钉子平台方向和原来方向的平台生成
            return;
        }
        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            //反转平台生成的方向
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);//生成平台的数量需要重新指定，因为到这就为零了
            SpawnPlatform();
        }
    }
    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        //用于决定陷阱生成的方向：0-右边；else-左边
        int ranDirValue = Random.Range(0, 2);
        //需要生成的数量大于0：一般生成普通平台
        if (spawnPlatformCount > 0)
        {
            SpawnNormalPlatform(ranDirValue);
        }
        //需要生成的数量等于0：即是最后生成的一个平台：一般为陷阱平台
        else if (spawnPlatformCount == 0)
        {
            //随机一个数，用于决定生成哪个陷阱主题Group
            //0：生成通用主题平台
            //1：生成对应主题平台
            //else：生成钉子主题
            int ran = Random.Range(0, 3);
            //生成通用主题平台
            if (ran == 0)
            {
                SpawnCommonPlatformGroup(ranDirValue);
            }
            //生成对应主题平台
            else if (ran == 1)
            {
                switch (themeGroupType)
                {
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranDirValue);
                        break;
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranDirValue);
                        break;
                    default:
                        break;
                }
            }
            //生成钉子平台
            else
            {
                int spikeDirValue = -1;
                //向左生成平台，需要生成右边的钉子
                if (isLeftSpawn)
                {
                    spikeDirValue = 0;//在右边生成钉子平台
                }
                //向右生成平台，需要生成左边的钉子
                else
                {
                    spikeDirValue = 1;//在左边生成钉子平台
                }
                //根据传入的value来决定生成哪一边的钉子平台
                SpawnSpikePlatform(spikeDirValue);

                isSpikeSpawn = true;//生成钉子平台：true
                afterSpawnSpikePlatformCount = 4;//在钉子平台方向在生成4个平台
                if (isSpikeSpawnLeft)//钉子平台生成在左边
                {
                    spikePlatformPos = new Vector3(platformSpawnPosition.x - 1.65f,
                        platformSpawnPosition.y + vars.nextYPos,
                        platformSpawnPosition.z);
                }
                else//钉子平台生成在右边
                {
                    spikePlatformPos = new Vector3(platformSpawnPosition.x + 1.65f,
                        platformSpawnPosition.y + vars.nextYPos,
                        platformSpawnPosition.z);
                }
            }
        }
        //随机生成钻石
        SpawnDiamond();
        //向左生成
        if (isLeftSpawn)
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                platformSpawnPosition.y + vars.nextYPos,
                platformSpawnPosition.z);
        }
        else //向右生成
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                platformSpawnPosition.y + vars.nextYPos,
                platformSpawnPosition.z);
        }
    }
    /// <summary>
    /// 随机Group主题
    /// </summary>
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformSpriteList.Count);
        sceletPlatformSprite = vars.platformSpriteList[ran];
        //ran == 2:平台Sprite == ice == Type：Winter
        if (ran == 2)
        {
            themeGroupType = PlatformGroupType.Winter;
        }
        //其他：Grass
        else
        {
            themeGroupType = PlatformGroupType.Grass;
        }
    }
    /// <summary>
    /// 生成普通平台（单个）
    /// </summary>
    private void SpawnNormalPlatform(int DirValue)
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();//生成在当前的物体下面
        go.transform.position = platformSpawnPosition;//设置成需要生成平台的位置
        go.GetComponent<Platform>().Init(sceletPlatformSprite, fallTime, DirValue);//调用修改平台的Sprite
        go.SetActive(true);
    }
    /// <summary>
    /// 随机生成钻石
    /// </summary>
    private void SpawnDiamond()
    {
        int ran = Random.Range(0, 10);
        if (ran == 6 && GameManager.Instance.isPlayerMoved)
        {
            GameObject go = ObjectPool.Instance.GetDiamondPre();//生成在当前的物体下面
            go.transform.position = new Vector3(platformSpawnPosition.x,
                platformSpawnPosition.y + 0.5f,
                0);//设置成需要生成钻石的位置
            go.SetActive(true);
        }
    }
    /// <summary>
    /// 生成组合平台（CommonGroup）
    /// </summary>
    private void SpawnCommonPlatformGroup(int DirValue)
    {
        GameObject go = ObjectPool.Instance.GetCommonPlatform();//生成在当前的物体下面
        go.transform.position = platformSpawnPosition;//设置成需要生成平台的位置
        go.GetComponent<Platform>().Init(sceletPlatformSprite, fallTime, DirValue);//调用修改平台的Sprite
        go.SetActive(true);
    }
    /// <summary>
    /// 生成组合平台（GrassGroup）
    /// </summary>
    private void SpawnGrassPlatformGroup(int DirValue)
    {
        GameObject go = ObjectPool.Instance.GetGrassPlatform();//生成在当前的物体下面
        go.transform.position = platformSpawnPosition;//设置成需要生成平台的位置
        go.GetComponent<Platform>().Init(sceletPlatformSprite, fallTime, DirValue);//调用修改平台的Sprite
        go.SetActive(true);
    }
    /// <summary>
    /// 生成组合平台（WinterGroup）
    /// </summary>
    private void SpawnWinterPlatformGroup(int DirValue)
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatform();//生成在当前的物体下面
        go.transform.position = platformSpawnPosition;//设置成需要生成平台的位置
        go.GetComponent<Platform>().Init(sceletPlatformSprite, fallTime, DirValue);//调用修改平台的Sprite
        go.SetActive(true);
    }
    /// <summary>
    /// 生成钉子平台（Spike：分左右）
    /// </summary>
    /// <param name="value">0：生成右边；1：生成左边</param>
    private void SpawnSpikePlatform(int DirValue)
    {
        GameObject temp = null;//用于存放需要生成的钉子平台
        if (DirValue == 0)
        {
            isSpikeSpawnLeft = false;//钉子平台生成在右边
            temp = ObjectPool.Instance.GetRightSpikePlatform();//生成右边的钉子平台
        }
        else
        {
            isSpikeSpawnLeft = true;//钉子平台生成在左边
            temp = ObjectPool.Instance.GetLeftSpikePlatform();//生成左边的钉子平台
        }
        temp.transform.position = platformSpawnPosition;//设置成需要生成平台的位置
        temp.GetComponent<Platform>().Init(sceletPlatformSprite, fallTime, DirValue);//调用修改平台的Sprite
        temp.SetActive(true);
    }
    /// <summary>
    /// 在钉子平台方向继续生成平台
    /// 当然也需要在原来方向继续生成平台
    /// </summary>
    private void AfterSpawnSpikePlatform()
    {
        if (afterSpawnSpikePlatformCount > 0)
        {
            afterSpawnSpikePlatformCount--;
            //0:生成原来方向的平台；1：生成钉子方向的平台
            for (int i = 0; i < 2; i++)
            {
                GameObject temp = ObjectPool.Instance.GetNormalPlatform();
                //生成原来方向平台
                if (i == 0)
                {
                    temp.transform.position = platformSpawnPosition;
                    //如果在钉子在左边，那么原先方向就在右边
                    if (isSpikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos,
                            platformSpawnPosition.z);
                    }
                    else//反之，在左边
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos,
                            platformSpawnPosition.z);
                    }
                }
                //生成钉子方向
                else
                {
                    temp.transform.position = spikePlatformPos;
                    //钉子在左边，平台就生成在左边：-
                    if (isSpikeSpawnLeft)
                    {
                        spikePlatformPos = new Vector3(spikePlatformPos.x - vars.nextXPos,
                            spikePlatformPos.y + vars.nextYPos,
                            spikePlatformPos.z);
                    }
                    else//反之在右边：+
                    {
                        spikePlatformPos = new Vector3(spikePlatformPos.x + vars.nextXPos,
                            spikePlatformPos.y + vars.nextYPos,
                            spikePlatformPos.z);
                    }
                }

                temp.GetComponent<Platform>().Init(sceletPlatformSprite, fallTime, 1);//调用修改平台的Sprite
                temp.SetActive(true);
            }
        }
        else
        {
            isSpikeSpawn = false;
            DecidePath();
        }
    }
}