using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class DiamondPresenter : ReactivePresenter<DiamondModel>
{
    private DiamondModel _diamondModel;
    public DiamondModel DiamondModel => _diamondModel;
    private PlayerModel _playerModel;
    private PointsModel _pointsModel;

    [Inject]
    private void Construct(DiamondModel diamondModel, Vector2 modelPosition, PlayerModel playerModel, PointsModel pointsModel)
    {
        _diamondModel = diamondModel;
        _diamondModel.Position.Value = modelPosition;
        _playerModel = playerModel;
        _pointsModel = pointsModel;
        SetModel(_diamondModel);
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
        name = "Diamond " + _diamondModel.Position.Value;
        transform.localPosition = new Vector3(2 * _diamondModel.Position.Value.x, 0.5f, 2 * _diamondModel.Position.Value.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
            _pointsModel.ReceivePoints();
        }
    }

    public class Factory : PlaceholderFactory<Vector2, DiamondPresenter>
    {

    }
}
