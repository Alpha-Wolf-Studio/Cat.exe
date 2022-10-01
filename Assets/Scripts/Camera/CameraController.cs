using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform = default;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float smoothTime = 0.3F;
    
    [SerializeField] private AnimationCurve rotateCurve = null;
    [SerializeField] private float rotateDelay = 0f;

    private Vector3 velocity = Vector3.zero;

    private void Update ()
    {
        transform.position = playerTransform.position;
    }

    private void LateUpdate()
    {
        if (playerTransform != null) 
        {
            Vector3 targetPosition = playerTransform.position + cameraOffset;
            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    public void Rotate(Vector3 rotation)
    {
        StartCoroutine(RotateDelay(cameraTransform.eulerAngles, rotation));
    }

    private IEnumerator RotateDelay(Vector3 origin, Vector3 target)
    {
        float timer = 0f;
        while (timer < rotateDelay)
        {
            timer += Time.deltaTime;
            float lerp = rotateCurve.Evaluate(timer / rotateDelay);

            cameraTransform.eulerAngles = Vector3.Lerp(origin, target, lerp);
            yield return new WaitForEndOfFrame();
        }

        cameraTransform.eulerAngles = target;
    }
}