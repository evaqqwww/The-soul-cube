using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LevelOrigin : MonoBehaviour
{

    //关卡前置机关
    public int gearCount;
    //双cube场景
    public bool doubleCube;

    void Start()
    {
        LevelMgr.It.InitScene(this);

    }


    public void SpawnPlayer(Transform player)
    {
        LevelMgr.It.Player.PlayerTrans.position = transform.position;
        
    }
}


