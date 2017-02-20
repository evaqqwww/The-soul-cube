using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SCMain : MonoBehaviour
{


    public void Start()
    {
        UIMgr.It.InitMainUIs();

        GameObject level = Resources.Load<GameObject>("Prefabs/LevelCube/Complete/" + SCApp.It.curlevelName);
        GameObject.Instantiate<GameObject>(level);
        InitScene();
       
    }

    private void InitScene()
    {
        GlobalEvents.It.events.dispatchEvent(EventsDefine.LOADINGPANEL_HIDE);

        GlobalEvents.It.events.dispatchEvent(EventsDefine.MAINPANEL_SHOW);
    }
}
