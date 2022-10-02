using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpitterVirus : MonoBehaviour
{
    [Header("Ball spawn")]
    [SerializeField] private float timePerBall = 0f;
    [SerializeField] private bool firstBallWithoutTimer = default;
    [SerializeField] private Transform ballSpawn = default;
    [SerializeField] private Vector3 forceBallSpawn = Vector3.zero;

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
        GameObject ball = objectPooler.SpawnFromPool("Spike", ballSpawn.position, Quaternion.identity);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.AddForce(forceBallSpawn);

        ballTimer.Reset();
        ballTimer.ToggleTimer(true);
    }
}