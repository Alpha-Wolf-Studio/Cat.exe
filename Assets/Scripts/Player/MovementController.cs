using System.Collections;

using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private LayerMask jumpeableMask = default;
    [SerializeField] private float dashForce = 0f;
    [SerializeField] private float dashCooldown = 0.2f;

    private Rigidbody rigid = null;
    private bool canDash = true;
    private float halfHeight = 0f;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        halfHeight = capsuleCollider.height / 2 + 0.05f;
    }

    public void AddMovement(Vector3 direction)
    {
        rigid.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    public void Dash()
    {
        IEnumerator SetDashCoolDown()
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        if (!canDash) return;
        
        rigid.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        canDash = false;

        StartCoroutine(SetDashCoolDown());
    }

    public void Jump()
    {        
        if (Physics.Raycast(transform.position, Vector3.down, halfHeight, jumpeableMask))
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
