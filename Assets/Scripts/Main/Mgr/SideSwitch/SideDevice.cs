using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SideDevice : MonoBehaviour
{
    public int sideNum;
    private GameObject _arts;

    public delegate void MapSideHandle(int state);    //-1：正常切换，0：初始状态隐藏，1：初始状态显示
    public delegate void ArtsSideHandle();

    public event MapSideHandle mapSideHandle;
    public event ArtsSideHandle artsSideHandle;

    public void Awake()
    {
        _arts = this.transform.FindChild("Arts").gameObject;
    }


    public void Start()
    {
        SideSwitchMgr.It.BindSideDev(this);

        if (sideNum == 0)
            return;
        _arts.SetActive(!_arts.activeInHierarchy);
    }

    public void MapSideSwitch()
    {
        if (mapSideHandle == null)
            return;
        mapSideHandle(-1);
    }

    public void ArtsSideSwitch()
    {
        if (artsSideHandle != null)
            artsSideHandle();
        _arts.SetActive(!_arts.activeInHierarchy);
    }

    public void InitState()
    {
        if (mapSideHandle == null)
            return;

        if (artsSideHandle != null)
            artsSideHandle();

        if (sideNum == 0)
        {
            _arts.SetActive(true);
            mapSideHandle(0);
        }
        else
        {
            _arts.SetActive(false);
            mapSideHandle(1);
        }
    }
}
