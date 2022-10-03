using UnityEngine;
using UnityEngine.Events;

public class ObstacleMovePlatform : MonoBehaviour
{

    [SerializeField] private UnityEvent OnSpawn = default;

    [Header("Move platform")]
    [SerializeField] private GameObject model = default;
    [SerializeField] private bool rightDirection = false;
    [SerializeField] private float speed = 0;
    [SerializeField] private float rightBound = 0;
    [SerializeField] private float leftBound = 0;

    private void Start()
    {
        OnSpawn?.Invoke();
    }

    private void Update()
    {
        if (rightDirection)
        {
            model.transform.localPosition += Vector3.right * speed * Time.deltaTime;
            if (model.transform.localPosition.x > rightBound) rightDirection = false;
        }
        else
        {
            model.transform.localPosition += Vector3.left * speed * Time.deltaTime;
            if (model.transform.localPosition.x < leftBound) rightDirection = true;
        }
    }
}