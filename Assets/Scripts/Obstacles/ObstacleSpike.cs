using UnityEngine;

public class ObstacleSpike : MonoBehaviour, IObstacle
{
    [Header("Spike")]
    [SerializeField] private float timeActive = 0f;

    private Timer activeTimer = default;
    private ChildrenCollision[] childrenCollision;

    private void Start ()
    {
        activeTimer = new Timer(timeActive, default, true, null, DesactiveSpike);

        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }
    }

    private void Update()
    {
        activeTimer.Update(Time.deltaTime);
    }

    private void DesactiveSpike()
    {
        gameObject.SetActive(false);
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