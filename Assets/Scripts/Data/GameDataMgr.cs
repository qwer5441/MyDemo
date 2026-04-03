using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 专门用来管理数据的类
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance=new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //记录选择的角色数据 用于之后在游戏场景中创建
    public RoleInfo nowSelRole;

    //音乐音效相关数据
    public MusicData musicData;

    //玩家相关数据
    public PlayerData playerData;
    //所有角色数据
    public List<RoleInfo> roleInfoList;

    private GameDataMgr()
    {
        //初始化默认数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //读取角色数据
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        //获取初始化玩家数据
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
    }
    /// <summary>
    /// 存储音乐音效
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
}
