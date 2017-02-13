using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class VoidPlatform : MonoBehaviour
{

    public int type;
    public bool isVoid;
    private bool _enable;

    private List<MapSideSwitch> _nodes = new List<MapSideSwitch>();


    public void Awake()
    {
        
        this.GetComponentsInChildren<MapSideSwitch>(_nodes);
    }


    public void Start()
    {
        VoidPlatformMgr.It.BindPlatforms(type, this);
        if (_nodes.Count == 0)
            return;
        for (int i = 0; i < _nodes.Count; i++)
        {
#if !sideSwitch
            _nodes[i].InitVoidPlat(VoidPlatformMgr.It.GetColorByIndex(type), isVoid);
#endif
        }
    }


    public void SwitchVoid()
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
#if !sideSwitch
            _nodes[i].SwitchState();
#endif
        }
    }
}


