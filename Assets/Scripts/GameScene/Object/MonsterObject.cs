using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 怪物表现行为：初始化数据，受伤，死亡
/// </summary>
public class MonsterObject : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private MonsterInfo monsterInfo;

    private int hp;
    public bool isDead=false;

    //上一次攻击的时间
    private float frontTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (isDead) return;

        //移动速度等于0就停下来
        animator.SetBool("Run", agent.velocity != Vector3.zero);

        //检测到目标点附近就攻击
        if (Vector3.Distance(transform.position, MainTowerObject.Instance.transform.position) < 5
                                 &&
            Time.time - frontTime > monsterInfo.atkOffset)
        {
            //记录这次攻击的时间
            frontTime = Time.time;
            animator.SetTrigger("Atk");
        }
    }
    //初始化动画控制器，血量，移动速度和旋转速度
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);

        hp=info.hp;

        agent.speed = agent.acceleration = info.moveSpeed;
        agent.angularSpeed=info.roundSpeed;

    }


    //受伤
    public void Wound(int dmg)
    {
        if (isDead) return;

        hp -= dmg;
        animator.SetTrigger("Wound");

        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            //播放受伤音效
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }
    //死亡
    public void Dead()
    {

        isDead = true;

        agent.enabled = false;
        
        animator.SetBool("Dead",true);

        GameDataMgr.Instance.PlaySound("Music/dead");

        //加钱
        GameLevelMgr.Instance.player.AddMoney(35);

    }

    //死亡动画播放完调用的事件办法
    public void DeadEvent() 
    {

        //从怪物列表中移除
        GameLevelMgr.Instance.RemoveMonster(this);

        //删除场景中死亡怪物对象
        Destroy(gameObject);

        //怪物死亡时检测是否胜利
        if (GameLevelMgr.Instance.CheckOver())
        {
            //游戏胜利
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(Random.Range(150,250), false);
        }

    }

    //出生后移动
    public void BornOver()
    {
        //出生后朝着目标移动
        agent.SetDestination(MainTowerObject.Instance.transform.position);

        //播放移动动画
        animator.SetBool("Run", true);
    }


    public void AtkEvent()
    {
        //伤害检测
        Collider[] colliders= Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("MainTower"));

        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Eat");

        for (int i = 0; i < colliders.Length; i++)
        {
            if (MainTowerObject.Instance.gameObject == colliders[i].gameObject)
            {
                //让保护区受伤
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }

    }
}
