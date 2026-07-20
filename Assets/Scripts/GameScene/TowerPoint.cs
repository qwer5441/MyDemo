using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    //造塔点关联的塔对象
    private GameObject towerObj=null;
    //造塔点关联的塔的数据
    public TowerInfo nowTowerInfo=null;
    //可以建造的三个塔的id
    public List<int> ChooseIDs;

    /// <summary>
    /// 建塔
    /// </summary>
    /// <param name="id"></param>
    public void CreateTower(int id)
    {
        TowerInfo info=GameDataMgr.Instance.towerInfoList[id-1];

        //钱不够 就不用建造
        if (info.money > GameLevelMgr.Instance.player.money) return;

        //扣钱
        GameLevelMgr.Instance.player.AddMoney(-info.money);

        //创建塔
        //先判断之前是否有塔，有就删除再建
        if (towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }

        //实例化塔
        towerObj=Instantiate(Resources.Load<GameObject>(info.res),transform.position,Quaternion.identity);

        //初始化塔
        towerObj.GetComponent<TowerObject>().InitInfo(info);

        //记录当前塔的数据
        nowTowerInfo = info;

        //塔建造完毕 更新游戏页面上的内容
        if (nowTowerInfo.nextLev != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //有塔并且等级满了
        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0) return;

        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        
    }
    private void OnTriggerExit(Collider other)
    {
        //离开造塔点隐藏造塔页面
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
