using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapSideSwitch : MonoBehaviour
{

    private GameObject _back;
    private SideDevice _sideDev;

    private SpriteRenderer _backRenderer;
    private SpriteRenderer _forwardRenderer;
    private BoxCollider _boxCollider;
    private bool _isVoid;


    public void Awake()
    {
        try
        {
            _back = transform.FindChild("mapBack").gameObject;
        }
        catch (Exception)
        {
            
        }
        _sideDev = transform.parent.parent.GetComponent<SideDevice>();

        if (_back)
            _backRenderer = _back.GetComponent<SpriteRenderer>();
        _forwardRenderer = transform.FindChild("map").GetComponent<SpriteRenderer>();

        _boxCollider = this.GetComponent<BoxCollider>();
    }

    public void Start()
    {
        _backRenderer.sprite = Resources.Load<Sprite>("Standard/mapNode");
        _backRenderer.color = Color.white;
        if (_backRenderer)
            _backRenderer.sprite.name = "1_back";

#if sideSwitch
        _forwardRenderer.enabled = false;
        if (_back)
        {
            _backRenderer.enabled = false;
            _sideDev.GetComponent<SideDevice>().mapSideHandle += SwitchSide;
            if (_sideDev.sideNum == 0)
                return;
            SwitchSide();
        }
#endif
    }

#region EditorTest

#if !sideSwitch

    public void InitVoidPlat(Color _color, bool _void)
    {
        _forwardRenderer.color = _color;
        _isVoid = _void;
        SwitchState();
    }

    public void SwitchState()
    {
        if (_isVoid)
        {
            _forwardRenderer.color = new Color(_forwardRenderer.color.r, _forwardRenderer.color.g, _forwardRenderer.color.b, 0.5f);
            _boxCollider.enabled = false;
            _isVoid = false;
        }
        else
        {
            _forwardRenderer.color = new Color(_forwardRenderer.color.r, _forwardRenderer.color.g, _forwardRenderer.color.b);
            _boxCollider.enabled = true;
            _isVoid = true;
        }
    }

#endif

#endregion

    private void SwitchSide()
    {
        _backRenderer.enabled = !_backRenderer.enabled;
    }
}
