using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class Pointer:MonoBehaviour
{

    //正确的位置
    public Vector3 correctPos;
    //初始角
    private Vector3 _initPos;
    //初始缩放
    private Vector3 _scale;
    //运动中
    private bool _isAni;

    private Transform _parent;

    private Transform TransParent
    {
        get
        {
            if (_parent == null)
                _parent = transform.parent;
            return _parent;
        }
    }

    private Clock _clock;

    private BoxCollider _boxCollider;

    //是否为顺时针运动
    private bool _movementalong;

    void Awake()
    {
        _clock = transform.parent.GetComponent<Clock>();
        _boxCollider = transform.GetComponent<BoxCollider>();
    }
    
    public void Start()
    {
        _initPos = transform.localRotation.eulerAngles;
        _scale = transform.localScale;
        if (!transform.name.Equals("1"))
        {
            correctPos = new Vector3(0, 0, 90.0f);
        }
        else if (_initPos.z > 180.0f)
        {
            correctPos = new Vector3(0, 0, 360f);
        }
        else
            correctPos = Vector3.zero;

        _clock.stopClock += SwitchCollider;
        LevelMgr.It.turnSideGlobalHandle += SwitchCollider;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("player"))
        {
            if (_isAni)
                return;
            PlayerDirBySelf(other.transform.position);
            //transform.Rotate(Vector3.forward, TurnAngles(), Space.World);
            float target;
            if (TurnAngles(out target))
            {
                _isAni = true;
                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, target), 0.2f, RotateMode.Fast).OnComplete(RotatingComplete);
            }
            else
            {
                transform.parent = TransParent;
                _isAni = true;
                transform.DOShakeRotation(0.3f, new Vector3(20.0f, 20.0f, 0)).OnComplete(ShakeCompleted);
            }
        }
    }


    private void SwitchCollider()
    {
        _boxCollider.enabled = !_boxCollider.enabled;
    }

    private void RotatingComplete()
    {
        transform.parent = TransParent;
        _isAni = false;
    }

    private void ShakeCompleted()
    {
        transform.localScale = _scale;
        _isAni = false;

    }


    //private float TurnAngles()
    //{
    //    float angles = 0f;
    //    if (_movementalong)
    //    {
    //        if (_clock.GetPointerState(transform.name))
    //        {
    //            if (correctPos.z - _initAngles < 0)
    //            {
    //                angles = _initAngles - correctPos.z;
    //                _clock.SetPointerState(transform.name, false);
    //            }
    //            return angles;

    //        }
    //        else
    //        {
    //            if (correctPos.z - _initAngles > 0)
    //            {
    //                angles = correctPos.z - _initAngles;
    //                _clock.SetPointerState(transform.name, true);
    //            }
    //            return angles;
    //        }
    //    }
    //    else
    //    {
    //        if (_clock.GetPointerState(transform.name))
    //        {
    //            if (correctPos.z - _initAngles > 0)
    //            {
    //                angles = _initAngles - correctPos.z;
    //                _clock.SetPointerState(transform.name, false);
    //            }
    //            return angles;
    //        }
    //        else
    //        {
    //            if (correctPos.z - _initAngles < 0)
    //            {
    //                angles = correctPos.z - _initAngles;
    //                _clock.SetPointerState(transform.name, true);
    //            }
    //            return angles;
    //        }
    //    }
    //}

    private bool TurnAngles(out float targetPos)
    {
        if (_movementalong)
        {
            if (_clock.GetPointerState(transform.name))
            {
                if (correctPos.z - _initPos.z < 0)
                {
                    _clock.SetPointerState(transform.name, false);
                    targetPos = _initPos.z - correctPos.z;
                    return true;
                }
            }
            else
            {
                if (correctPos.z - _initPos.z > 0)
                {
                    _clock.SetPointerState(transform.name, true);
                    targetPos = correctPos.z - _initPos.z;
                    return true;
                }
            }
        }
        else
        {
            if (_clock.GetPointerState(transform.name))
            {
                if (correctPos.z - _initPos.z > 0)
                {
                    _clock.SetPointerState(transform.name, false);
                    targetPos = _initPos.z - correctPos.z;
                    return true;
                }
            }
            else
            {
                if (correctPos.z - _initPos.z < 0)
                {
                    _clock.SetPointerState(transform.name, true);
                    targetPos = correctPos.z - _initPos.z;
                    return true;
                }
            }
        }
        targetPos = 0;
        return false;
    }
    

    private void PlayerDirBySelf(Vector3 playPos)
    {

        Vector3 offset = playPos - transform.position;
        if(TransParent!=null)
            transform.parent = null;

        Vector3 cross = Vector3.Cross(transform.up, offset);
        if (cross.z > 0)
        {
            // 玩家在物体逆时针方向
            _movementalong = false;
        }
        else if (cross.z == 0)
        {
            // 玩家在物体方向相同（平行）
        }
        else
        {
            // 玩家在物体的顺时针方向
            _movementalong = true;
        }

        //transform.parent = TransParent;
    }
}
