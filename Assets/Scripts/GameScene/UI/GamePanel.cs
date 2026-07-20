using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel :BasePanel
{
    public Image imgHp;
    public Text txtHp;
    public Text txtWave;
    public Text txtMoney;
    
    //默认宽
    public float hpW=500;

    public Button btnQuit;

    //塔组合的父对象 控制显隐
    public Transform botTrans;
    //3个复合控件 
    public List<TowerBtn>towerBtns=new List<TowerBtn>();

    //当前进入和选中的造塔点
    private TowerPoint nowSelTowerPoint;

    //用来标识 是否检测造塔输入的
    private bool checkInput;
    public override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            //隐藏游戏界面
            UIManager.Instance.HidePanel<GamePanel>();
            //返回到开始页面
            SceneManager.LoadScene("BeginScene");
        });
        //一开始隐藏下方和造塔相关的UI
        botTrans.gameObject.SetActive(false);

        //进入游戏锁定鼠标
        Cursor.lockState = CursorLockMode.Confined; 

    }
    /// <summary>
    /// 更新安全区域血量显示值
    /// </summary>
    /// <param name="hp">当前血量</param>
    /// <param name="maxHp">最大血量</param>
    public void UpdateTowerHp(int hp,int maxHp)
    {
        txtHp.text=hp+"/"+maxHp;
        //更新血条的长度
        (imgHp.transform as RectTransform).sizeDelta=new Vector2(((float)hp/maxHp)*hpW,44);
    }


    /// <summary>
    /// 更新剩余波数
    /// </summary>
    /// <param name="nowNum">当前波数</param>
    /// <param name="maxNum">最大波数</param>
    public void UpdateWaveNum(int nowNum,int maxNum)
    {
        txtWave.text=nowNum+"/"+maxNum;
    }

    
    /// <summary>
    /// 更新金币
    /// </summary>
    /// <param name="money">当前获得的金币</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text=money.ToString();
    }

    /// <summary>
    /// 更新当前选中造塔点 界面的变化
    /// </summary>
    /// <param name="point"></param>
    public void UpdateSelTower(TowerPoint point)
    {
        nowSelTowerPoint=point;
        //数据为空就隐藏下方造塔页面
        if (nowSelTowerPoint == null)
        {
            checkInput=false;
            botTrans.gameObject.SetActive(false );
        }
        else
        {
            checkInput = true;
            ///显示下方造塔按钮
            botTrans.gameObject.SetActive(true);

            //如果没有造过塔
            if (nowSelTowerPoint.nowTowerInfo == null)
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.ChooseIDs[i], "数字键" + (i + 1));
                }
            }
            //如果造过塔
            else
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);

                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLev, "空格键");

            }
        }
      
    }
    protected override void Update()
    {
        base.Update();

        if (!checkInput) return;
        
        //没造过塔 检测按钮造塔
        if(nowSelTowerPoint.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.ChooseIDs[0]);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.ChooseIDs[1]);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.ChooseIDs[2]);

            }

        }
        //造过 检测空格键升级
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.nextLev);
            }
        }
    }
}
