using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InputMap
{
    private readonly Dictionary<NetKeyCode, ButtonInput> _buttons = new();
    public readonly Dictionary<string, Action> InputActions = new();

    public InputMap()
    {
        _buttons.Add(NetKeyCode.Up, new ButtonInput());
        _buttons.Add(NetKeyCode.Down, new ButtonInput());
        _buttons.Add(NetKeyCode.Left, new ButtonInput());
        _buttons.Add(NetKeyCode.Right, new ButtonInput());
        _buttons.Add(NetKeyCode.A, new ButtonInput());
        _buttons.Add(NetKeyCode.B, new ButtonInput());
        _buttons.Add(NetKeyCode.X, new ButtonInput());
        _buttons.Add(NetKeyCode.Y, new ButtonInput());

        ButtonInput top = _buttons[NetKeyCode.Up];
        ButtonInput bottom = _buttons[NetKeyCode.Down];
        ButtonInput left = _buttons[NetKeyCode.Left];
        ButtonInput right = _buttons[NetKeyCode.Right];
        ButtonInput a = _buttons[NetKeyCode.A];
        ButtonInput b = _buttons[NetKeyCode.B];
        ButtonInput x = _buttons[NetKeyCode.X];
        ButtonInput y = _buttons[NetKeyCode.Y];

        InputActions.Add("leftd", () => left.OnButtonDown());
        InputActions.Add("leftu", () => left.OnButtonUp());
        InputActions.Add("rightd", () => right.OnButtonDown());
        InputActions.Add("rightu", () => right.OnButtonUp());
        InputActions.Add("upd", () => top.OnButtonDown());
        InputActions.Add("upu", () => top.OnButtonUp());
        InputActions.Add("downd", () => bottom.OnButtonDown());
        InputActions.Add("downu", () => bottom.OnButtonUp());
        InputActions.Add("ad", () => a.OnButtonDown());
        InputActions.Add("au", () => a.OnButtonUp());
        InputActions.Add("bd", () => b.OnButtonDown());
        InputActions.Add("bu", () => b.OnButtonUp());
        InputActions.Add("xd", () => x.OnButtonDown());
        InputActions.Add("xu", () => x.OnButtonUp());
        InputActions.Add("yd", () => y.OnButtonDown());
        InputActions.Add("yu", () => y.OnButtonUp());
    }

    public void SetInput(string input)
    {
        var action = InputActions[input];
        action.Invoke();
    }

    public bool GetKey(NetKeyCode key)
    {
        ButtonInput currentKey = _buttons[key];
        return currentKey.GetInput();
    }
    public bool GetKeyDown(NetKeyCode key)
    {
        ButtonInput currentKey = _buttons[key];
        return currentKey.GetInputDown();
    }
    public bool GetKeyUp(NetKeyCode key)
    {
        ButtonInput currentKey = _buttons[key];
        return currentKey.GetInputUp();
    }
    public void UpdateInputMap()
    {
        foreach (var button in _buttons.Values)
        {
            button.ApplyToInputMap();
        }
    }

    public class ButtonInput
    {
        private bool _buttonPressed = false;
        private bool _buttonDownCached = false;
        private bool _buttonUpCached = false;
        private bool _buttonPressedCached = false;

        public void ApplyToInputMap()
        {
            _buttonDownCached = !_buttonPressedCached && _buttonPressed;
            _buttonUpCached = _buttonPressedCached && !_buttonPressed;
            _buttonPressedCached = _buttonPressed;
        }
        public void OnButtonDown()
        {
            _buttonPressed = true;
        }

        public void OnButtonUp()
        {
            _buttonPressed = false;
        }

        public bool GetInputDown()
        {
            return _buttonDownCached;
        }
        public bool GetInputUp()
        {
            return _buttonUpCached;
        }
        public bool GetInput()
        {
            return _buttonPressedCached;
        }
    }
}
public enum NetKeyCode
{
    Up,
    Down,
    Left,
    Right,
    A,
    B,
    X,
    Y
}
