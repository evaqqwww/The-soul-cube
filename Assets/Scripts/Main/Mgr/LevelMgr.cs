using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class LevelMgr : Singleton<LevelMgr>
{

    public Bounds levelBounds = new Bounds(Vector3.zero, Vector3.one * 10);

    private GameObject[] _gameCubes;
    private Transform[] _cubesTrans;

    public Transform[] CubesTrans
    {
        get
        {
            if (_cubesTrans == null)
            {
                _cubesTrans = new Transform[2];
                if (!_levelOrigin.doubleCube)
                    _cubesTrans[0] = _gameCubes[0].transform;
                else
                {
                    for (int i = 0; i < _gameCubes.Length; i++)
                    {
                        if (_gameCubes[i].name.Equals("Cube_1"))
                            _cubesTrans[0] = _gameCubes[i].transform;
                        else
                            _cubesTrans[1] = _gameCubes[i].transform;
                    }
                }
            }
            return _cubesTrans;
        }
    }

    //当前关卡基础信息
    private LevelOrigin _levelOrigin;

    //前置机关数量
    private int _gearCount;

    private PlayerCtrl _player;

    private Transform _hTurn, _vTurn, _singleTurn;

    private int _vRotation;
    private int _hRotation;

    //单cube水平转动角
    private int _hSRotation;

    private Door _door;

    public delegate void TurnSideGlobalHandle();

    public event TurnSideGlobalHandle turnSideGlobalHandle;

    public PlayerCtrl Player
    {
        get
        {
            return _player;
        }
    }


    protected override void Awake()
    {
        base.Awake();
        _player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerCtrl>();
        _hTurn = GameObject.FindGameObjectWithTag("hturn").transform;
        _vTurn = GameObject.FindGameObjectWithTag("vturn").transform;
        _singleTurn = GameObject.FindGameObjectWithTag("singleTurn").transform;

    }

    void Start()
    {

    }

    public void InitScene(LevelOrigin info)
    {
        _levelOrigin = info;
        _gearCount = _levelOrigin.gearCount;
       
        _gameCubes = GameObject.FindGameObjectsWithTag("cube");

        _player.SwitchShow();
        _levelOrigin.SpawnPlayer(_player);

        int zValue = CubesTrans[0].GetComponent<CubeState>().mapBase / 2;
        _hTurn.position = new Vector3(0, 0, zValue);
        _vTurn.position = new Vector3(0, 0, zValue);

        _door = GameObject.FindGameObjectWithTag("door").GetComponent<Door>();
        if (_door)
            _door.InitState(_gearCount > 0 ? true : false);
    }

    public void ClearWorld()
    {
        turnSideGlobalHandle = InitEvent;

    }

    //初始化全局事件
    private void InitEvent()
    {
        
    }

    #region CubeTurnSide

    public void GameCubeTurnRight(float time)
    {
        TurnSidePrepar(_hTurn);
        _hRotation += 90;
        SideSwitchMgr.It.SwitchBegin(BoundsBehavior.right);
        _hTurn.DORotateQuaternion(Quaternion.AngleAxis(_hRotation, Vector3.up), time).OnComplete(TurnSideCompleted);
    }

    public void GameCubeTurnLeft(float time)
    {
        TurnSidePrepar(_hTurn);
        _hRotation -= 90;
        SideSwitchMgr.It.SwitchBegin(BoundsBehavior.left);
        _hTurn.DORotateQuaternion(Quaternion.AngleAxis(_hRotation, Vector3.up), time).OnComplete(TurnSideCompleted);
    }

    public void GameCubeTurnUp(float time)
    {
        TurnSidePrepar(_vTurn);
        _vRotation -= 90;
        SideSwitchMgr.It.SwitchBegin(BoundsBehavior.up);
        _vTurn.DORotateQuaternion(Quaternion.AngleAxis(_vRotation, Vector3.right), time).OnComplete(TurnSideCompleted);
    }

    public void GameCubeTurnDown(float time)
    {
        TurnSidePrepar(_vTurn);
        _vRotation += 90;
        SideSwitchMgr.It.SwitchBegin(BoundsBehavior.down);
        _vTurn.DORotateQuaternion(Quaternion.AngleAxis(_vRotation, Vector3.right), time).OnComplete(TurnSideCompleted);
    }

    //单cube水平旋转
    public void SingleCubeTurn(int part)
    {
        ResetParents();
        CubesTrans[part].SetParent(_singleTurn);
        _hSRotation += 90;
        _singleTurn.DORotateQuaternion(Quaternion.AngleAxis(_hSRotation, Vector3.up), _player.cubeRotationTime).OnComplete(_player.RevertState);

    }

    private void ResetParents(bool reset = false)
    {
        foreach (var item in CubesTrans)
        {
            if (!item)
                continue;
                item.parent = null;
            if (reset)
                item.rotation = Quaternion.Euler(Vector3.zero);
        }

    }

    //设置父物体等转面准备工作
    private void TurnSidePrepar(Transform ctrl)
    {
        if (null != turnSideGlobalHandle)
            turnSideGlobalHandle();

        if (!_levelOrigin.doubleCube)
            CubesTrans[0].SetParent(ctrl);
        else if (_gameCubes.Length == 2)
        {
            CubesTrans[0].SetParent(ctrl);
            CubesTrans[1].SetParent(ctrl);
        }
        else
        {
            Debug.LogError("Current Scene is not doubleCube!!!");
            return;
        }
    }

    private void TurnSideCompleted()
    {
        _player.RevertState();
        turnSideGlobalHandle();
    }


    #endregion

    public void KillPlayer()
    {
        _player.KillSelf();
    }

    public void SpawnPlayer()
    {
        ResetParents(true);
        _levelOrigin.SpawnPlayer(_player);
    }

    public void UnlockGear()
    {
        _gearCount--;
        if (_gearCount == 0)
            _door.OpenTheDoor();
    }

}
