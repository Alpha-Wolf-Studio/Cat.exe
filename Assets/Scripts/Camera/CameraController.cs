using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float smoothTime = 0.3f;

    [SerializeField] private AnimationCurve rotateCurve;
    [SerializeField] private float rotateDelay;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        playerTransform = GameplayManager.Get().playerController.transform;
    }

    private void FixedUpdate ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position, ref velocity, smoothTime);
    }

    public void Rotate (Vector3 rotation)
    {
        StartCoroutine(RotateDelay(transform.eulerAngles, rotation));
    }

    private IEnumerator RotateDelay (Vector3 origin, Vector3 target)
    {
        float timer = 0f;
        while (timer < rotateDelay)
        {
            timer += Time.deltaTime;
            float lerp = rotateCurve.Evaluate(timer / rotateDelay);

            transform.eulerAngles = Vector3.Lerp(origin, target, lerp);
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = target;
    }
}