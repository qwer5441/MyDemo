using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private Animator animator;

    //角色攻击力
    private int atk;
    //拥有的钱
    public int money;
    //旋转的速度
    private float roundSpeed = 100;
    public float animSpeed = 1.3f;
    //持枪对象才有的开火点
    public Transform gunPoint;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
    }
    /// <summary>
    /// 初始化玩家属性
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="money"></param>
    public void InitPlayerInfo(int atk,int money )
    {
        this.atk = atk;
        this.money = money;

        UpdateMoney();
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));

        transform.Rotate(Vector3.up,Input.GetAxis("Mouse X")*roundSpeed*Time.deltaTime);
        animator.speed = animSpeed;

        //蹲 控制权重
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
            animator.SetTrigger("Roll");

        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("Fire");
    }

    /// <summary>
    /// 处理刀武器攻击动作的伤害检测事件
    /// </summary>
    public void KnifeEvent()
    {
        //伤害范围检测 
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));

        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Knife");

        for (int i = 0; i < colliders.Length; i++)
        {
            MonsterObject monster= colliders[i].gameObject.GetComponent<MonsterObject>();
            if (monster != null&&!monster.isDead)
            {
                monster.Wound(atk);
                break;
            }
        }
    }
    public void ShootEvent()
    {
        //射线检测
        RaycastHit[] hits= Physics.RaycastAll(new Ray(gunPoint.position,transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));

        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Gun");

        for (int i = 0; i < hits.Length; i++)
        {
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                //特效创建
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);

                Destroy(effObj,1);

                monster.Wound(atk);

                break;
            }
        }

    }
    public void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    public void AddMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
}
