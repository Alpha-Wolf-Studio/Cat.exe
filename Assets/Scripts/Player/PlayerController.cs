using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private MovementController movementController = null;

    private bool dead = false;

    private void Update()
    {
        if (dead) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementController.Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            movementController.Dash();
        }
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
            movementController.AddMovement(transform.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementController.AddMovement(-transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementController.AddMovement(transform.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementController.AddMovement(-transform.right);
        }
    }

    public void Kill ()
    {
        dead = true;
        //Timer se para
        //Animacion de muerte
        //Desaparece el modelo
    }

    public void Respawn ()
    {
        //Se usa el ultimo checkpoint del CheckPointManager para volver a posicionar al player
        Vector3 newPos = default;
        Vector3 newRot = default;
        CheckPointManager.CurrentLastTransform(out newPos, out newRot);
        transform.Translate(newPos);
        transform.Rotate(newRot);
        //Animacion de Respawn
        //Se resetea el Timer
        dead = false;
    }
}
