using System.Collections.Generic;
using UnityEngine;
using Tetris;
using TMPro;
using Zenject;

public class TetrisManager : IInitializable, ITickable
{
    [Inject] private GameConfig _gameConfig;
    [Inject] private TetrisGrid _tetrisGrid;
    [Inject] private InputManager _inputManager;
    [Inject] private Menu _menu;
    
    private bool _isPlaying;
    private float _step;
    private float _nextTime = 0;
    private readonly TetrisCore _tetris = new TetrisCore();
    
    public void Initialize()
    {
        _step = _gameConfig.Frequency;
        _tetris.GameOverEvent += () =>
        {
            _isPlaying = false;
            Debug.Log("Game Over");
        };
        _inputManager.OnGetNewInput += SetComands;
        _isPlaying = true;
    }

    
    public void Tick()
    {
        if (!_isPlaying) 
            return;
        
        if (Time.time > _nextTime)
        {
            _tetris.MoveDown();
            _nextTime = Time.time + _step;
            SetComand(Command.Down);
        }
        
        _menu.UpdateScoreText(_tetris.CompletedRows);
    }

    private void SetComands(Command[] commands)
    {
        for (int i = 0; i < commands.Length; i++)
        {
            SetComand(commands[i]);
        }
    }
    
    
    private void SetComand(Command command)
    {
        switch (command)
        {
            case Command.Down:
                _tetris.MoveDown();
                break;
            case Command.Left:
                _tetris.MoveLeft();
                break;
            case Command.Right:
                _tetris.MoveRight();
                break;
            case Command.Rotate:
                _tetris.Rotate();
                break;
        }

        _tetrisGrid.Draw(_tetris.ViewMatrix);
    }
}
