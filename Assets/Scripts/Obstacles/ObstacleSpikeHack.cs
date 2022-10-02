using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ObstacleSpikeHack : MonoBehaviour, IObstacle
{
    [SerializeField] private Transform animatorSpike;
    [SerializeField] private float startTime;

    [Header("Movement Up")] 
    [SerializeField] private float timeToUp = 1;
    [SerializeField] private AnimationCurve curveUp;
    private Vector3 maxPosition;

    [Header("Stay Up")]
    [SerializeField] private float timeToStayUp = 0.5f;
    [SerializeField] private AnimationCurve curveStayUp;
    
    [Header("Movement Down")] 
    [SerializeField] private float timeToDown = 1;
    [SerializeField] private AnimationCurve curveDown;
    private Vector3 minPosition;

    [Header("Stay Down")] 
    [SerializeField] private float timeToStayDown = 0.5f;
    [SerializeField] private AnimationCurve curveStayDown;
    
    private ChildrenCollision[] childrenCollision;
    float deltaTime;
    float lerp;

    private void Start ()
    {
        deltaTime = (startTime == 0) ? (Random.Range(0, timeToUp + timeToDown + timeToStayDown + timeToStayUp)) : startTime;

        maxPosition = animatorSpike.transform.localPosition;
        minPosition = maxPosition;
        minPosition.y = 0;

        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }

        StartCoroutine(SpikeMoveCoroutine());
    }

    private IEnumerator SpikeMoveCoroutine ()
    {
        while (true) // AnimateAlways
        {
            while (deltaTime < timeToUp)
            {
                SetPositionByCurve(curveUp);
                yield return null;
            }
            deltaTime -= timeToUp;

            while (deltaTime < timeToStayUp)
            {
                SetPositionByCurve(curveStayUp);
                yield return null;
            }
            deltaTime -= timeToStayUp;

            while (deltaTime < timeToDown)
            {
                SetPositionByCurve(curveDown);
                yield return null;
            }
            deltaTime -= timeToDown;

            while (deltaTime < timeToStayDown)
            {
                SetPositionByCurve(curveStayDown);
                yield return null;
            }
            deltaTime -= timeToStayDown;

            deltaTime = 0;
            yield return null;
        }
    }

    void SetPositionByCurve (AnimationCurve curve)
    {
        deltaTime += Time.deltaTime;
        lerp = deltaTime / timeToUp;
        Vector3 pos = Vector3.Lerp(minPosition, maxPosition, curve.Evaluate(lerp));
        animatorSpike.localPosition = pos;
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