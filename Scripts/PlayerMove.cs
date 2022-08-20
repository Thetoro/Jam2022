using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private float _jumpForce = 10;
    private Rigidbody2D _rb;
    private PlayerMoveControl playerControl;
    private Vector2 _move;
    [SerializeField]
    private Transform _groundCheckCollider;
    [SerializeField]
    private LayerMask _groundLayer;
    private float _groundCheckRadio = 0.01f;
    [SerializeField]
    private float _dashDistance = 15f;
    private bool _jump;
    private bool _dash;
    private bool _isGrounded;
    private bool _isDashing;
    private bool _isReadyToDash;
    private bool _isMoving;
    private bool _isFacingLeft;

    private float _wallJumpTime = 0.01f;
    private float _slidingSpeed = 0.2f;
    [SerializeField]
    private float _wallDistance = 0.5f;
    private bool _isSliding;
    private RaycastHit2D _wallCheckHit;
    private float _jumpTime;

    private Animator _animator;
    private AnimatorClipInfo[] _currentAniamtionInfo;
    private string _currentAnimName;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = new PlayerMoveControl();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void onMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
        _isMoving = context.action.triggered;
    }

    public void onJump(InputAction.CallbackContext context)
    {
        _jump = context.action.triggered;
    }

    public void onDash(InputAction.CallbackContext context)
    {
        _dash = context.action.triggered;
    }

    private void Update() 
    {
        IsGrounded();
        Movement();
        WallJump();
    }

    private void Movement()
    {
        if(!_isDashing)
        {
            _rb.velocity = new Vector2(_move.x * _speed * 100 * Time.deltaTime, _rb.velocity.y);
            _animator.SetFloat("Walk_velocity", Mathf.Abs(_rb.velocity.x));
        }

        if(_isMoving && _isGrounded && !_isDashing)
        {
            if(!_audioManager.IsPlaying("Caminar"))
                _audioManager.Play("Caminar");
        }
        else
        {
            _audioManager.Stop("Caminar");
        }
            


        if(_isGrounded && _jump || _isSliding && _jump)
        {
            _audioManager.Play("Salto");
            _animator.SetBool("IsJumping", true);
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }

        if(_dash && _isReadyToDash)
        {
            StartCoroutine(Dash());
        }

        if(_rb.velocity.x > 0 && _isFacingLeft)
            Flip();
        if(_rb.velocity.x < 0 && !_isFacingLeft)
            Flip();
    }

    private void WallJump()
    {
        if(!_isFacingLeft)
        {
            _wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(_wallDistance, 0), _wallDistance, _groundLayer);
            Debug.DrawRay(transform.position, new Vector2(_wallDistance, 0), Color.red);
        }
        if(_isFacingLeft)
        {
            _wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-_wallDistance, 0), _wallDistance, _groundLayer);
            Debug.DrawRay(transform.position, new Vector2(-_wallDistance, 0), Color.red);
        }

        if(_wallCheckHit && !_isGrounded && _isMoving)
        {
            _isSliding = true;
            _jumpTime = Time.time + _wallJumpTime;
        }
        else if(_jumpTime < Time.time)
        {
            _isSliding = false;
            _animator.SetBool("IsTre", false);
        }

        if(_isSliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, _slidingSpeed, float.MaxValue));
            _animator.SetBool("IsTre", true);
            _animator.SetBool("IsJumping", false);
        }
    }

    private void IsGrounded()
    {
        _isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheckCollider.position, _groundCheckRadio, _groundLayer);
        if(colliders.Length > 0)
        {
            _isGrounded = true;
            _isReadyToDash = true;
            _animator.SetBool("IsJumping", false);
            _animator.SetBool("IsTre", false);
            _animator.SetBool("IsDashing", false);
        }
            
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        _isFacingLeft = !_isFacingLeft;
    }

    IEnumerator Dash()
    {
        _isDashing = true;
        _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsDashing", true);
        if(_isMoving)
        {
            _audioManager.Play("Dash");
            _rb.AddForce(_move * _dashDistance, ForceMode2D.Impulse);
        }
        if(!_isMoving)
        {
            if(_isFacingLeft)
            {
                _audioManager.Play("Dash");
                _rb.AddForce(new Vector2(-1f, 0) * _dashDistance, ForceMode2D.Impulse);
            }
            if(!_isFacingLeft)
            {
                _audioManager.Play("Dash");
                _rb.AddForce( new Vector2(1f, 0) * _dashDistance, ForceMode2D.Impulse);
            }
        }
        _rb.gravityScale = 0;
        yield return new WaitForSeconds(0.4f);
        _isDashing = false;
        _rb.gravityScale = 3f;
        _isReadyToDash = false;
    }
}
