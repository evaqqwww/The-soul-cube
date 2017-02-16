using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BackgroundMgr : MonoBehaviour
{

    public int xCount,yCount;

    private GameObject _preObj;
    private Transform _ttRoot;
    

    public void Awake()
    {
        _preObj = this.transform.FindChild("tt").gameObject;
        _ttRoot = this.transform.FindChild("TTRoot");
    }

    public void Start()
    {
        InitPartGround();
    }


    void InitPartGround()
    {
        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                RandomState();
                Vector3 pos1 = new Vector3(i * 3.0f, j * 2.1f, 0);
                GameObject.Instantiate(_preObj, pos1, Quaternion.identity, _ttRoot);
                RandomState();
                Vector3 pos4 = new Vector3(i * 3.0f * -1, j * 2.1f * -1, 0);
                GameObject.Instantiate(_preObj, pos4, Quaternion.identity, _ttRoot);
                if (i != 0 && j != 0)
                {
                    RandomState();
                    Vector3 pos2 = new Vector3(i * 3.0f * -1, j * 2.1f, 0);
                    GameObject.Instantiate(_preObj, pos2, Quaternion.identity, _ttRoot);
                    RandomState();
                    Vector3 pos3 = new Vector3(i * 3.0f, j * 2.1f * -1, 0);
                    GameObject.Instantiate(_preObj, pos3, Quaternion.identity, _ttRoot);
                }
            }
        }
    }


    void RandomState()
    {
        //return;
        int type = UnityEngine.Random.Range(0, 4);
        switch (type)
        {
            case 0:
                _preObj.transform.localScale = new Vector3(1, 1, 1);
                break;
            case 1:
                _preObj.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case 2:
                _preObj.transform.localScale = new Vector3(1, -1, 1);
                break;
            case 3:
                _preObj.transform.localScale = new Vector3(-1, -1, 1);
                break;
            default:
                break;
        }
        
    }


}
