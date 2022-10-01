using UnityEngine;

public class ObstacleSpike : MonoBehaviour, IObstacle
{


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter (Collision other)
    {
        if (other.transform.CompareTag("Player"))
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