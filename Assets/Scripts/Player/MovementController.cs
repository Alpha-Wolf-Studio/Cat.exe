using System;
using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 0f;
    [SerializeField] private float rotationSpeed = 0f;
    [Header("Jump")] 
    [SerializeField] private float maxJumpTime = .25f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private LayerMask jumpeableMask = default;
    [Header("Dash")]
    [SerializeField] private float dashForce = 0f;
    [SerializeField] private float dashCooldown = 0.2f;
    [Space(10)]
    [SerializeField] private Animator animator = null;

    private Rigidbody rigid = null;
    private bool canDash = true;
    private float halfHeight = 0f;
    
    private bool jumping = false;
    private float jumpDelta = 0;
    
    private void Start ()
    {
        rigid = GetComponent<Rigidbody>();
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        halfHeight = capsuleCollider.height / 2 + 0.05f;
    }

    private void Update()
    {
        if (!jumping) return;
        
        jumpDelta += Time.deltaTime;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (jumpDelta > maxJumpTime)
            JumpEnd();
    }

    public void AddMovement (Vector3 direction)
    {
        animator.SetBool("run", true);

        rigid.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Acceleration);

        ProcessRotation(direction);
    }

    public void StopRunningAnimation()
    {
        animator.SetBool("run", false);
    }

    public void Dash ()
    {
        IEnumerator SetDashCoolDown ()
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        if (!canDash) return;

        animator.SetTrigger("dash");
        canDash = false;

        StartCoroutine(SetDashCoolDown());
    }

    public void MovementDash()
    {
        rigid.AddForce(transform.forward * dashForce, ForceMode.Impulse);
    }

    public void TriggerDeathAnimation()
    {
        animator.SetTrigger("death");
    }
    
    public void JumpStart()
    {
        if (Physics.Raycast(transform.position, Vector3.down, halfHeight, jumpeableMask))
        {
            animator.SetTrigger("jump");
            jumping = true;
        }
    }
    
    public void JumpEnd()
    {
        jumping = false;
        jumpDelta = 0;
    }
    
    private void ProcessRotation (Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}