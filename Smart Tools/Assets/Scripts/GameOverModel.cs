using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameOverModel
{
    private bool _isGameOver = false;
    public bool IsGameOver => _isGameOver;

    public event Action OnGameOver;

    public void GameOver()
    {
        _isGameOver = true;
        OnGameOver();
    }
}
