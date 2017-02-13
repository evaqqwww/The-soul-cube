using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class VoidPlatformMgr
{

    private static VoidPlatformMgr _it;

    public static VoidPlatformMgr It
    {
        get
        {
            if (null == _it)
                _it = new VoidPlatformMgr();
            return _it;
        }
    }

    //虚空平台集合
    private Dictionary<int, List<VoidPlatform>> _platformDic = new Dictionary<int, List<VoidPlatform>>();

    private Color[] randomColor = new Color[6] { new Color(48/255.0f, 97/255.0f, 135/255.0f), new Color(48/255.0f, 135/255.0f, 85/255.0f), new Color(146/255.0f, 70/255.0f, 111/255.0f), 
                                                 new Color(153/255.0f, 54/255.0f, 41/255.0f), new Color(177/255.0f, 177/255.0f, 177/255.0f), new Color(192/255.0f, 164/255.0f, 77/255.0f) };

    public Color GetColorByIndex(int type)
    {
        return randomColor[type];
    }
    
    //根据类型绑定到集合
    public void BindPlatforms(int index,VoidPlatform self)
    {
        if (!_platformDic.ContainsKey(index))
        {
            List<VoidPlatform> _list = new List<VoidPlatform>();
            _platformDic.Add(index, _list);
        }

        _platformDic[index].Add(self);

    }

    public void SwitchVoidState(int index)
    {
        if (!_platformDic.ContainsKey(index))
            return;
        List<VoidPlatform> temp = new List<VoidPlatform>();
        _platformDic.TryGetValue(index, out temp);
        if (temp.Count == 0)
            return;
        for (int i = 0; i < temp.Count; i++)
        {
            temp[i].SwitchVoid();
        }

        temp = null;
    }

    public void UnloadMgr()
    {
        _platformDic = null;
        _it = null;
    }


}
