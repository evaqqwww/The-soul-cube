using UnityEngine;
using Spine.Unity;
using System.Collections;

public class Door : MonoBehaviour
{

    private BoxCollider _collider;
    private SkeletonAnimation _skeletonAni;

    //动画播放标志
    private bool _playAni;
    private int _theSide;

    [SpineAnimation]
    public string closed, unlocked, opening, open;

    void Awake()
    {
        _collider = transform.GetComponent<BoxCollider>();
        _skeletonAni = this.transform.FindChild("doorAni").GetComponent<SkeletonAnimation>();
        _theSide = this.transform.parent.GetComponent<SideDevice>().sideNum;
    }

    void Start()
    {
        _playAni = false;

        LevelMgr.It.turnSideGlobalHandle += SwitchAni;

    }

    public void InitState(bool hasLockGear)
    {
        _collider.enabled = !hasLockGear;
        _skeletonAni.AnimationState.SetAnimation(0, closed, true);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (LevelMgr.It.Player.isGround)
        {
            int curIndex = SCApp.It.levelNum;
            curIndex++;

            if (curIndex > 16)
                return;
            LevelMgr.It.ClearWorld();
            GlobalEvents.It.events.dispatchEvent(new HEventWithParams(EventsDefine.LEVELTOLOAD, curIndex));
        }
    }

    public void OpenTheDoor()
    {
        _playAni = true;
        _collider.enabled = true;
    }

    private void SwitchAni()
    {
        if (!_playAni)
            return;
        if (_theSide == SideSwitchMgr.It.curSide)
        {
            _playAni = false;
            StartCoroutine(OpenAni());
        }
    }

    IEnumerator OpenAni()
    {
        yield return null;
        _skeletonAni.AnimationState.SetAnimation(0, unlocked, false);
        yield return new WaitForSeconds(1.5f);
        _skeletonAni.AnimationState.SetAnimation(0, opening, false);
        _skeletonAni.AnimationState.AddAnimation(0, open, true, 0);
    }
}
