using System.Collections;
using UnityEngine;

public class ObstacleSpikeHack : MonoBehaviour, IObstacle
{

    [Header(("No Action Configuration"))]
    [SerializeField] private float spikeNoActionTime = 5f;
    [Header("Wobble Configuration")]
    [SerializeField] private float spikeWobbleTime = 2f;
    [SerializeField] private float spikeWobbleAnimationSpeed = 1f;

    private readonly string WOBBLE_ANIMATION_SPEED = "WobbleSpeed";
    private readonly string WOBBLE_ANIMATION_TRIGGER = "Wobble";
    
    [Header("Start Configuration")]
    [SerializeField] private float spikeStartAnimationSpeed = 1f;
    
    private readonly string START_ANIMATION_SPEED = "StartSpeed";
    private readonly string START_ANIMATION_TRIGGER = "Start";
    
    private Animator spikeAnimator;
    private ChildrenCollision[] childrenCollision;
    private void Start ()
    {
        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }

        spikeAnimator = GetComponent<Animator>();
        
        spikeAnimator.SetFloat(WOBBLE_ANIMATION_SPEED, spikeWobbleAnimationSpeed);
        spikeAnimator.SetFloat(START_ANIMATION_SPEED, spikeStartAnimationSpeed);

        StartCoroutine(SpikeMoveCoroutine());
    }
    private void OnCollisionEnter (Collision other)
    {
        CheckIsPlayer(other.transform);
    }

    private IEnumerator SpikeMoveCoroutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(spikeNoActionTime);
            spikeAnimator.SetTrigger(WOBBLE_ANIMATION_TRIGGER);
            yield return new WaitForSeconds(spikeWobbleTime);
            spikeAnimator.SetTrigger(START_ANIMATION_TRIGGER);
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
}
