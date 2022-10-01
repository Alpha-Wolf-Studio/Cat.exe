using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed = 0f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private LayerMask jumpeableMask = default;

    private Rigidbody rigid = null;
    private float halfHeight = 0f;
    private bool dead = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        halfHeight = capsuleCollider.height / 2 + 0.05f;
    }

    private void Update()
    {
        if (dead) return;

        Jump();
    }

    private void FixedUpdate()
    {
        if (dead) return;

        InputMovement();
    }

    private void InputMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            AddMovement(transform.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            AddMovement(-transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            AddMovement(transform.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            AddMovement(-transform.right);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position, Vector3.down, halfHeight, jumpeableMask))
            {
                rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void AddMovement(Vector3 direction)
    {
        rigid.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    public void Kill ()
    {
        dead = true;
    }

    public void Respawn ()
    {
        dead = false;
    }
}
