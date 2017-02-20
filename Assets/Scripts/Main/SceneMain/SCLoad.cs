using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SCLoad : MonoBehaviour
{


    public void Start()
    {
        UIMgr.It.InitCommonUIs();
        StartLoad();
    }


    private void StartLoad()
    {
        GlobalEvents.It.events.dispatchEvent(EventsDefine.LOADINGPANEL_SHOW);

    }
}
