using UnityEngine;
using UnityEngine.Events;

public class ObstacleDeathZone : MonoBehaviour, IObstacle
{

    [SerializeField] private UnityEvent OnSpawn = default;
    
    private ChildrenCollision[] childrenCollision;
    private void Start ()
    {
        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }
        
        OnSpawn?.Invoke();
    }

    private void OnCollisionEnter (Collision other)
    {
        CheckIsPlayer(other.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckIsPlayer(other.transform);
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
}
