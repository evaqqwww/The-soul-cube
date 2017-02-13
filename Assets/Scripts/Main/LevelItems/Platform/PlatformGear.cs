using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlatformGear : MonoBehaviour
{
    //状态
    //private bool _left;
    private Platform _platform;
    //当前面
    private Transform _cubeSide;
    private BoxCollider _collider;
    //初始缩放
    private Vector3 _scale;

    public void Awake()
    {
        _cubeSide = transform.parent.parent;
        _platform = transform.parent.GetComponent<Platform>();
        _collider = transform.GetComponent<BoxCollider>();
    }


    public void Start()
    {
        transform.parent = _cubeSide;
        _scale = transform.localScale;
        LevelMgr.It.turnSideGlobalHandle += SwitchCollider;
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag.Equals("player"))
        {
            //transform.localScale = new Vector3(_left ? _scale.x : -_scale.x, _scale.y, _scale.z);
            //_left = !_left;
            if (other.transform.up.y == transform.up.y && LevelMgr.It.Player.falling)
            {
                LevelMgr.It.Player.StoreState();
                _platform.StartAni();
            }
        }
            
    }

    private void SwitchCollider()
    {
        _collider.enabled = !_collider.enabled;
    }


}
