using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(OnStartClick);
        btnSetting.onClick.AddListener(OnSettingClick);
        btnAbout.onClick.AddListener(OnAboutClick);
        btnQuit.onClick.AddListener(OnQuitClick);
    }
    // 开始游戏按钮
    void OnStartClick()
    {
        Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>//传入左转后要做的事情
        {
            print("显示选色面板");
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });

        UIManager.Instance.HidePanel<BeginPanel>();    
    }

    // 设置按钮
    void OnSettingClick()
    {
        UIManager.Instance.ShowPanel<SettingPanel>();
    }

    // 关于按钮
    void OnAboutClick()
    {
       
    }

    // 退出游戏按钮
    void OnQuitClick()
    {
       
        Application.Quit();
        Debug.Log("退出游戏");
    }

}
