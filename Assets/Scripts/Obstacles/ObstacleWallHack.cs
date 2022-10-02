using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleWallHack : MonoBehaviour, IObstacle
{

    [SerializeField] private UnityEvent OnWallStartMoving;
    [SerializeField] private UnityEvent OnWallReachEnd;
    
    [SerializeField] private WallType wallType = default;
    [SerializeField] private float wallForwardSpeed = 5f;
    [SerializeField] private float wallBackwardSpeed = 1f;
    [SerializeField] private Transform wallEndTransform = default;

    [Header("Timing Wall Configuration")]
    [SerializeField] private float wallWaitTime = 2f;

    [Header("Player In Front Wall Configuration")] 
    [SerializeField] private LayerMask playerLayerMask = default;
    [SerializeField] private float wallFrontRaycastDistance = 10f;
    
    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;
    private ChildrenCollision[] childrenCollision = default;

    public enum  WallType
    {
        Timing,
        OnPlayerInFront
    }
    
    private void Start ()
    {
        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }

        startPosition = transform.position;
        endPosition = wallEndTransform.position;
        
        switch (wallType)
        {
            case WallType.Timing:
                StartCoroutine(TimingWallCoroutine());
                break;
            case WallType.OnPlayerInFront:
                StartCoroutine(InFrontPlayerWallCoroutine());
                break;
        }
    }

    public void CheckIsPlayer (Transform other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            IDamageable damageable = other.transform.GetComponent<IDamageable>();
            if (damageable != null)
                Kill(damageable);
        }
    }

    public void Kill (IDamageable damageable)
    {
        damageable.Kill();
    }

    private IEnumerator InFrontPlayerWallCoroutine()
    {
        while (enabled)
        {
            yield return null;
            if (Physics.Raycast(transform.position, transform.right, wallFrontRaycastDistance, playerLayerMask))
            {
                
                OnWallStartMoving?.Invoke();
                
                float t = 0;
                while (t < 1)
                {
                    t += Time.deltaTime * wallForwardSpeed;
                    transform.position = Vector3.Lerp(startPosition, endPosition, t);
                    yield return null;
                }

                transform.position = wallEndTransform.position;
                
                OnWallReachEnd?.Invoke();
            
                while (t > 0)
                {
                    t -= Time.deltaTime * wallBackwardSpeed;
                    transform.position = Vector3.Lerp(startPosition, endPosition, t);
                    yield return null;
                }

                transform.position = startPosition;
            }
        }
    }

    private IEnumerator TimingWallCoroutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(wallWaitTime);
            
            OnWallStartMoving?.Invoke();
            
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * wallForwardSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            transform.position = wallEndTransform.position;
            
            OnWallReachEnd?.Invoke();
            
            while (t > 0)
            {
                t -= Time.deltaTime * wallBackwardSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            transform.position = startPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * wallFrontRaycastDistance);
    }
}