using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SideDevice : MonoBehaviour
{
    public int sideNum;
   
    public delegate void MapSideHandle();
    public delegate void ArtsSideHandle();

    public event MapSideHandle mapSideHandle;
    public event ArtsSideHandle artsSideHandle;


    public void Start()
    {
       
    }

    public void MapSideSwitch()
    {
        if (mapSideHandle == null)
            return;
        mapSideHandle();
    }

    public void ArtsSideSwitch()
    {
        if (artsSideHandle == null)
            return;
        artsSideHandle();
    }
}
