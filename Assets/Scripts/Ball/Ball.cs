using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InteractionAnimationPlayer))]
public class Ball : MonoBehaviour
{
    [Header("Movement")]
    public float maxPower;
    [Tooltip("Set gravity to 0 if you want a top down ball game like billiardo.")]
    public float gravity = 1;

    [Tooltip("Allows you to aim and shot even when the ball is still moving.")]
    public bool shootWhileMoving = false;
    [Tooltip("Drag forward to aim instead of reverse aiming.")]
    public bool forwardDraging = true;
    [Tooltip("Show the draging line in the screen so you will not get confused where you aiming")]
    public bool showLineOnScreen = false;
    [Tooltip("Allow you to click whenever in the screen to start aiming, turn it off if you only want to start aiming while clicking in the ball")]
    public bool freeAim = true;
    [Range(0, 1)]
    [SerializeField] private float _slowMotion = 0.7f;

    Transform direction;
    Rigidbody2D rb;
    LineRenderer line;
    LineRenderer screenLine;

    // Vectors // 
    Vector2 startPosition;
    Vector2 targetPosition;
    Vector2 startMousePos;
    Vector2 currentMousePos;

    float shootPower;
    public bool CanShoot { private set; get; } = true;

    public bool CanShootShadow
    {
        get { return _canShootShadow; }
        set
        {
            _canShootShadow = value;
            _animator.SetBool("InteractionSpedUp", _canShootShadow);
        }
    }
    private bool _canShootShadow;
    public float SlowMotion => _slowMotion;
    [SerializeField] private float _minVelocity = 0.5f;
    [SerializeField] private GameObject _shadowPrefab;
    [SerializeField] private GameObject _shadowBouncinesField;
    [SerializeField] private int _sensitivityScale = 12;
    [SerializeField] private float _tailLineDivider = 3;
    [SerializeField] private Transform _spriteTransform;
    private Animator _animator;
    public bool ShadowWasShot { private set; get; }
    private bool _bouncing;

    public void ShootAgain()
    {
        CanShoot = true;
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        GameTimeScaler.ResetTimeScale();
        _animator.SetBool("Interaction", false);
        _animator.SetBool("InteractionSpedUp", false);
    }

    void OnEnable()
    {
        GameTimeScaler.ResetTimeScale();

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        line = GetComponent<LineRenderer>();
        direction = transform.Find("Direction");
        screenLine = direction.GetComponent<LineRenderer>();

        if (_shadowBouncinesField.activeSelf)
            _shadowBouncinesField.SetActive(false);

        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if ((!CanShootShadow && !CanShoot) || TouchInputs.OverUINotClickthrough())
        {
            if (screenLine.enabled)
                DisableEffects();
        }

        if (rb.velocity.magnitude < _minVelocity && !_bouncing)
        {
            float scale = (_minVelocity + 0.1f) / rb.velocity.magnitude;
            Vector2 newVelocity = rb.velocity * scale;
            if (!float.IsNaN(newVelocity.x) || !float.IsNaN(newVelocity.y))
                rb.velocity = newVelocity;
        }

        if (CanShoot)
            if (rb.velocity != Vector2.zero)
                rb.velocity = Vector2.zero;

        if (Ability.AbilityInUse)
            return;

        _DragAndShoot();
    }

    private void _DragAndShoot()
    {
        if (TouchInputs.TouchBegan())
        {
            if (TouchInputs.OverUINotClickthrough()) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            if (freeAim)
                MouseClick();
            else
                BallClick();
        }
        if (isAiming)
        {
            if (Input.touches.Length > 0)
            {
                if (TouchInputs.OverUINotClickthrough()) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
                MouseDrag();
            }
        }

        if (TouchInputs.TouchReleased() && isAiming)
        {
            if (TouchInputs.OverUINotClickthrough()) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            MouseRelease();
        }
    }

    private void DisableEffects()
    {
        screenLine.enabled = false;
        line.enabled = false;
        isAiming = false;
        _shadowBouncinesField.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _spriteTransform.up = (collision.contacts[0].point - (Vector2)_spriteTransform.position).normalized;
        _animator.SetTrigger("Bounce");
        StopAllCoroutines();
        StartCoroutine(BounceAfterAnimation());
    }

