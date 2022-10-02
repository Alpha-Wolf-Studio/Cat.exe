using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    [SerializeField] private float rotationSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private LayerMask jumpeableMask = default;
    [SerializeField] private float dashForce = 0f;
    [SerializeField] private float dashCooldown = 0.2f;
    [SerializeField] private Animator animator = null;

    private Rigidbody rigid = null;
    private bool canDash = true;
    private float halfHeight = 0f;

    private void Start ()
    {
        rigid = GetComponent<Rigidbody>();
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        halfHeight = capsuleCollider.height / 2 + 0.05f;
    }

    public void AddMovement (Vector3 direction)
    {
        rigid.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Acceleration);

        ProcessRotation(direction);
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

    public void Jump ()
    {
        if (Physics.Raycast(transform.position, Vector3.down, halfHeight, jumpeableMask))
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ProcessRotation (Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}