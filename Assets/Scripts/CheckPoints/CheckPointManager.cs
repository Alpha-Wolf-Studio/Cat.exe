using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] private List<CheckPoint> checkPoints = new List<CheckPoint>();

    public static CheckPoint lastCheckPoint;

    private void Awake ()
    {
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].ID = i;
            checkPoints[i].OnSaveCheckPoint += SaveLastPosition;
        }
    }

    public void SetCheckPointCallbacks(Action enterCallback, Action endCallback)
    {
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].OnEnterCheckPoint += enterCallback;
            checkPoints[i].OnExitCheckPoint += endCallback;
        }
    }

    private void SaveLastPosition (CheckPoint checkPoint)
    {
        lastCheckPoint = checkPoint;
    }

    public static void CurrentLastTransform (out Vector3 pos, out Vector3 orientation)
    {
        pos = lastCheckPoint.GetPositionSpawn();
        orientation = lastCheckPoint.GetRotationSpawn();
    }
}