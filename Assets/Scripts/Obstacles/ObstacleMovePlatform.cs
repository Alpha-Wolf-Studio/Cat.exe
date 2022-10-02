using UnityEngine;
using UnityEngine.Events;

public class ObstacleMovePlatform : MonoBehaviour
{

    [SerializeField] private UnityEvent OnSpawn = default;
    
    [Header("Move platform")]
    [SerializeField] private bool rightDirection = false;
    [SerializeField] private float speed = 0;
    [SerializeField] private int rightBound = 0;
    [SerializeField] private int leftBound = 0;

    private float initialXPosition = 0;

    private void Start()
    {
        initialXPosition = transform.position.x;
        OnSpawn?.Invoke();
    }

    private void Update()
    {
        if (rightDirection)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= (initialXPosition + rightBound)) rightDirection = false;
        }
        else
        {
            transform.Translate(Vector2.right * -speed * Time.deltaTime);
            if (transform.position.x <= (initialXPosition + leftBound)) rightDirection = true;
        }
    }
}