using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovePlatform : MonoBehaviour, IObstacle
{
    [Header("Move platform")]
    public GameObject platform = null;
    public bool rightDirection = false;
    public float speed = 0;
    public int rightBound = 0;
    public int leftBound = 0;

    private void Update()
    {
        if (rightDirection)
        {
            platform.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (platform.transform.localPosition.x >= rightBound) rightDirection = false;
        }
        else
        {
            platform.transform.Translate(Vector2.right * -speed * Time.deltaTime);
            if (platform.transform.localPosition.x <= leftBound) rightDirection = true;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        CheckIsPlayer(other.transform);
    }

    public void CheckIsPlayer(Transform other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            IDamageable damageable = other.transform.GetComponent<IDamageable>();
            if (damageable != null)
                Kill(damageable);
        }
    }

    public void Kill(IDamageable damageable)
    {
        damageable.Kill();
    }
}
