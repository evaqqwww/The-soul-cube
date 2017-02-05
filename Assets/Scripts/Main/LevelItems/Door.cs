using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    private GameObject _open;
    private GameObject _close;
    private BoxCollider _collider;

    void Awake()
    {
        _open = transform.FindChild("open").gameObject;
        _close = transform.FindChild("close").gameObject;
        _collider = transform.GetComponent<BoxCollider>();
    }

    void Start()
    {
        InitState(LevelMgr.It.gearCount > 0 ? true : false);
    }

    void InitState(bool hasLockGear)
    {
        _collider.enabled = !hasLockGear;
        _open.SetActive(!hasLockGear);
        _close.SetActive(hasLockGear);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (LevelMgr.It.Player.isGround)
        {
            int curIndex = SCApp.It.levelNum;
            curIndex++;
            if (curIndex > 3)
                return;
            GlobalEvents.It.events.dispatchEvent(new HEventWithParams(EventsDefine.LEVELTOLOAD, curIndex));
        }
    }

    public void OpenTheDoor()
    {
        _open.SetActive(true);
        _close.SetActive(false);
        _collider.enabled = true;
    }
}
