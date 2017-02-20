using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


[NGUIPanel(PanelDefine.LEVELSELECTION)]
public class LevelSelectionPanel : NGUIBasePanel
{

    public LevelSelectionPanel()
        : base("Prefabs/DemoStandard/UI/LevelSelectionPanel", true)
    {

    }


    public override void Init()
    {
        base.Init();


        UIButtonEventObject _playBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorCenterT, "start");
        _playBtn.AddSingleClick(OnLevelStart, 1);
        

        UIButtonEventObject _returnBtn = SystemUtil.AddOrGetComponent<UIButtonEventObject>(this._anchorBottomRightT, "return");
        _returnBtn.AddSingleClick(OnBtnReturn);

    }


    protected override void DL_RegisterEvents()
    {
        BindHandle(true);

    }
    
    public override void RemoveFromStage()
    {
        base.RemoveFromStage();
        BindHandle(false);
    }

    void BindHandle(bool bBind)
    {
        bindEventListener(bBind, EventsDefine.LEVELSELECTION_SHOW, ToShow);
        bindEventListener(bBind, EventsDefine.LEVELSELECTION_HIDE, ToHide);
        bindEventListener(bBind, EventsDefine.LEVELTOLOAD, ToLoadLevel);
    }


    public override void OnShowing()
    {
        base.OnShowing();
    }


    public override void OnHidden()
    {
        base.OnHidden();
    }

    #region internal member function

    private void ToLoadLevel(int levelNum)
    {
        StringBuilder levelName = new StringBuilder("Level");
        levelName.Append(levelNum.ToString());
        SCApp.It.curlevelName = levelName.ToString();
        SCApp.It.levelNum = levelNum;

        SceneManager.LoadScene("Loading");
    }

    #endregion


    #region events handle

    private void ToShow(HEvent evt)
    {
        Show();
    }

    private void ToHide(HEvent evt)
    {
        Close();
    }

    private void ToLoadLevel(HEvent evt)
    {
        HEventWithParams es = evt as HEventWithParams;
        if (es.paras.Length > 0)
        {
            int index = (int)es.paras[0];
            ToLoadLevel(index);
        }
    }

    //关卡选择
    private void OnLevelStart(object[] ps)
    {
        int index = (int)ps[0];
        Hide();
        
        ToLoadLevel(index);
    }


    private void OnBtnReturn(object[] ps)
    {
        Hide();
        GlobalEvents.It.events.dispatchEvent(EventsDefine.LOGOPANEL_SHOW);
    }



    #endregion
}
