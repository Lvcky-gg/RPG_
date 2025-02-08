using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : Entity
{
    [Header("Dash Info")]
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashDuration;

    private float dashTime;
    [SerializeField]
    private float dashCooldown;
    private float dashCooldownTimer;
    [Header("Attack Info")]
    private bool isAttacking;

    private int combatCounter;
    private float comboTimeCounter;
    [SerializeField]
    private float comboTime = 1f;

    [SerializeField]
    private float xInput;
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpForce;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Movement();
        Inputs();
        Dash();
        comboTimeCounter -= Time.deltaTime;
        FlipController();
        AnimatorControllers();
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", combatCounter);
    }
    private void Dash()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }
    private void Movement()
    {
        if (isAttacking)
            rb.velocity = new Vector2(0, 0);
        else if (dashTime > 0)
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        else
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

    }

    public void AttackOver()
    {
        isAttacking = false;
        combatCounter++;
        if (combatCounter > 2)
            combatCounter = 0;
    }
    private void Inputs()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        StartAttackEvent();
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
    private void StartAttackEvent()
    {
        if (!isGrounded)
            return;
        if (comboTimeCounter < 0)
            combatCounter = 0;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttacking = true;
            comboTimeCounter = comboTime;
        }
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();

    }

}

