using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovePlatform : MonoBehaviour
{
    [Header("Move platform")]
    public bool rightDirection = false;
    public float speed = 0;
    public int rightBound = 0;
    public int leftBound = 0;

    private float initialXPosition = 0;

    private void Start()
    {
        initialXPosition = transform.position.x;
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