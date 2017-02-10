using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    //初始化状态
    public bool initCompleted;
    //当前编辑平面
    public int curSide;
    //地图块基数
    public int mapBase;

    public void Start()
    {
        int baseNum = mapBase / 2;
        LevelMgr.It.levelBounds.extents = new Vector3(baseNum + 0.2f, baseNum + 0.2f, baseNum * 0.1f);

        SideSwitchMgr.It.Init(baseNum);

    }




}
