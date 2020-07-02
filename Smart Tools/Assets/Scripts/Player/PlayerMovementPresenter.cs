using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementPresenter : MonoBehaviour
{
    [SerializeField] float _speed = 10f;

    private bool _isGameStarted = false;

    private PlayerModel _playerModel;
    private RoadModel _roadModel;
    private GameOverModel _gameOverModel;
    private QuadTools _quadTools;
    private Vector3 _movementDirection;

    [Inject]
    private void Construct(PlayerModel playerModel, GameOverModel gameOverModel, RoadModel roadModel, QuadTools quadTools)
    {
        _roadModel = roadModel;
        _playerModel = playerModel;
        _gameOverModel = gameOverModel;
        _quadTools = quadTools;
    }

    private void Awake()
    {
        _movementDirection = Vector3.forward;      
    }

    void Update()
    {
        StartGame();
        ChangeMovementDirection();
        Move();
    }

    private void Move()
    {
        if (_isGameStarted)
        {
            transform.localPosition += _movementDirection * Time.deltaTime * _speed;
            _playerModel.Position.Value = _quadTools.WorldToQuadPosition(new Vector2(transform.localPosition.x, transform.localPosition.z));

            CheckWayPosition();
        }
    }

    private void CheckWayPosition()
    {
        if (_quadTools.IsQuadPosChanged(_playerModel.Position.Value) &&
            !_roadModel.IsThereQuadBeneath(_playerModel.Position.Value))
        {
            _speed = 0f;
            _gameOverModel.GameOver();
        }
    }

    private void ChangeMovementDirection()
    {
        if (Input.GetKeyDown(KeyCode.A) && _isGameStarted)
        {
            _movementDirection = Vector3.forward;
        }

        if (Input.GetKeyDown(KeyCode.D) && _isGameStarted)
        {
            _movementDirection = Vector3.right;
        }
    }

    private void StartGame()
    {
        if (Input.anyKeyDown && !_isGameStarted)
        {
            _isGameStarted = true;
        }
    }
}
