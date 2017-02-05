using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CrossPlatformInputManager
{
    private static CrossPlatformInputManager _it;

    public static CrossPlatformInputManager It
    {
        get
        {
            if (null == _it)
                _it = new CrossPlatformInputManager();
            return _it;
        }
    }

    //当前输入值
    private int _inputValue;

    public float GetValue
    {
        get { return _inputValue; }
    }

    public void Update(int value)
    {
        _inputValue = value;
        Debug.Log(_inputValue);
    }
}
