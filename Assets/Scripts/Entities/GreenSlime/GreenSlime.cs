using System;
using UnityEngine;

public class GreenSlime : MonoBehaviour
{
    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    private static readonly int TransitionDown = Animator.StringToHash("TransitionDown");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");

    private const float MinVelocity = -0.001f;
    private const float DefaultJumpCooldown = 5.0f;
    
    private Animator _Animator;
    private Rigidbody2D _Rigidbody2D;

    private Vector2 _MovementInput;
    
    private float _JumpSpeed = 100.0f;
    private float _JumpCooldown = DefaultJumpCooldown; // Seconds

    private bool _IsGrounded;
    
    void Start()
    {
        _Animator = GetComponentInChildren<Animator>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_JumpCooldown <= 0.0f)
        {
            _Animator.SetTrigger(JumpTrigger);
            _Rigidbody2D.AddForce(Vector2.up * (_JumpSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);
            _JumpCooldown = DefaultJumpCooldown;

            _IsGrounded = false;
        }
        
        if (_Rigidbody2D.linearVelocity.y < MinVelocity)
        {
            _Animator.ResetTrigger(JumpTrigger);
            _Animator.SetTrigger(TransitionDown);
        }

        if (_IsGrounded)
        {
            _Rigidbody2D.MovePosition(_Rigidbody2D.position + _MovementInput * (Time.fixedDeltaTime * 100));
        }
        
        _JumpCooldown -= Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (_IsGrounded)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            _Animator.SetFloat(Horizontal, horizontalInput);
            Debug.Log(horizontalInput);
            _MovementInput = new Vector2(horizontalInput, transform.position.y);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _Animator.ResetTrigger(TransitionDown);
            _IsGrounded = true;
        }
    }
}
