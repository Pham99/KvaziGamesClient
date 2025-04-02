using System.Collections.Generic;
using System;
using UnityEngine;

public class InputMap
{
    public ButtonInput left = new ButtonInput();
    public ButtonInput right = new ButtonInput();
    public ButtonInput top = new ButtonInput();
    public ButtonInput bottom = new ButtonInput();
    public ButtonInput a = new ButtonInput();
    public ButtonInput b = new ButtonInput();
    public ButtonInput x = new ButtonInput();
    public ButtonInput y = new ButtonInput();
    public readonly Dictionary<string, Action> InputActions = new();

    public InputMap()
    {
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

    public class ButtonInput
    {
        private bool _buttonDown = false;
        //private bool _buttonUp = false;
        private bool _buttonPressed = false;

        public void OnButtonDown()
        {
            _buttonDown = true;
        }

        public void OnButtonUp()
        {
            _buttonDown = false;
        }

        public bool OnButtonPressed()
        {
            return _buttonPressed;
        }

        public bool GetInput()
        {
            return _buttonDown;
        }
    }
}
