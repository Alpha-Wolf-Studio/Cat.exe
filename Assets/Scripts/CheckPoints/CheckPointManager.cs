using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] private List<CheckPoint> checkPoints = new List<CheckPoint>();

    private static Vector3 lastSpawnPoint;
    private static Vector3 lastSpawnOrientation;

    private void Awake ()
    {
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].ID = i;
            checkPoints[i].OnSaveCheckPoint += SaveLastPosition;
        }
    }

    public void SetEnterCheckPointCallback(Action callback)
    {
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].OnEnterCheckPoint += callback;
        }
    }

    private void SaveLastPosition (Vector3 position, Vector3 orientation)
    {
        lastSpawnPoint = position;
        lastSpawnOrientation = orientation;
    }

    public static void CurrentLastTransform (out Vector3 pos, out Vector3 orientation)
    {
        pos = lastSpawnPoint;
        orientation = lastSpawnOrientation;
    }
}