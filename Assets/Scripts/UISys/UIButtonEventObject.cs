using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// 按钮事件，可以带一个参数
[RequireComponent(typeof(BoxCollider))]
public class UIButtonEventObject : UIWidgetContainer
{
    public delegate void MouseEventHandler(object[] ps);

    public class Handler
    {
        public MouseEventHandler handle;
        public object[] paras;
    }
    public bool skipClickWhenInLongPressed = false;         // 是否长按后忽略OnClick
    //可调参数
    public float firstDelay = 0.1f;                         // 首次间隔时间
                
    protected bool _longPressEnd = false;                    // 是否长按状态。。。。。
    protected bool _bPressed = false;                       // 按下状态，是否按下。     
    protected float _pressDelay;                            // 记录进入press的时间
    protected bool _bDraging = false;

    protected List<Handler> _lClick = new List<Handler>();
    protected List<Handler> _lPress = new List<Handler>();

    void OnDisable()
    {
        _bPressed = false;
        _longPressEnd = false;
        _bDraging = false;
    }

    public void AddClick(MouseEventHandler eh, params object[] ps)
    {
        Handler h = new Handler();
        h.handle = eh;
        h.paras = ps;
        _lClick.Add(h);
    }

    public void AddSingleClick(MouseEventHandler eh, params object[] ps)
    {
        _lClick.Clear();
        Handler h = new Handler();
        h.handle = eh;
        h.paras = ps;
        _lClick.Add(h);
    }

    public void AddPress(MouseEventHandler eh, params object[] ps)
    {
        Handler h = new Handler();
        h.handle = eh;
        h.paras = ps;
        _lPress.Add(h);
    }

    public void AddSinglePress(MouseEventHandler eh, params object[] ps)
    {
        _lPress.Clear();
        AddPress(eh, ps);
    }

    public void RemoveClick(MouseEventHandler eh)
    {
        for (int i = 0; i < _lClick.Count; i++)
        {
            if (_lClick[i].handle == eh)
            {
                _lClick.RemoveAt(i);
                return;
            }
        }
    }

    public void ClearClick()
    {
        _lClick.Clear();
    }

    protected void CallHandlerList(List<Handler> l)
    {
        if (l == null || l.Count == 0) return;

        Handler[] handlers = new Handler[l.Count];
        l.CopyTo(handlers, 0);

        for (int i = 0; i < handlers.Length; i++)
        {
            try
            {
                Handler h = handlers[i];
                h.handle(h.paras);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    #region UICamera Event

    protected void OnClick()
    {
        if (this.skipClickWhenInLongPressed && _longPressEnd)
            return;
        CallHandlerList(_lClick);
    }

    protected virtual void OnPress(bool pressed)
    {
        if (pressed)
        {
            _bPressed = true;
            ResetLongPress();
        }
        else
        {
            _bDraging = false;
            _bPressed = false;

            if (_lPress.Count != 0)
                CrossPlatformInputManager.It.Update(0); //更新移动输入状态
        }
    }

    protected virtual void OnDrag(Vector2 delta)
    {
        _bDraging = true;
    }

    #endregion


    protected void ResetLongPress()
    {
        _pressDelay = 0;
        _longPressEnd = false;
    }

    protected void Update()
    {
        if (_bPressed)
        {
            _pressDelay += Time.deltaTime;

            CheckCanEnterLongPress();
        }
    }

    protected virtual void CheckCanEnterLongPress()
    {
        if (_bDraging)
            return;
        if (_longPressEnd)
            return;
        if (_pressDelay >= this.firstDelay)
        {
            CallHandlerList(_lPress);
            _longPressEnd = true;
            //skipClickWhenInLongPressed = false;
        }
    }
}
