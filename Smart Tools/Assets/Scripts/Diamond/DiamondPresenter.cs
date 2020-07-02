using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class DiamondPresenter : ReactivePresenter<DiamondModel>
{
    private DiamondModel _diamondModel;
    public DiamondModel DiamondModel => _diamondModel;
    private PointsModel _pointsModel;
    private QuadTools _quadTools;

    private float height;

    [Inject]
    private void Construct(DiamondModel diamondModel, Vector2 modelPosition, PointsModel pointsModel, QuadTools quadTools)
    {
        _diamondModel = diamondModel;
        _diamondModel.Position.Value = modelPosition;
        _pointsModel = pointsModel;
        _quadTools = quadTools;
        SetModel(_diamondModel);
    }

    private void Awake()
    {
        height = transform.localScale.y * 0.5f;
    }
    protected override void OnEnable()
    {
        _diamondModel.Position
            .ObserveEveryValueChanged(x => x.Value)
            .FirstOrDefault()
            .Subscribe(xs => { OnPositionChanged(); }).AddTo(Subscriptions);
    }

    private void OnPositionChanged()
    {
        Vector3 worldPos= _quadTools.QuadToWorldPosition(_diamondModel.Position.Value);
        worldPos.y = height;
        transform.localPosition = worldPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovementPresenter>())
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
            _pointsModel.ReceivePoints();
        }
    }

    public class Factory : PlaceholderFactory<Vector2, DiamondPresenter>
    {

    }
}
