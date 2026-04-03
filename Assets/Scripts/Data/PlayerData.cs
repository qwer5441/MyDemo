using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家数据
/// </summary>
public class PlayerData
{
    //当前拥有的钱
    public int haveMoney = 0;
    //当前解锁了哪些角色，存储id记录
    public List<int>buyHero=new List<int>();
}
