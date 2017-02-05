using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalEvents  {

    private static GlobalEvents _instance;
    public static GlobalEvents It
    {
        get
        {
            if (null == _instance)
                _instance = new GlobalEvents();
            return _instance;
        }
    }
    public EventDispatcher events = new EventDispatcher();

    public GlobalEvents()
    {
        
    }

}
