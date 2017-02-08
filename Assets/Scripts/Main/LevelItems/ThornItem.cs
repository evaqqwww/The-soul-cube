using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ThornItem : MonoBehaviour
{

    public void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag.Equals("player"))
        {
            LevelMgr.It.KillPlayer();
        }
    }


}
