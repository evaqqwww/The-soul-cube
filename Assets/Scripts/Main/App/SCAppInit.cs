using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SCApp
{
    private static SCApp _it;
    private Camera _uiCam;
    
    //当前关卡
    public string curlevelName;
    public int levelNum;

    public static SCApp It
    {

        get
        {
            if (_it == null)
                _it = new SCApp();
            return _it;
        }
    }

    public Camera UICam
    {
        get
        {
            return _uiCam;
        }
    }


    public void InitPreAwake()
    {
        InitUI();
    }

    void InitUI()
    {
        GameObject gm = GameObject.Find("Anchor");
        UIMgr.It.Init(gm);
        _uiCam = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();

    }
}
