using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointsModel 
{
    private float _points = 0;
    public event Action<float> OnPointsReceived;

    public void ReceivePoints()
    {
        _points++;
        OnPointsReceived(_points);
    }
}
