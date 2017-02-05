using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    public float UpdateInterval = 0.5f;

    private float _framesAccumulated = 0f;
    private float _framesDrawnInTheInterval = 0f;
    private float _timeLeft;

    private UILabel _text;

    public void Start()
    {

        _text = this.GetComponent<UILabel>();
        _timeLeft = UpdateInterval;  

    }

    public void Update()
    {

        _framesDrawnInTheInterval++;
        _framesAccumulated = _framesAccumulated + Time.timeScale / Time.deltaTime;
        _timeLeft = _timeLeft - Time.deltaTime;

        if (_timeLeft <= 0.0)
        {
            _text.text = (_framesAccumulated / _framesDrawnInTheInterval).ToString("f2");
            _framesDrawnInTheInterval = 0;
            _framesAccumulated = 0f;
            _timeLeft = UpdateInterval;
        }
    }



}
