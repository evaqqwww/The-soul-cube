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

    //private int _curSide;
    //private int _nextSide;

    private SideSwitchRay[] _rays = new SideSwitchRay[4];

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

    public void SwitchBegin(BoundsBehavior behavior)
    {
#if sideSwitch
        switch (behavior)
        {
            case BoundsBehavior.up:
                _rays[2].SideSwitching();
                RaysLaunch(false);
                _rays[3].isLastSide = true;
                break;
            case BoundsBehavior.down:
                _rays[3].SideSwitching();
                RaysLaunch(false);
                _rays[2].isLastSide = true;
                break;
            case BoundsBehavior.left:
                _rays[0].SideSwitching();
                RaysLaunch();
                _rays[1].isLastSide = true;
                break;
            case BoundsBehavior.right:
                _rays[1].SideSwitching();
                RaysLaunch();
                _rays[0].isLastSide = true;
                break;
            default:
                break;
        }
#endif
    }

    //射线发射开关
    private void RaysLaunch(bool isHorizontal=true)
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


}
