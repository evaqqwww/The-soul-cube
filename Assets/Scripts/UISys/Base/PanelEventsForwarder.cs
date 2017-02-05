using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class PanelEventHandler
{
    public EventDispatcher.ObjectDelegate callback;
    public bool strongEvent;
}

// 单个面板内不存在一个事件多个处理者
public class PanelEventsForwarder
{
    NGUIBasePanel _panel;
    Dictionary<string, PanelEventHandler> _events =
        new Dictionary<string, PanelEventHandler>();

    public void Init(NGUIBasePanel panel)
    {
        _panel = panel;
    }

    public void Release()
    {
        _panel = null;
        _events.Clear();
    }

    public void Bind(bool bind, string eventName, EventDispatcher.ObjectDelegate callback,
        bool strongEvent = true)
    {
        if (!bind)
        {
            RemoveCallback(eventName);
            return;
        }
        PanelEventHandler handler = new PanelEventHandler();
        handler.callback = callback;
        handler.strongEvent = strongEvent;
        AddCallback(eventName, handler);
    }

    void AddCallback(string eventName, PanelEventHandler callback)
    {
        RemoveCallback(eventName);

        _events[eventName] = callback;
        GlobalEvents.It.events.addEventListener(eventName, OnEvent);
    }

    void RemoveCallback(string eventName)
    {
        PanelEventHandler handler = null;
        if (!_events.TryGetValue(eventName, out handler))
            return;
        _events.Remove(eventName);
        GlobalEvents.It.events.removeEventListener(eventName, OnEvent);
    }

    void OnEvent(HEvent e)
    {
        ForwardEvent(e);
    }

    void ForwardEvent(HEvent e)
    {
        PanelEventHandler handler = null;
        if (!_events.TryGetValue(e.Name, out handler))
            return;
        _panel.ForwardEvent(e, handler.callback, handler.strongEvent);
    }
}

