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

    //当前关卡基础信息
    private LevelOrigin _levelOrigin;
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
    }

    void Start()
    {


    }

    public void InitScene(LevelOrigin info)
    {
        _gameCubes = GameObject.FindGameObjectsWithTag("cube");
        _door = GameObject.FindGameObjectWithTag("door").GetComponent<Door>();
        _levelOrigin = info;
        _gearCount = _levelOrigin.gearCount;
        if (!_levelOrigin.doubleCube)
            return;
        _singleTurn = GameObject.FindGameObjectWithTag("singleTurn").transform;

        _levelOrigin.SpawnPlayer(Player.PlayerTrans);
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
        _hTurn.DORotateQuaternion(Quaternion.AngleAxis(_hRotation, Vector3.up), time).OnComplete(TurnSideCompleted);
    }

    public void GameCubeTurnLeft(float time)
    {
        TurnSidePrepar(_hTurn);
        _hRotation -= 90;
        _hTurn.DORotateQuaternion(Quaternion.AngleAxis(_hRotation, Vector3.up), time).OnComplete(TurnSideCompleted);
    }

    public void GameCubeTurnUp(float time)
    {
        TurnSidePrepar(_vTurn);
        _vRotation -= 90;
        _vTurn.DORotateQuaternion(Quaternion.AngleAxis(_vRotation, Vector3.right), time).OnComplete(TurnSideCompleted);
    }

    public void GameCubeTurnDown(float time)
    {
        TurnSidePrepar(_vTurn);
        _vRotation += 90;
        _vTurn.DORotateQuaternion(Quaternion.AngleAxis(_vRotation, Vector3.right), time).OnComplete(TurnSideCompleted);
    }

    //单cube水平旋转
    public void SingleCubeTurn(int part)
    {
        foreach (var item in CubesTrans)
        {
            item.parent = null;
        }
        CubesTrans[part].SetParent(_singleTurn);
        _hSRotation += 90;
        _singleTurn.DORotateQuaternion(Quaternion.AngleAxis(_hSRotation, Vector3.up), LevelMgr.It.Player.cubeRotationTime).OnComplete(_player.RevertState);

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


    public void UnlockGear()
    {
        _gearCount--;
        if (_gearCount == 0)
            _door.OpenTheDoor();
    }

}
