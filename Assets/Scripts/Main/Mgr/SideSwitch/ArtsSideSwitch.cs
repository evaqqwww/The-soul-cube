using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ArtsSideSwitch : MonoBehaviour
{

    private SpriteRenderer _renderer;
    private SideDevice _sideDev;
    private LayerMask _initialLayer;

    public bool isAni;

    public void Awake()
    {
        _renderer = transform.GetComponent<SpriteRenderer>();
        _sideDev = transform.parent.parent.GetComponent<SideDevice>();
    }
    

    void Start()
    {
        _initialLayer = this.gameObject.layer;
#if sideSwitch
        _sideDev.artsSideHandle += SwitchSide;
        if (_sideDev.sideNum == 0)
            return;
        SwitchSide();
#endif
    }

    private void SwitchSide()
    {
        if (isAni)
            this.gameObject.SetActive(!this.gameObject.activeInHierarchy);
        else if (_initialLayer.value != 12)
            _renderer.enabled = !_renderer.enabled;
        else
        {
            this.gameObject.layer = this.gameObject.layer == 8 ? 12 : 8;

        }

    }


}
