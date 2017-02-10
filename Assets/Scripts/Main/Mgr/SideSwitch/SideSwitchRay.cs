using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SideSwitchRay : MonoBehaviour
{

    private int _curSide = -1;

    private float _raylength = 10.0f;

    private RaycastHit hitsStorage;

    private SideDevice _tempSideDev;
    private SideDevice _curSideDev;

    public LayerMask layerMask;

    public bool launching;

    public bool isLastSide;


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
#if sideSwitch
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
            if (isLastSide)
                SideSwitching();
        }
#endif

    }


    public void SideSwitching(bool _isLastSide = true)
    {
        if (_isLastSide)
            isLastSide = false;

        _curSideDev.MapSideSwitch();
        _curSideDev.ArtsSideSwitch();
    }
   
}

