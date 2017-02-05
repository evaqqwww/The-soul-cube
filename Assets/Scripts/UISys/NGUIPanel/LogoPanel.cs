using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[NGUIPanel(PanelDefine.LOGOPANEL)]
public class LogoPanel : NGUIBasePanel
{

    public LogoPanel()
        : base("Prefabs/DemoStandard/UI/LogoPanel")
    {

    }


    public override void Init()
    {
        base.Init();


        UIButtonEventObject _startBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorCenterT, "StartButton");
        _startBtn.AddSingleClick(OnBtnStart);
        UIButtonEventObject _quitBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorCenterT, "QuitButton");
        _quitBtn.AddSingleClick(OnBtnQuit);
        BindHandle(true);
    }

   


    protected override void DL_RegisterEvents()
    {
        
        
    }


    public override void RemoveFromStage()
    {
        base.RemoveFromStage();
        BindHandle(false);
    }

    void BindHandle(bool bBind)
    {
        bindEventListener(bBind, EventsDefine.LOGOPANEL_SHOW, ToShow);
        bindEventListener(bBind, EventsDefine.LOGOPANEL_HIDE, ToHide);
    }

    
    public override void OnShowing()
    {
        base.OnShowing();
    }


    public override void OnHidden()
    {
        base.OnHidden();
    }


    #region events handle

    private void ToShow(HEvent evt)
    {
        Show();
    }

    private void ToHide(HEvent evt)
    {
        Close();
    }

    private void OnBtnStart(object[] ps)
    {
        Hide();
        GlobalEvents.It.events.dispatchEvent(EventsDefine.LEVELSELECTION_SHOW);

    }

    private void OnBtnQuit(object[] ps)
    {
        Application.Quit();
    }

    

    #endregion
}
