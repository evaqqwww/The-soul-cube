using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class RotationPlatform : Platform
{

    private bool _aniForward;
    private Tweener _tween;

   
    public void Start()
    {
        _aniForward = true;
        _tween = transform.DORotate(transform.localRotation.eulerAngles + new Vector3(0, 0, 90.0f), 0.3f, RotateMode.LocalAxisAdd).SetAutoKill(false);
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
            _tween.PlayBackwards();
            _aniForward = true;
        }
    }

}
