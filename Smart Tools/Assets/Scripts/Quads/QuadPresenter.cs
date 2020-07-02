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
    private QuadTools _quadTools;

    [Inject]
    private  void Construct(Vector2 modelPos, QuadModel quadModel, PlayerModel player, RoadModel roadModel, QuadTools quadTools)
    {
        quadModel.Position.Value = modelPos;

        _roadModel = roadModel;
        _playerModel = player;
        _quadModel = quadModel;
        _quadTools = quadTools;
        SetModel(_quadModel);
    }

    protected override void OnEnable()
    {
        _quadModel.Position
            .ObserveEveryValueChanged(x => x.Value)
            .FirstOrDefault()
            .Subscribe(xs => { OnPositionChanged(); }).AddTo(Subscriptions);

        _playerModel.Position
                .ObserveEveryValueChanged(x => x.Value)
                .Subscribe(xs => { OnPlayerPositionChanged(); }).AddTo(Subscriptions);
    }

    private void OnPositionChanged()
    {
        transform.localPosition = _quadTools.QuadToWorldPosition(_quadModel.Position.Value);
    }

    private void OnPlayerPositionChanged()
    {
        if (_playerModel.Position.Value.x> _quadModel.Position.Value.x ||
            _playerModel.Position.Value.y> _quadModel.Position.Value.y)
        {
            if (_roadModel.IsQuadStartPanel(this))
                return;

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
