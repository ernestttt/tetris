using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputManager : IInitializable, IFixedTickable
{
    [Inject] private GameConfig _gameConfig;
    private float step;

    public event Action<Command[]> OnGetNewInput;

    private List<Command> _commands = new();
    private readonly Dictionary<KeyCode, bool> _pressedButtons = new();
    private readonly Dictionary<KeyCode, float> _buttonNextTimes = new();
    private readonly Dictionary<KeyCode, Command> _commandsMap;

    public InputManager()
    {
        _commandsMap = new Dictionary<KeyCode, Command>()
        {
            {KeyCode.UpArrow, Command.Rotate},
            {KeyCode.DownArrow, Command.Down},
            {KeyCode.LeftArrow, Command.Left},
            {KeyCode.RightArrow, Command.Right},
        };
    }

    public void Initialize()
    {
        step = _gameConfig.Frequency;
    }

    public void FixedTick()
    {
        _commands.Clear();
        
        CheckKey(KeyCode.UpArrow);
        CheckKey(KeyCode.DownArrow, .1f);
        CheckKey(KeyCode.LeftArrow, .5f);
        CheckKey(KeyCode.RightArrow, .5f);
        
        if (_commands.Count > 0)
        {
            OnGetNewInput?.Invoke(_commands.ToArray());
        }
    }

    private void CheckKey(KeyCode key, float scale = 1)
    {
        if (Input.GetKeyUp(key))
        {
            SetButtonPressedValue(key, false);
        }
        else if (Input.GetKey(key))
        {
            bool isButtonPressed = IsButtonPressed(key);
            if (isButtonPressed)
            {
                if(Time.time > GetButtonNextTime(key))
                {
                    UpdateCommandBuffer(key, .25f * scale);
                }
            }
            else
            {
                SetButtonPressedValue(key, true);
                UpdateCommandBuffer(key, scale);
            }
        }
    }

    private void UpdateCommandBuffer(KeyCode key, float speedCoefficient)
    {
        Command command = GetCommandForKey(key);
        if (command != Command.Empty)
        {
            _commands.Add(GetCommandForKey(key));
        }
        AddButtonNextTime(key, Time.fixedTime + step * speedCoefficient);
    }

    private void SetButtonPressedValue(KeyCode key, bool value)
    {
        if (_pressedButtons.ContainsKey(key))
        {
            _pressedButtons[key] = value;
        }
        else
        {
            _pressedButtons.Add(key, value);
        }
    }
    
    private Command GetCommandForKey(KeyCode key)
    {
        Command command = Command.Empty;
        if (_commandsMap.ContainsKey(key))
        {
            command = _commandsMap[key];
        }

        return command;
    }

    private float GetButtonNextTime(KeyCode key)
    {
        if (_buttonNextTimes.ContainsKey(key))
        {
            return _buttonNextTimes[key];
        }

        return float.MaxValue;
    }

    private void AddButtonNextTime(KeyCode key, float nextTime)
    {
        if (_buttonNextTimes.ContainsKey(key))
        {
            _buttonNextTimes[key] = nextTime;
        }
        else
        {
            _buttonNextTimes.Add(key, nextTime);
        }
    }

    private bool IsButtonPressed(KeyCode key)
    {
        bool result = false;
        if (_pressedButtons.ContainsKey(key))
        {
            result = _pressedButtons[key];
        }
        else
        {
            _pressedButtons.Add(key, false);
        }

        return result;
    }
        
}


public enum Command
{
    Empty = 0,
    Down,
    Left,
    Right,
    Rotate,
}