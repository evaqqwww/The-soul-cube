using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

[NGUIPanel(PanelDefine.MAINPANEL)]
public class MainPanel : NGUIBasePanel
{

    
    
    public MainPanel()
        : base("Prefabs/DemoStandard/UI/MainPanel", true)
    {

    }


    public override void Init()
    {
        base.Init();

        UIButtonEventObject _leftBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorBottomLeftT, "left");
        _leftBtn.AddSinglePress(OnBtnLeft);
        UIButtonEventObject _rightBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorBottomLeftT, "right");
        _rightBtn.AddSinglePress(OnBtnRight);
        UIButtonEventObject _upBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorBottomRightT, "up");
        _upBtn.AddSingleClick(OnBtnJump);

        UIButtonEventObject _settingBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorTopRightT, "setting");
        _settingBtn.AddSingleClick(OnBtnSetting);
        UIButtonEventObject _playBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorCenterT, "play");
        _playBtn.AddSingleClick(OnBtnPlay);
        UIButtonEventObject _returnBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorCenterT, "return");
        _returnBtn.AddSingleClick(OnBtnReturn);

    }


    protected override void DL_RegisterEvents()
    {
        BindHandle(true);
    }


    public override void RemoveFromStage()
    {
        BindHandle(false);
        base.RemoveFromStage();
    }

    void BindHandle(bool bBind)
    {
        bindEventListener(bBind, EventsDefine.MAINPANEL_SHOW, ToShow);
        bindEventListener(bBind, EventsDefine.MAINPANEL_HIDE, ToClose);

    }

    #region internal member function 

    private void SettingSwitch()
    {
        this._anchorCenterT.gameObject.SetActive(!this._anchorCenterT.gameObject.activeInHierarchy);

    }

    #endregion

    #region events Handle


    private void ToShow(HEvent evt)
    {
        Show();
    }

    private void ToClose(HEvent evt)
    {
        Close();
    }

    private void OnBtnReturn(object[] ps)
    {
        SettingSwitch();
        Hide();
        Time.timeScale = 1;
        GlobalEvents.It.events.dispatchEvent(EventsDefine.LEVELSELECTION_SHOW);
    }

    private void OnBtnPlay(object[] ps)
    {
        Time.timeScale = 1;
        SettingSwitch();
    }

    private void OnBtnSetting(object[] ps)
    {
        SettingSwitch();
        if (Time.timeScale == 1)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    private void OnBtnJump(object[] ps)
    {
        LevelMgr.It.Player.MobileJump();
        
    }

    private void OnBtnRight(object[] ps)
    {

        CrossPlatformInputManager.It.Update(1);
        
    }

    private void OnBtnLeft(object[] ps)
    {
        CrossPlatformInputManager.It.Update(-1);
    }

    #endregion 

}
