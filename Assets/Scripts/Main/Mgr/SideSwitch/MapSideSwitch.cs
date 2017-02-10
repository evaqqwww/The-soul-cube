using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapSideSwitch : MonoBehaviour
{

    private GameObject _back;
    private GameObject _forward;
    private SideDevice _sideDev;
    private SpriteRenderer _backRenderer;


    public void Awake()
    {
        _back = transform.FindChild("mapBack").gameObject;
        _forward = transform.FindChild("map").gameObject;
        _sideDev = transform.parent.parent.GetComponent<SideDevice>();
        _backRenderer = _back.GetComponent<SpriteRenderer>();

    }

    public void Start()
    {
        _backRenderer.sprite = Resources.Load<Sprite>("Standard/mapNode");
        _backRenderer.color = Color.white;
        _backRenderer.sprite.name = "1_back";

#if sideSwitch
        _forward.SetActive(false);
        _back.SetActive(false);
        _sideDev.GetComponent<SideDevice>().mapSideHandle += SwitchSide;
        if (_sideDev.sideNum == 0)
            return;
        SwitchSide();
#endif
    }


    private void SwitchSide()
    {
        _back.SetActive(!_back.activeInHierarchy);
        //_forward.SetActive(!_forward.activeInHierarchy);
    }
}
