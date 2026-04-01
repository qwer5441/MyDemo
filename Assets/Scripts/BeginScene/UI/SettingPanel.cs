using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        //初始化面板的内容
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn=data.musicOpen;
        togSound.isOn=data.soundOpen;

        sliderMusic.value = data.musicValue;
        sliderSound.value = data.soundValue;

        btnClose.onClick.AddListener(OnCloseClick);

        togMusic.onValueChanged.AddListener(OnMusicValueChanged);

        togSound.onValueChanged.AddListener(OnSoundValueChanged);

        sliderMusic.onValueChanged.AddListener(OnMusicSliderChanged);

        sliderSound.onValueChanged.AddListener(OnSoundSliderChanged);

    }
    // 关闭按钮点击
    void OnCloseClick()
    {
        //关闭面板前保存数据
        GameDataMgr.Instance.SaveMusicData();

        UIManager.Instance.HidePanel<SettingPanel>();
    }

    // 音乐开关变化
    void OnMusicValueChanged(bool v)
    {
        BKMusic.Instance.SetIsOpen(v);
        GameDataMgr.Instance.musicData.musicOpen = v;
    }

    // 音效开关变化
    void OnSoundValueChanged(bool v)
    {
        //记录音效开关数据
        GameDataMgr.Instance.musicData.soundOpen = v;
    }

    // 音乐音量滑块变化
    void OnMusicSliderChanged(float v)
    {
        //背景音乐改变
        BKMusic.Instance.ChangeValue(v);
        //记录音乐大小数据
        GameDataMgr.Instance.musicData.musicValue = v;

    }

    // 音效音量滑块变化
    void OnSoundSliderChanged(float v)
    {
        //记录音效大小的数据
        GameDataMgr.Instance.musicData.soundValue = v;
    }
}
