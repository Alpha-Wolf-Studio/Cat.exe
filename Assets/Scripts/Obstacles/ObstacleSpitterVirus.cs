using UnityEngine;
using UnityEngine.Events;

public class ObstacleSpitterVirus : MonoBehaviour
{

    [SerializeField] private UnityEvent OnSpit = default;

    [Header("Ball spawn")]
    [SerializeField] private float timePerBall = 0f;
    [SerializeField] private bool firstBallWithoutTimer = default;
    [SerializeField] private Transform ballSpawn = default;
    [SerializeField] private float forceBallSpeed = 500f;
    [SerializeField] private ObstacleSpike obstacleSpike = default;

    private ObstacleSpike lastSpike = default;
    private ObjectPooler objectPooler = default;
    private Timer ballTimer = default;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        ballTimer = new Timer(timePerBall, default, true, null, Instanciate2DBall);
        if (firstBallWithoutTimer) Instanciate2DBall();
    }

    private void Update()
    {
        ballTimer.Update(Time.deltaTime);
    }

    private void Instanciate2DBall()
    {
        if(lastSpike) Destroy(lastSpike.gameObject);
        lastSpike = Instantiate(obstacleSpike, ballSpawn.position, Quaternion.identity, ballSpawn);
        Rigidbody ballRigidbody = lastSpike.gameObject.GetComponent<Rigidbody>();
        ballRigidbody.AddForce(-transform.right * forceBallSpeed);

        ballTimer.Reset();
        ballTimer.ToggleTimer(true);
        OnSpit?.Invoke();
    }
}