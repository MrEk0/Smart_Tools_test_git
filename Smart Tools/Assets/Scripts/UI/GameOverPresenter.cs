using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameOverPresenter : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    private GameOverModel _gameOverModel;

    [Inject]
    private void Construct(GameOverModel gameOverModel)
    {
        _gameOverModel = gameOverModel;
    }

    private void OnEnable()
    {
        _gameOverModel.OnGameOver += ShowGameOverPanel;
    }

    private void OnDisable()
    {
        _gameOverModel.OnGameOver -= ShowGameOverPanel;
    }

    private void Update()
    {
        if(Input.anyKeyDown && _gameOverModel.IsGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}
