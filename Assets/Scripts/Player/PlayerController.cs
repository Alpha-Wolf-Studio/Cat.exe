using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public event Action OnDeath;
    private MovementController movementController;
    private Transform cameraTransform;

    [Header("Visual Effects")]
    [SerializeField] private DissolveEffect dissolveEffect;

    private bool dead = false;

    private void Awake ()
    {
        movementController = GetComponent<MovementController>();
    }

    private void Start()
    {
        cameraTransform = GameplayManager.Get().cameraController.transform;
        dissolveEffect.SetDissolve(0);
    } 

    private void Update()
    {
        if (dead) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementController.JumpStart();
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            movementController.JumpEnd();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftShift))
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
        bool moving = false;
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += cameraTransform.forward;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction += -cameraTransform.forward;
            moving = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += cameraTransform.right;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction += -cameraTransform.right;
            moving = true;
        }

        if (!moving)
        {
            movementController.StopRunningAnimation(); 
        }
        else
        {
            movementController.AddMovement(direction);
        }
    }

    public void Kill()
    {
        OnDeath?.Invoke();
        dead = true;
        movementController.TriggerDeathAnimation();
        dissolveEffect.SetDissolve(1); //Desaparece el modelo
    }

    public void Respawn()
    {
        //Se usa el ultimo checkpoint del CheckPointManager para volver a posicionar al player
        Vector3 newPos = default;
        Vector3 newRot = default;
        CheckPointManager.CurrentLastTransform(out newPos, out newRot);
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(newRot);
        dissolveEffect.SetDissolve(0);//Reaparece el modelo
        //Se resetea el Timer
        dead = false;
    }
}
