using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer line;
    public float Direction { get; set; } = 0.0f;
    public int PositionCount => line.positionCount;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void SetPosition(int index, Vector3 position)
    {
        line.SetPosition(index,position);
    }

    public Vector3 GetPosition(int index) => line.GetPosition(index);

    private void FixedUpdate()
    {
        Vector3 start = line.GetPosition(0);
        Vector3 end = line.GetPosition(line.positionCount - 1);
        Vector3 middle = CalculateMiddlePoint(end, start);
        for (int i = 1; i < line.positionCount; i++)
        {
            line.SetPosition(i,BezierCurve(start,middle,end,(float)i/(float)line.positionCount));
        }
    }

    private Vector3 CalculateMiddlePoint(Vector3 p1, Vector3 p0)
    {
        Vector3 scaledDir = (p1 - p0)*0.25f;
        Vector3 right = Vector3.Cross(Vector3.up, scaledDir);
        return p1 + scaledDir + right * Direction;
    }



    private Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
    }
}
