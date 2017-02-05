using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;


[NGUIPanel(PanelDefine.LOADINGPANEL)]
public class LoadingPanel : NGUIBasePanel
{
    
    public LoadingPanel()
        : base("Prefabs/DemoStandard/UI/LoadingPanel", true)
    {

    }


    public override void Init()
    {
        base.Init();


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
        bindEventListener(bBind, EventsDefine.LOADINGPANEL_SHOW, ToShow);
        bindEventListener(bBind, EventsDefine.LOADINGPANEL_HIDE, ToClose);
    }


    public override void OnShowing()
    {
        base.OnShowing();
    }


    public override void OnHidden()
    {
        base.OnHidden();
    }


    #region Internal member function

    private void AsynLoadScene(string scene)
    {
        CoroutineEngine.instance.StartCoroutine(StartLoading(scene));
    }

    private IEnumerator StartLoading(string scene)
    {

        int displayProgress = 0;

        int toProgress = 0;

        AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);

        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {

            toProgress = (int)op.progress * 100;

            if (toProgress == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            while (displayProgress < toProgress)
            {

                ++displayProgress;

                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;

        while (displayProgress < toProgress)
        {

            ++displayProgress;
            
            yield return new WaitForEndOfFrame();

        }
        
        op.allowSceneActivation = true;
    }

    #endregion


    #region events handle

    private void ToShow(HEvent evt)
    {
        if (evt == null)
            return;
        HEventWithParams hp = (HEventWithParams)evt;
        string scene = (string)hp.paras[0];

        Show();

        AsynLoadScene(scene);
    }

    private void ToClose(HEvent evt)
    {
        Close();
    }


    #endregion


}
