using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 怪物出怪点
/// </summary>
public class MonsterPoint : MonoBehaviour
{
    //波数
    public int maxWave;

    //每波怪物只数
    public int monsterNumOneWave;

    //记录当前波的怪物只数
    private int nowNum;

    //怪物ID
    public List<int> monsterIDs;

    //当前波创建的ID怪物
    private int nowID;

    //单只怪物创建间隔时间
    public float createOffsetTime;

    //波数之间的间隔时间
    public float delayTime;

    //首波怪物创建的时间
    public float firstDelayTime;

    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
        //记录出怪点
        GameLevelMgr.Instance.AddMonsterPoint(this);
        //更新最大波数
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);
    }
    /// <summary>
    /// 创建一波
    /// </summary>
    private void CreateWave()
    {
        //得到当前波怪物的ID
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        //当前波怪物创建多少只
        nowNum = monsterNumOneWave;

        //创建怪物
        CreateMonster();
        
        //减少波数
        --maxWave;

        GameLevelMgr.Instance.ChangeNowWaveNum(1);

    }
    /// <summary>
    /// 创建怪物
    /// </summary>
    private void CreateMonster()
    {
        //取出怪物数据
        MonsterInfo info=GameDataMgr.Instance.monsterInfoList[nowID-1];

        //创建单只怪物预制体
        GameObject obj= Instantiate(Resources.Load<GameObject>(info.res), transform.position, Quaternion.identity);

        //添加怪物脚本
        MonsterObject monsterObj= obj.AddComponent<MonsterObject>();
        //初始化怪物数据
        monsterObj.InitInfo(info);


        //记录怪物到列表中  
        GameLevelMgr.Instance.AddMonster(monsterObj);

        //要创建怪物的只数减一
        --nowNum;

        if (nowNum == 0)
        {
            if(maxWave > 0)
            {
                Invoke("CreateWave", delayTime);

            }
        }
        else 
        {
            Invoke("CreateMonster", createOffsetTime);

        }
    }
    /// <summary>
    /// 出怪点是否出怪结束
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0&&maxWave==0;
    }
}
