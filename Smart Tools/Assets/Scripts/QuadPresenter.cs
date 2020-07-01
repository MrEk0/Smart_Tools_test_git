using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;


public class QuadPresenter : ReactivePresenter<QuadModel>
{
    private QuadModel _quadModel;
    public QuadModel QuadModel=>_quadModel;

    private PlayerModel _playerModel;
    private RoadModel _roadModel;

    [Inject]
    private  void Construct(Vector2 modelPos, QuadModel quadModel, PlayerModel player, RoadModel roadModel)
    {
        quadModel.Position.Value = modelPos;

        _roadModel = roadModel;
        _playerModel = player;
        _quadModel = quadModel;
        SetModel(_quadModel);
    }

    protected override void OnEnable()
    {
        _quadModel.Position
            .ObserveEveryValueChanged(x => x.Value)
            .FirstOrDefault()
            .Subscribe(xs => { OnPositionChanged(); }).AddTo(Subscriptions);

        _playerModel.WorldPosition
                .ObserveEveryValueChanged(x => x.Value)
                .Subscribe(xs => { OnPlayerPositionChanged(); }).AddTo(Subscriptions);
    }

    private void OnPositionChanged()
    {
        name = "Quad " + _quadModel.Position.Value;
        transform.localPosition = new Vector3(2 * _quadModel.Position.Value.x, 0, 2 * _quadModel.Position.Value.y);
    }

    private void OnPlayerPositionChanged()
    {
        if (!_roadModel.IsQuadExist())
            return;

        Vector2Int playerPos = _playerModel.WorldToQuadPosition();

        if (playerPos.x-3> _quadModel.Position.Value.x ||
            playerPos.y-3> _quadModel.Position.Value.y)
        {
            gameObject.SetActive(false);
            _roadModel.ResetRoad();
        }
    }

    public Vector2 GetPosition()
    {
        return _quadModel.Position.Value;
    }
    public class Factory : PlaceholderFactory<Vector2, QuadPresenter>
    {

    }
}
