using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;


/// <summary>
/// 边缘行为
/// </summary>
public enum BoundsBehavior
{
    up = 0,
    down,
    left,
    right,
}

public class PlayerCtrl : MonoBehaviour 
{

    //控制器可用
    public bool ctrlActive;
    
    private Transform _playerTrans;

    public Transform PlayerTrans
    {
        get
        {
            if (null == _playerTrans)
                _playerTrans = this.transform;
            return _playerTrans;
        }
    }

    //玩家图形
    public Transform playerGraphics;
    private Rigidbody _rigidbody;
    private BoxCollider _playerCollider;

    [Range(0.0f, 1.0f)]
    //旋转中玩家平移时间
    public float translationTime = 0.9f;
    [Range(0.0f, 0.5f)]
    //旋转中玩家平移偏移
    public float targetOffset = 0.2f;
    [Range(0.0f, 1.0f)]
    //方块旋转速度
    public float cubeRotationTime = 1.0f;

    [Range(0.0f, 50.0f)]
    //水平移动速度
    public float moveSpeed;
    private float _moveVelocity;

    //快速下落中双面射线
    private bool _bothSideRay;
    //跳跃高度
    [Range(0.0f, 50.0f)]
    public float jumpHeight = 18.0f;

    public LayerMask whatIsGround;
    
    public bool isGround;

    //触碰墙壁边缘
    public bool isWallEdge;
    public int rayCount;              //检测射线数
    private Rect _rayBoundsRectangle;
    private float _extendOffset = 0.05f;
    
    public bool facingRight = true;
    public bool walking;
    public bool falling;
    public bool goUp;

    //换面相关参数
    private Bounds _bounds;
    private bool _checkingBounds;
   
    void Awake()
    {
        playerGraphics = transform.FindChild("playerGraphics");
        _rigidbody = transform.GetComponent<Rigidbody>();
        _playerCollider = transform.GetComponent<BoxCollider>();
        SetRaysParameters();
    }

    void Start()
    {
        _bounds = LevelMgr.It.levelBounds;
        _checkingBounds = true;
    }

    void EveryFrame()
    {
        //this.isGround = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsGround).Length != 0;
        facingRight = playerGraphics.localScale.x > 0;
        CastRaysToTheSides();
        CastRaysAbove();
        CastRaysBelow();
    }

    void Update()
    {
        if (_rigidbody.velocity.y < -0.1f && !isGround)
        {
            falling = true;
            _bothSideRay = true;
        }
        else
        {
            falling = false;
            _bothSideRay = false;
        }

        if (_rigidbody.velocity.y > 0.1f && !isGround)
        {
            goUp = true;
        }
        else
        {
            goUp = false;
        }

        if (ctrlActive == true && Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jump();
        }

        if (ctrlActive)
        {
#if UNITY_EDITOR||UNITY_STANDALONE
            _moveVelocity = moveSpeed * Input.GetAxisRaw("Horizontal");
#else
             _moveVelocity = moveSpeed * CrossPlatformInputManager.It.GetValue;
#endif
        }

        SetRaysParameters();

        if (ctrlActive)
            EveryFrame();

        //朝向和行走状态
        if (ctrlActive && Input.GetAxis("Horizontal") > 0.1f || ctrlActive && CrossPlatformInputManager.It.GetValue > 0.5f)
        {
            playerGraphics.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (isGround)
                walking = true;
        }
        else if (ctrlActive && Input.GetAxis("Horizontal") < -0.1f || ctrlActive && CrossPlatformInputManager.It.GetValue < -0.5f)
        {
            playerGraphics.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            if (isGround)
                walking = true;
        }
        else
            walking = false;

        if (isWallEdge)
            _moveVelocity = 0;
        
        _rigidbody.velocity = new Vector2(_moveVelocity, _rigidbody.velocity.y);

        SetRaysParameters();
       
        CheckBoundsEdge();

    }

