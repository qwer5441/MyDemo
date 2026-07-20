using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text txtWin;
    public Text txtMoney;

    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            //隐藏面板
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();

            //清空当前关卡数据
            GameLevelMgr.Instance.ClearInfo();

            //切换场景
            SceneManager.LoadScene("BeginScene");
        });
    }
    public void InitInfo(int money,bool isWin)
    {
        if (isWin)
        {
            txtWin.text = "胜利";
            txtWin.color = new Color(0.62f, 0.98f, 0f);
        }
        else
        {
            txtWin.text = "失败";
            txtWin.color = new Color(0.98f, 0f, 0f);

        }
        txtMoney.text ="￥"+ money;

        //保存数据
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }
    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}
