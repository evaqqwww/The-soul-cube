using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public virtual void StartAni()
    {

    }

    public void RevertState()
    {
        LevelMgr.It.Player.RevertState();
    }
}
