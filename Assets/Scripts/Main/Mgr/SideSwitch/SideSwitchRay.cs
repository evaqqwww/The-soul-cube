using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SideSwitchRay : MonoBehaviour
{

    private int _curSide = -1;

    public int CurSide
    {
        get
        {
            return _curSide;
        }
    }

    private float _raylength = 10.0f;

    private RaycastHit hitsStorage;

    private SideDevice _tempSideDev;
    private SideDevice _curSideDev;

    public LayerMask layerMask;

    public bool launching;

    public bool isPreSide;


    public void Start()
    {
        launching = true;

        hitsStorage = new RaycastHit();

    }

    public void InitRayParms()
    {


    }

    public void Update()
    {

        if (!LevelMgr.It.isSideSwitch)
            return;
        if (!launching)
            return;
        hitsStorage = SystemUtil.RayCast(transform.position, transform.forward, _raylength, layerMask, Color.black, true);
        if (!hitsStorage.collider)
            return;
        _tempSideDev = hitsStorage.collider.GetComponent<SideDevice>();
        int num = _tempSideDev.sideNum;

        if (_curSide != num)
        {
            launching = false;
            _curSide = num;
            _curSideDev = _tempSideDev;
            if (isPreSide)
                SideSwitching();
        }

    }


    public void SideSwitching(bool isPre = true)
    {
        if (isPre)
            isPreSide = false;

        _curSideDev.MapSideSwitch();
        _curSideDev.ArtsSideSwitch();
    }

}

