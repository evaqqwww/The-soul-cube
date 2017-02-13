using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public enum eTowards
{
    up,
    down,
    left,
    right,
}

public class MovePlatform : Platform
{

    private bool _aniForward;
    private Tweener _tween;

    //移动方向和数值
    [HideInInspector]
    [SerializeField]
    private bool _ishori;
    [HideInInspector]
    [SerializeField]
    private int _value;
    [HideInInspector]
    [SerializeField]
    private eTowards _towards;

    public float dis;
    public float time;

    public void Start()
    {
        //Debug.Log(_ishori + _value.ToString());
        
        _aniForward = true;
        if (_ishori)
            _tween = transform.DOMoveX(transform.localPosition.x + dis * _value, time)
                     .SetAutoKill(false).OnComplete(RevertState);
        else
            _tween = transform.DOMoveY(transform.localPosition.y + dis * _value, time)
                     .SetAutoKill(false).OnComplete(RevertState);

        _tween.Pause();
    }

    public override void StartAni()
    {

        if (_aniForward)
        {
            _tween.PlayForward();
            _aniForward = false;
        }
        else
        {
            _tween.OnRewind(RevertState);
            _tween.PlayBackwards();
            _aniForward = true;
        }
    }

    public void SetMoveToward(bool side, int value)
    {
        _ishori = side;
        _value = value;
    }

}
