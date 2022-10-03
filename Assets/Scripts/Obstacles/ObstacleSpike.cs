using UnityEngine;
using UnityEngine.Events;

public class ObstacleSpike : MonoBehaviour, IObstacle
{

    [SerializeField] private UnityEvent OnSpawn;
    
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