    //跳跃
    private void jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpHeight);
    }

    //移动设备跳跃
    public void MobileJump()
    {
        if (ctrlActive == true && isGround)
            jump();
    }

    #region 碰撞检测射线

    //发射墙壁检测射线
    private void CastRaysToTheSides()
    {
        Vector3 rayCastFromBottom = new Vector3(_rayBoundsRectangle.center.x, _rayBoundsRectangle.yMin + _extendOffset * 0.5f, 0);
        Vector3 rayCastFromUp = new Vector3(_rayBoundsRectangle.center.x, _rayBoundsRectangle.yMax - _extendOffset * 0.5f, 0);

        float rayLength = /*Mathf.Abs(_moveVelocity * Time.deltaTime)*/ + _rayBoundsRectangle.width * 0.5f + _extendOffset;

        int dir = facingRight ? 1 : -1;
        RaycastHit hitsStorage = new RaycastHit();

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 rayOriginPoint = Vector3.Lerp(rayCastFromBottom, rayCastFromUp, (float)i / (float)(rayCount - 1));

            if (/*_bothSideRay*/true)
            {
                hitsStorage = SystemUtil.RayCast(rayOriginPoint, -dir * Vector3.right, rayLength, whatIsGround, Color.black, true);

                if (hitsStorage.collider != null)
                {
                    if (_moveVelocity == 0)
                        return;
                    //判断墙壁和移动方向一致性
                    if (Math.Sign(_moveVelocity) - dir == 0)
                    {
                        isWallEdge = false;
                        return;
                    }
                        
                    isWallEdge = true;
                    //防止穿墙的位置重置（通用原理）
                    //this.PlayerTrans.position = new Vector3(hitsStorage.point.x + dir * _rayBoundsRectangle.width * 0.5f, PlayerTrans.position.y, 0);

                    return;
                }
            }

            hitsStorage = SystemUtil.RayCast(rayOriginPoint, dir * Vector3.right, rayLength, whatIsGround, Color.black, true);

            if (hitsStorage.collider != null)
            {
                if (_moveVelocity == 0)
                    return;
                if (Math.Sign(_moveVelocity) + dir == 0)
                {
                    isWallEdge = false;
                    return;
                }

                isWallEdge = true;
                //this.PlayerTrans.position = new Vector3(hitsStorage.point.x - dir * _rayBoundsRectangle.width * 0.5f, PlayerTrans.position.y, 0);

                return;
            }
        }

        isWallEdge = false;
    }

    //发射顶部检测射线
    private void CastRaysAbove()
    {
        Vector3 verticalRayCastFromLeft = new Vector3(_rayBoundsRectangle.xMin + _extendOffset,
                                                        _rayBoundsRectangle.center.y, 0);
        Vector3 verticalRayCastToRight = new Vector3(_rayBoundsRectangle.xMax - _extendOffset,
                                                   _rayBoundsRectangle.center.y, 0);
        float rayLength = Mathf.Abs(_rigidbody.velocity.y * Time.deltaTime) + _rayBoundsRectangle.height * 0.5f + _extendOffset;

        RaycastHit hitsStorage = new RaycastHit();

        for (int i = 0; i < rayCount; i++)
        {
            if (!(_rigidbody.velocity.y > 0))
                break;
            Vector3 rayOriginPoint = Vector3.Lerp(verticalRayCastFromLeft, verticalRayCastToRight, (float)i / (float)(rayCount - 1));
            hitsStorage = SystemUtil.RayCast(rayOriginPoint, Vector3.up, rayLength, whatIsGround, Color.black, true);

            if (hitsStorage.collider != null)
            {
                _rigidbody.velocity = new Vector2(_moveVelocity, 0);
                this.PlayerTrans.position = new Vector3(PlayerTrans.position.x, hitsStorage.point.y - _rayBoundsRectangle.height * 0.5f, 0);
                return;
            }
        }
    }

    //发射地面检测射线
    private void CastRaysBelow()
    {
        
        Vector3 verticalRayCastFromLeft = new Vector3(_rayBoundsRectangle.xMin + _extendOffset * 4,
                                                        _rayBoundsRectangle.center.y, 0);
        Vector3 verticalRayCastToRight = new Vector3(_rayBoundsRectangle.xMax - _extendOffset * 4,
                                                   _rayBoundsRectangle.center.y, 0);
        float rayLength = Mathf.Abs(_rigidbody.velocity.y * Time.deltaTime) + _rayBoundsRectangle.height * 0.5f + _extendOffset;

        RaycastHit hitsStorage = new RaycastHit();

        for (int i = 0; i < rayCount; i++)
        {
            if (_rigidbody.velocity.y > 0)
                break;
            Vector3 rayOriginPoint = Vector3.Lerp(verticalRayCastFromLeft, verticalRayCastToRight, (float)i / (float)(rayCount - 1));
            hitsStorage = SystemUtil.RayCast(rayOriginPoint, Vector3.down, rayLength, whatIsGround, Color.black, true);
            
            if (hitsStorage.collider != null)
            {
                this.isGround = true;
                _rigidbody.useGravity = false;
                _rigidbody.velocity = new Vector2(_moveVelocity, 0);
                this.PlayerTrans.position = new Vector3(PlayerTrans.position.x, hitsStorage.point.y + _rayBoundsRectangle.height * 0.5f, 0);
                return;
            }
        }

        this.isGround = false;
        _rigidbody.useGravity = true;

    }

    #endregion

    private void SetRaysParameters()
    {

        _rayBoundsRectangle = new Rect(_playerCollider.bounds.min.x,
                                       _playerCollider.bounds.min.y,
                                       _playerCollider.bounds.size.x,
                                       _playerCollider.bounds.size.y);
#if UNITY_EDITOR
        Debug.DrawLine(new Vector2(_rayBoundsRectangle.center.x, _rayBoundsRectangle.yMin), new Vector2(_rayBoundsRectangle.center.x, _rayBoundsRectangle.yMax), Color.yellow);
        Debug.DrawLine(new Vector2(_rayBoundsRectangle.xMin, _rayBoundsRectangle.center.y), new Vector2(_rayBoundsRectangle.xMax, _rayBoundsRectangle.center.y), Color.yellow);
#endif
    }


    private void CheckBoundsEdge()
    {
        if (!_checkingBounds)
            return;
        if (_bounds.size != Vector3.zero)
        {

            if (_rayBoundsRectangle.yMax > _bounds.max.y)
                ApplyBoundsBehavior(BoundsBehavior.up);

            if (_rayBoundsRectangle.yMin < _bounds.min.y)
                ApplyBoundsBehavior(BoundsBehavior.down);

            if (_rayBoundsRectangle.xMax > _bounds.max.x)
                ApplyBoundsBehavior(BoundsBehavior.right);

            if (_rayBoundsRectangle.xMin < _bounds.min.x)
                ApplyBoundsBehavior(BoundsBehavior.left);
        }
    }

    private void ApplyBoundsBehavior(BoundsBehavior behavior)
    {
        switch (behavior)
        {
            case BoundsBehavior.up:
                LevelMgr.It.GameCubeTurnUp(cubeRotationTime);
                break;
            case BoundsBehavior.down:
                LevelMgr.It.GameCubeTurnDown(cubeRotationTime);
                break;
            case BoundsBehavior.left:
                LevelMgr.It.GameCubeTurnLeft(cubeRotationTime);
                break;
            case BoundsBehavior.right:
                LevelMgr.It.GameCubeTurnRight(cubeRotationTime);
                break;
            default:
                break;
        }

        PlayerTurnSide(behavior);
        StoreState();
    }

    private void PlayerTurnSide(BoundsBehavior behavior)
    {
        switch (behavior)
        {
            case BoundsBehavior.up:
                PlayerTrans.DOMoveY(_bounds.min.y + _rayBoundsRectangle.height * 0.5f + targetOffset, translationTime);
                break;
            case BoundsBehavior.down:
                PlayerTrans.DOMoveY(_bounds.max.y - _rayBoundsRectangle.height * 0.5f - targetOffset, translationTime);
                break;
            case BoundsBehavior.left:
                PlayerTrans.DOMoveX(_bounds.max.x - _rayBoundsRectangle.width * 0.5f - targetOffset, translationTime);
                break;
            case BoundsBehavior.right:
                PlayerTrans.DOMoveX(_bounds.min.x + _rayBoundsRectangle.width * 0.5f + targetOffset, translationTime);
                break;
            default:
                break;
        }
    }


    public void KillSelf()
    {
        SwitchShow();
        ctrlActive = false;
        
        LevelMgr.It.SpawnPlayer();
    }

    public void SwitchShow()
    {
        bool state = gameObject.activeInHierarchy;
        gameObject.SetActive(!state);
    }


    public void StoreState()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _checkingBounds = false;

    }

    public void RevertState()
    {
        
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        _rigidbody.freezeRotation = true;
        _checkingBounds = true;
    }

}
