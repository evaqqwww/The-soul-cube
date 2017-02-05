using UnityEngine;
using System.Collections;


public class HEvent
{
    private string eventName_;

    public HEvent(string eventName)
    {
        this.eventName_ = eventName;
    }

    public string Name
    {
        get { return this.eventName_; }
    }
}

public class HEventWithParams : HEvent
{
    private object[] _params;
    public object[] paras
    {
        get
        {
            return _params;
        }
    }

    public HEventWithParams(string eventName, params object[] args)
        : base(eventName)
    {
        _params = args;
    }
}


public class EventDispatcher
{
    public delegate void ObjectDelegate(HEvent evt);

    private Hashtable m_listenerTable = new Hashtable();
    public Hashtable listeners { get { return m_listenerTable; } }

    public bool addEventListener(string eventName, ObjectDelegate listener)
    {
        if (listener == null || eventName == null)
        {
            Debug.LogError("Event Manager: AddListener failed due to no listener or event name specified.");

            return false;
        }

        if (!m_listenerTable.ContainsKey(eventName))
        {
            m_listenerTable.Add(eventName, new ArrayList());
        }

        ArrayList listenerList = m_listenerTable[eventName] as ArrayList;

        if (listenerList.Contains(listener))
        {
            Debug.Log("Event Manager: Listener: " + listener.GetType().ToString()
                + " is already in list for event: " + eventName);

            return false;
        }

        listenerList.Add(listener);

        return true;
    }
    
    public bool dispatchEvent(HEvent evt)
    {
        string eventName = evt.Name;

        if (!m_listenerTable.ContainsKey(eventName))
        {
            //Debug.Log("Event Manager: Event \"" + eventName + "\" triggered has no listeners!");

            return false; // No listeners for event so ignore it
        }

        ArrayList listenerList = m_listenerTable[eventName] as ArrayList;

        // 防止出现在处理中remove自己的处理器
        ArrayList toCall = listenerList.Clone() as ArrayList;
        foreach (ObjectDelegate listener in toCall)
        {
            listener(evt);
        }

        return true;
    }

    
    public bool removeEventListener(string eventName, ObjectDelegate listener)
    {
        if (!m_listenerTable.ContainsKey(eventName))
        {
            Debug.LogWarning("remove event listener fail. "
                + "because event is not exist: " + eventName);

            return false;
        }

        ArrayList listenerList = m_listenerTable[eventName] as ArrayList;

        if (!listenerList.Contains(listener))
        {
            Debug.LogWarning("remove event listener fail. "
                + "because listener is not exist: " + listener.ToString());

            return false;
        }

        listenerList.Remove(listener);

        return true;
    }

    
    public bool bindEventListener(bool bAdd, string eventName, ObjectDelegate listener)
    {
        if (bAdd)
            return addEventListener(eventName, listener);
        return removeEventListener(eventName, listener);
    }

    
    public bool hasEventListener(string eventName)
    {
        return m_listenerTable.ContainsKey(eventName);
    }
}

