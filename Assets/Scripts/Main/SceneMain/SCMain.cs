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
        GlobalEvents.It.events.dispatchEvent(EventsDefine.LOADINGPANEL_HIDE);
        
        GlobalEvents.It.events.dispatchEvent(EventsDefine.MAINPANEL_SHOW);
    }
}
