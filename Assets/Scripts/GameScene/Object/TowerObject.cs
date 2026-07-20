 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    //炮台 看向敌人
    public Transform head;

    //开火点
    public Transform gunPoint;

    //旋转速度
    private float roundSpeed = 20;

    //炮塔要关联的基础数据
    private TowerInfo info;

    //当前要攻击的目标
    private MonsterObject targetObj;

    //当前要攻击的所有目标
    private List<MonsterObject> targetObjs;

    //计时 判断攻击间隔
    private float nowTime;

    //用于记录怪物位置
    private Vector3 monsterPos;

    /// <summary>
    /// 初始化炮台数据
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }
  
    // Update is called once per frame
    void Update()
    {
        //单体攻击炮台
        if (info.atkType == 1)
        {
            //没有目标  目标死亡  超出攻击范围
            //都需要寻找攻击目标
            if(targetObj == null ||
                targetObj.isDead ||
                Vector3.Distance(transform.position, targetObj.transform.position) > info.atkRange)
            {
                targetObj = GameLevelMgr.Instance.FindMonster(transform.position, info.atkRange);
            }

            if (targetObj == null) return;


            monsterPos = targetObj.transform.position;
            monsterPos.y=head.position.y;

            head.rotation=Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos-head.position),roundSpeed*Time.deltaTime);

            if(Vector3.Angle(head.forward,monsterPos-head.position)<5&&
                Time.time - nowTime >= info.offsetTime)
            {
                //让目标受伤
                targetObj.Wound(info.atk);

                //播放音效
                GameDataMgr.Instance.PlaySound("Music/Tower");

                //创建开火特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff),gunPoint.position,gunPoint.rotation);
                Destroy(effObj, 0.2f);
                nowTime = Time.time;
            }
        }
        //群体攻击
        else
        {
            targetObjs = GameLevelMgr.Instance.FindMonsters(transform.position,info.atkRange);
            if(targetObjs.Count>0&&
                Time.time - nowTime >= info.offsetTime)
            {
                //创建开火特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                Destroy(effObj, 0.2f);

                //让目标们受伤
                for (int i = 0; i < targetObjs.Count; i++) 
                {
                    targetObjs[i].Wound(info.atk);
                }

                nowTime = Time.time;
            }
        }
    }
}
