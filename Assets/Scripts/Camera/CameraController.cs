using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform playerTransform = default;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if(playerTransform != null) 
        {
            Vector3 targetPosition = playerTransform.position + cameraOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

}
