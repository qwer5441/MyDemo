using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel :BasePanel
{
    //左右键
    public Button btnLeft;
    public Button btnRight;
    //购买按钮
    public Button btnUnLock;
    public Text txtUnLock;
    //开始和返回
    public Button btnStart;
    public Button btnBack;
    //左上角拥有的钱
    public Text txtMoney;
    //角色名
    public Text txtName;

    //角色需要创建的位置
    private Transform heroPos;

    //当前场景中显示的对象
    private GameObject heroObj;
    //当前使用的数据
    private RoleInfo nowRoleData;
    //当前使用数据的索引
    private int nowIndex;
    public override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;
        txtMoney.text =GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex < 0)
            {
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            }
            ChangeHero();

        });

        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if(nowIndex >= GameDataMgr.Instance.roleInfoList.Count)
            {
                nowIndex = 0;
            }
            ChangeHero();

        });

        btnUnLock.onClick.AddListener(() =>
        {
            PlayerData data=GameDataMgr.Instance.playerData;
            if (data.haveMoney >= nowRoleData.lockMoney)
            {
                data.haveMoney-= nowRoleData.lockMoney;
                txtMoney.text=data.haveMoney.ToString();
                //记录购买id
                data.buyHero.Add(nowRoleData.id);
                GameDataMgr.Instance.SavePlayerData();
                UpdateLockBtn();
                //购买成功提示
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
            }
            else
            {
                //显示金钱不足
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足");

            }
        });

        btnStart.onClick.AddListener(() =>
        {
            //记录选择的角色
            GameDataMgr.Instance.nowSelRole=nowRoleData;
            //隐藏自己 显示选择场景面板
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();

            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>//传入左转后要做的事情
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });

        });

        ChangeHero();

    }


    /// <summary>
    /// 更新场景上显示的模型
    /// </summary>
    private void ChangeHero()
    {
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
        //根据索引值取出一条数据
        nowRoleData=GameDataMgr.Instance.roleInfoList[nowIndex];
        txtName.text= nowRoleData.tips;
        //实例化模型 并记录下来 用于下次切换时删除
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);

        Destroy(heroObj.GetComponent<PlayerObject>());

        //根据解锁相关数据来决定是否显示解锁按钮
        UpdateLockBtn();
    }
    /// <summary>
    /// 更新解锁按钮显示情况
    /// </summary>
    private void UpdateLockBtn()
    {
        //需要解锁 && 没有解锁过
        //才显示解锁按钮 并且隐藏开始按钮
        if(nowRoleData.lockMoney > 0&&!GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id))
        {
            //更新解锁按钮显示 并更新上面的钱
            btnUnLock.gameObject.SetActive(true);
            txtUnLock.text = "￥" + nowRoleData.lockMoney;
            //隐藏开始按钮
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);

        }
    }
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        //每次隐藏自己时把当前显示的角色删除
        if(heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
