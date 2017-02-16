using System;
using System.Collections;
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
    private Vector3 _spawanPoint;
    private int _mapBase;

    public void Awake()
    {
        _mapBase = this.transform.parent.parent.GetComponent<CubeState>().mapBase;
    }

    void Start()
    {
        _spawanPoint = transform.position;
        int _base = _mapBase / 2;

        LevelMgr.It.InitScene(this, _base);
    }


    public void SpawnPlayer(PlayerCtrl player)
    {
        StartCoroutine(SpawnPlayerAni(player));
    }

    IEnumerator SpawnPlayerAni(PlayerCtrl player)
    {
        player.PlayerTrans.position = _spawanPoint;
        yield return new WaitForSeconds(0.5f);
        player.SwitchShow();

        yield return null;

        player.ctrlActive = true;

    }
}


