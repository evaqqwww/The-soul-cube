using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*cube旋转中，当前面和目标面的画面控制，主要涉及前景和背景的转换*/
public class SideSwitchMgr
{

    private static SideSwitchMgr _it;

    public static SideSwitchMgr It
    {
        get
        {
            if (null == _it)
                _it = new SideSwitchMgr();
            return _it;
        }
    }

    public int curSide;
    //private int _nextSide;

    private SideSwitchRay[] _rays = new SideSwitchRay[4];
    private List<SideDevice> _sideDevs = new List<SideDevice>();


    public void Init(int mapBase)
    {
        Transform raysMgr = GameObject.FindGameObjectWithTag("sideSwitch").transform;
        if (!raysMgr)
            return;
        for (int i = 1; i < 5; i++)
        {
            _rays[i - 1] = raysMgr.FindChild(i.ToString()).GetComponent<SideSwitchRay>();
            Vector3 pos = _rays[i - 1].transform.position;
            _rays[i - 1].transform.position = new Vector3(pos.x, pos.y, mapBase);
        }

    }


    public void BindSideDev(SideDevice dev)
    {
        _sideDevs.Add(dev);
    }


    //初始化各个平面中元素状态
    public void InitSideState()
    {
        if (!LevelMgr.It.isSideSwitch)
            return;
        curSide = 0;
        if (_sideDevs.Count != 0)
        {
            for (int i = 0; i < _sideDevs.Count; i++)
            {
                _sideDevs[i].InitState();
            }
        }
    }


    public void SwitchBegin(BoundsBehavior behavior)
    {
        if (!LevelMgr.It.isSideSwitch)
            return;
        switch (behavior)
        {
            case BoundsBehavior.up:
                curSide = _rays[2].CurSide;
                _rays[2].SideSwitching();
                RaysLaunch(false);
                _rays[3].isPreSide = true;
                break;
            case BoundsBehavior.down:
                curSide = _rays[3].CurSide;
                _rays[3].SideSwitching();
                RaysLaunch(false);
                _rays[2].isPreSide = true;
                break;
            case BoundsBehavior.left:
                curSide = _rays[0].CurSide;
                _rays[0].SideSwitching();
                RaysLaunch();
                _rays[1].isPreSide = true;
                break;
            case BoundsBehavior.right:
                curSide = _rays[1].CurSide;
                _rays[1].SideSwitching();
                RaysLaunch();
                _rays[0].isPreSide = true;
                break;
            default:
                break;
        }
       
    }

    //射线发射开关
    private void RaysLaunch(bool isHorizontal = true)
    {
        if (isHorizontal)
        {
            _rays[0].launching = true;
            _rays[1].launching = true;
        }
        else
        {
            _rays[2].launching = true;
            _rays[3].launching = true;
        }
    }

    public void LevelWorld()
    {
        _sideDevs.Clear();
        curSide = 0;
    }

    public void InitRays()
    {
        for (int i = 0; i < _rays.Length; i++)
        {
            _rays[i].launching = true;
            _rays[i].isPreSide = false;
        }

    }


}
