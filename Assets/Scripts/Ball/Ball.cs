using System.Collections;
using UnityEngine;

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
    bool canShoot = true;

    public bool CanShootShadow;
    public float SlowMotion => _slowMotion;
    [SerializeField] private float _minVelocity = 0.5f;
    [SerializeField] private GameObject _shadowPrefab;
    [SerializeField] private GameObject _shadowBouncinesField;
    [SerializeField] private int _sensitivityScale = 12;
    [SerializeField] private float _tailLineDivider = 3;
    [SerializeField] private Transform _spriteTransform;
    private Animator _animator;
    private bool _shadowWasShot;

    void OnEnable()
    {
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
        if ((!CanShootShadow && !canShoot) || TouchInputs.OverUINotClickthrough())
        {
            if (screenLine.gameObject.activeSelf)
                DisableEffects();

        }

        if (rb.velocity.magnitude < _minVelocity)
        {
            float scale = (_minVelocity + 0.1f) / rb.velocity.magnitude;
            Vector2 newVelocity = rb.velocity * scale;
            if (!float.IsNaN(newVelocity.x) || !float.IsNaN(newVelocity.y))
                rb.velocity = newVelocity;
        }

        if (Ability.AbilityIsUsed)
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
        if (TouchInputs.TouchDragged() && isAiming)
        {
            if (TouchInputs.OverUINotClickthrough()) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            MouseDrag();
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
        GameTimeScaler.ResetTimeScale();
        isAiming = false;
        _shadowBouncinesField.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _spriteTransform.up = (collision.contacts[0].point - (Vector2)_spriteTransform.position).normalized;
        _animator.SetTrigger("Bounce");
        StartCoroutine(BounceAfterAnimation());
    }

    private IEnumerator BounceAfterAnimation()
    {
        yield return new WaitForEndOfFrame();
        Vector2 velocity = rb.velocity;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 0.9f);
        rb.velocity = velocity;
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
            if (canShoot)
            {
                GameTimeScaler.ChangeTimeScale(_slowMotion);

                Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                //transform.right = dir * 1;

                startMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            else
            {
                if (!_shadowWasShot && CanShootShadow)
                {
                    GameTimeScaler.ChangeTimeScale(_slowMotion);

                    Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    //transform.right = dir * 1;

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
            if (canShoot)
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
            Aim();
            DrawLine();

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
            if (canShoot || (!_shadowWasShot && CanShootShadow))
            {
                Aim();
                DrawLine();

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
        }

    }
    void MouseRelease()
    {
        if (shootWhileMoving /*&& !EventSystem.current.IsPointerOverGameObject()*/)
        {
            Shoot();
            screenLine.enabled = false;
            line.enabled = false;
        }
        else
        {
            if (canShoot /*&& !EventSystem.current.IsPointerOverGameObject()*/)
            {
                Shoot();
                screenLine.enabled = false;
                line.enabled = false;
            }
            else
            {
                if (!_shadowWasShot && CanShootShadow)
                {
                    ShootShadow();
                    screenLine.enabled = false;
                    line.enabled = false;
                    _shadowBouncinesField.SetActive(false);
                }
            }
        }

        isAiming = false;

        GameTimeScaler.ResetTimeScale();
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
        canShoot = false;
        rb.velocity = (direction.position - transform.position).normalized * shootPower;
    }

    private void ShootShadow()
    {
        _shadowWasShot = true;
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