using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed = 10f;

    private bool _isGameStarted = false;

    private PlayerModel _playerModel;
    private Vector3 _movementDirection;

    [Inject]
    private void Construct(PlayerModel playerModel)
    {
        _playerModel = playerModel;
    }

    private void Awake()
    {
        _movementDirection = Vector3.forward;      
    }

    void Update()
    {
        if (Input.anyKeyDown && !_isGameStarted)
        {
            _isGameStarted = true;
        }

        if(Input.GetKeyDown(KeyCode.A) && _isGameStarted)
        {
            _movementDirection = Vector3.forward;
        }

        if (Input.GetKeyDown(KeyCode.D) && _isGameStarted)
        {
            _movementDirection = Vector3.right;
        }

        if (_isGameStarted)
        {
            transform.localPosition += _movementDirection * Time.deltaTime * _speed;
            _playerModel.WorldPosition.Value = new Vector2(transform.localPosition.x, transform.localPosition.z);
        }
    }
}
