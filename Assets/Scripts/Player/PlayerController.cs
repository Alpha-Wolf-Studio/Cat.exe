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
        cameraTransform = GameplayManager.Get().cameraController.transform;
    }

    private void Start()
    {
        dissolveEffect.SetDissolve(0);
    } 

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
            movementController.AddMovement(cameraTransform.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementController.AddMovement(-cameraTransform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementController.AddMovement(cameraTransform.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementController.AddMovement(-cameraTransform.right);
        }
    }

    public void Kill()
    {
        OnDeath?.Invoke();
        dead = true;
        //Animacion de muerte
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
        //Animacion de Respawn
        dissolveEffect.SetDissolve(0);
        //Se resetea el Timer
        dead = false;
    }
}
