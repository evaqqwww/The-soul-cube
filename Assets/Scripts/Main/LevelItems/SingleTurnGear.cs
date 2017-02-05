using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SingleTurnGear : MonoBehaviour
{

    public int part;

    private BoxCollider _collider;
   
    public void Awake()
    {
        _collider = transform.GetComponent<BoxCollider>();
    }

    public void Start()
    {
        LevelMgr.It.turnSideGlobalHandle += SwitchCollider;

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag.Equals("player"))
        {
            //transform.localScale = new Vector3(_left ? _scale.x : -_scale.x, _scale.y, _scale.z);
            //_left = !_left;
            if (other.transform.up.y == transform.up.y && LevelMgr.It.Player.falling)
            {
                LevelMgr.It.Player.StoreState();
                LevelMgr.It.SingleCubeTurn(part);
            }
        }

    }


    private void SwitchCollider()
    {
        _collider.enabled = !_collider.enabled;
    }

}
