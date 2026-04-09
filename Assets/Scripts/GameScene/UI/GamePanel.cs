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
    public override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            //隐藏游戏界面
            UIManager.Instance.HidePanel<GamePanel>();
            //返回到开始页面
            SceneManager.LoadScene("BeginScene");
        });
        botTrans.gameObject.SetActive(false);
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
        (imgHp.transform as RectTransform).sizeDelta=new Vector2((float)hp/maxHp*hpW,44);
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
}
