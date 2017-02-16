using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TTRandom : MonoBehaviour
{

    //private SpriteRenderer _renderer;
    

    public void Awake()
    {
        //_renderer = this.GetComponent<SpriteRenderer>();
    }

    public void Start()
    {

        this.InvokeRepeating("SwitchDir", 0.1f, 0.2f);
    }

    void SwitchDir()
    {
        //_renderer.flipX = !_renderer.flipX;
        Vector3 scale = this.transform.localScale;
        this.transform.localScale = new Vector3(-scale.x, scale.y, 1);
    }

}
