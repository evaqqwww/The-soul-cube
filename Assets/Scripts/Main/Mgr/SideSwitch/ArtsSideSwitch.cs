using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ArtsSideSwitch : MonoBehaviour
{

    private SideDevice _sideDev;

    //所属平面
    private int _theSide;

    public void Awake()
    {
        _sideDev = transform.parent.parent.GetComponent<SideDevice>();
        _theSide = _sideDev.sideNum;
    }


    void Start()
    {
        if (!LevelMgr.It.isSideSwitch)
            return;
        if (this.gameObject.layer != 12)
            return;
        SwitchSide();
        _sideDev.artsSideHandle += SwitchSide;

    }

    private void SwitchSide()
    {
        if (_theSide == SideSwitchMgr.It.curSide)
            this.gameObject.layer = 12;
        else
            this.gameObject.layer = 8;
    }


}
