using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Clock : MonoBehaviour
{
    //分针，时针的正确状态
    private bool _pointer_1;
    private bool _pointer_2;

    public delegate void StopClock();

    public event StopClock stopClock;

    public void Start()
    {

    }


    public bool GetPointerState(string name)
    {
        if (name.Equals("1"))
            return _pointer_1;
        else
            return _pointer_2;
    }

    public void SetPointerState(string name, bool state)
    {
        if (name.Equals("1"))
        {
            _pointer_1 = state;
        }
        else
        {
            _pointer_2 = state;
        }

        if (_pointer_1 && _pointer_2)
        {
            Debug.LogError("clock is right!!!");
            stopClock();
            LevelMgr.It.UnlockGear();
        }

    }

}
    
