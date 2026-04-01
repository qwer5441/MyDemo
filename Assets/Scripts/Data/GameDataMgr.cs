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

    //音乐音效相关数据
    public MusicData musicData;

    private GameDataMgr()
    {
        //初始化默认数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
    }
    /// <summary>
    /// 存储音乐音效
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
}