    private IEnumerator BounceAfterAnimation()
    {
        yield return new WaitForEndOfFrame();
        Vector2 velocity = rb.velocity;
        rb.velocity = Vector2.zero;
        _bouncing = true;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 0.9f);
        rb.velocity = velocity;
        _bouncing = false;
    }

    private bool objectClicked()
    {
        RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), 0.2f, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            return true;
        }
        return false;
    }


    // MOUSE INPUTS
    void MouseClick()
    {
        isAiming = true;
        if (shootWhileMoving)
        {
            Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            transform.right = dir * 1;

            startMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
        {
            if (CanShoot)
            {
                if (_shadowBouncinesField.activeSelf)
                    _shadowBouncinesField.SetActive(false);

                Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                startMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            else
            {
                if (!ShadowWasShot && CanShootShadow)
                {
                    Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                    startMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                    _shadowBouncinesField.SetActive(true);
                }
            }
        }

    }

    private bool isAiming = false;

    void BallClick()
    {
        if (!objectClicked())
            return;

        isAiming = true;

        if (shootWhileMoving)
        {
            Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            transform.right = dir * 1;

            startMousePos = transform.position;
        }
        else
        {
            if (CanShoot)
            {
                Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                transform.right = dir * 1;

                startMousePos = transform.position;
            }
        }

    }
    void MouseDrag()
    {
        if (!freeAim)
            startMousePos = transform.position;

        if (shootWhileMoving)
        {
            //Aim();
            //DrawLine();

            if (showLineOnScreen)
                DrawScreenLine();

            float distance = Vector2.Distance(currentMousePos, startMousePos);

            if (distance > 0.1f)
            {
                line.enabled = true;

                if (showLineOnScreen)
                    screenLine.enabled = true;
            }
        }
        else
        {
            if (CanShoot || (!ShadowWasShot && CanShootShadow))
            {
                Aim();
                DrawLine();

                if (showLineOnScreen)
                    DrawScreenLine();

                float distance = Vector2.Distance(currentMousePos, startMousePos);

                if (distance > 0.1f)
                {
                    line.enabled = true;
                    if (CanShootShadow && !CanShoot)
                        _shadowBouncinesField.SetActive(true);
                    if (showLineOnScreen)
                        screenLine.enabled = true;
                }
            }
        }

    }
    void MouseRelease()
    {
        if (shootWhileMoving /*&& !EventSystem.current.IsPointerOverGameObject()*/)
        {
            Shoot();
            screenLine.enabled = false;
            line.enabled = false;
            //GameTimeScaler.ResetTimeScale();
            isAiming = false;
        }
        else
        {
            if (CanShoot /*&& !EventSystem.current.IsPointerOverGameObject()*/)
            {
                Shoot();
                screenLine.enabled = false;
                line.enabled = false;
                GameTimeScaler.ResetTimeScale();
                isAiming = false;
            }
            else
            {
                if (!ShadowWasShot && CanShootShadow)
                {
                    ShootShadow();
                    screenLine.enabled = false;
                    line.enabled = false;
                    _shadowBouncinesField.SetActive(false);
                    GameTimeScaler.ResetTimeScale();
                    isAiming = false;
                }
            }
        }
    }


    // ACTIONS  
    void Aim()
    {
        Vector2 dir = startMousePos - currentMousePos;

        float dis = Vector2.Distance(currentMousePos, startMousePos);
        dis *= _sensitivityScale;


        if (dis < maxPower)
        {
            direction.localPosition = dir.normalized * (dis / _tailLineDivider);
            shootPower = dis;
        }
        else
        {
            shootPower = maxPower;
            direction.localPosition = dir.normalized * (maxPower / _tailLineDivider);
        }

    }
    private void Shoot()
    {
        if ((direction.position - transform.position).normalized == Vector3.zero)
            return;
        CanShoot = false;
        rb.velocity = (direction.position - transform.position).normalized * shootPower;
    }

    private void ShootShadow()
    {
        if ((direction.position - transform.position).normalized == Vector3.zero)
            return;
        ShadowWasShot = true;
        CanShootShadow = false;
        GameObject shadow = Instantiate(_shadowPrefab, transform.position, Quaternion.identity);
        Rigidbody2D shadowRb = shadow.GetComponent<Rigidbody2D>();
        shadowRb.velocity = (direction.position - transform.position).normalized * shootPower;
    }

    void DrawScreenLine()
    {
        screenLine.positionCount = 1;
        screenLine.SetPosition(0, startMousePos);

        screenLine.positionCount = 2;

        screenLine.SetPosition(1, currentMousePos);
    }

    void DrawLine()
    {

        startPosition = transform.position;

        line.positionCount = 1;
        line.SetPosition(0, startPosition);


        targetPosition = direction.transform.position;
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        line.positionCount = 2;
        line.SetPosition(1, targetPosition);
    }

    Vector3[] positions;


}
