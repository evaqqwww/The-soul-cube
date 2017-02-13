using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class VoidGear : MonoBehaviour
{

    public int type;

    private SpriteRenderer _renderer;

    public void Awake()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
    }


    public void Start()
    {
        _renderer.color = VoidPlatformMgr.It.GetColorByIndex(type);
    }


    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag.Equals("player"))
        {
            Vector3 lastScale = transform.localScale;
            transform.localScale = new Vector3(-lastScale.x, lastScale.y, lastScale.z);
            VoidPlatformMgr.It.SwitchVoidState(type);
        }

    }
}
