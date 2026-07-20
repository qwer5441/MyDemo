using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 全局关卡管理器管理
/// </summary>
public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    private GameLevelMgr() { }
    public PlayerObject player;

    //所有的出怪点
    private List<MonsterPoint> points = new List<MonsterPoint>();

    //记录还有多少波
    private int nowWaveNum = 0;
    //记录一共有多少波
    private int maxWaveNum = 0;


    //记录当前场景上怪物的列表 给炮台使用
    private List<MonsterObject> monsterList = new List<MonsterObject>();

    public void InitInfo(SceneInfo info)
    {
        //显示游戏界面
        UIManager.Instance.ShowPanel<GamePanel>();

        //玩家创建
        //先获取之前记录的当前选中的角色数据
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;

        //获取场景中玩家出生点位置
        Transform heroPos = GameObject.Find("HeroBornPos").transform;

        //创建玩家
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, heroPos.rotation);

        //对玩家进行初始化
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, info.money);

        //设置摄像机的看向
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化保护区域血量
        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);
    }
    /// <summary>
    /// 记录所有出怪点
    /// </summary>
    /// <param name="point"></param>
    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }

    public void UpdateMaxNum(int num) 
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;

        //更新界面
        UIManager.Instance.ShowPanel<GamePanel>().UpdateWaveNum(nowWaveNum,maxWaveNum);
    }
    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.ShowPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);

    }
    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        //出怪点
        for(int i = 0; i < points.Count; i++)
        {
            //只要有一个出怪点还没有出完怪就还没胜利
            if (!points[i].CheckOver()){
                return false;
            }
        }

        //怪物列表是否还存在怪物啊
        if(monsterList .Count> 0)
            return false;
        return true;
    }



    /// <summary>
    /// 记录怪物到怪物列表中
    /// </summary>
    /// <param name="obj"></param>
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }

    /// <summary>
    /// 将怪物移除列表 死亡时使用
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }


    /// <summary>
    /// 寻找满足条件的单个怪物
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos,int range)
    {
        //在怪物列表中寻找符合条件的怪物 返回去 用于被塔攻击
        for (int i = 0; i < monsterList.Count; i++)
        {
            //怪物没有死亡 && 在范围内
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                return monsterList[i];
            }
        }
        return null;
    }


    /// <summary>
    /// 寻找满足条件的所有怪物
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 pos,int range)
    {
        List<MonsterObject> list= new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;
    }
    /// <summary>
    /// 清空当前关卡记录的数据
    /// </summary>
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum=maxWaveNum=0;
        player=null;
    }
}
