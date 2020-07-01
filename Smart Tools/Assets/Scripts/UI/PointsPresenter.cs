using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class PointsPresenter : MonoBehaviour
{
    [SerializeField] Text _pointText;
    PointsModel _pointsModel;

    [Inject]
    private void Construct(PointsModel pointsModel)
    {
        _pointsModel = pointsModel;
    }

    private void OnEnable()
    {
        _pointsModel.OnPointsReceived += ShowPoints;
    }

    private void OnDisable()
    {
        _pointsModel.OnPointsReceived -= ShowPoints;
    }

    private void ShowPoints(float points)
    {
        _pointText.text = points.ToString();
    }
}
