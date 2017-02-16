using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    private Transform[] cams = new Transform[2];

    public void Awake()
    {
        cams[0] = this.transform.FindChild("MainCam");
        cams[1] = this.transform.FindChild("BlurCam");
    }


    public void Start()
    {

    }


    public void InitCamsPos(int baseNum)
    {
        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].position = new Vector3(0, 0, -5.0f * baseNum);
        }
    }


}
