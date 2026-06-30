using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class InputManager : MonoBehaviour
{
    public enum DeviceType
    {
        KeyboardMouse,
        Gamepad
    }

    public static event Action<DeviceType, string> OnDeviceChanged;
    public static event Action<bool> OnSelectionModeChanged;

    private DeviceType _currentDeviceType;
    private string _currentDeviceName;
    private bool _currentSelectionAllowed = true;

    void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var action = obj as InputAction;
        var control = action?.activeControl;
        var device = control?.device;
        if (device == null) return;

        if (device is Mouse)
        {
            HandleDeviceSwitch(DeviceType.KeyboardMouse, "Mouse");
            SetSelectionAllowed(false);
            return;
        }

        if (device is Keyboard)
        {
            HandleDeviceSwitch(DeviceType.KeyboardMouse, "Keyboard");
            SetSelectionAllowed(true);
            return;
        }

        if (device is Gamepad)
        {
            string gamepadName = GetGamepadDisplayName(device);
            HandleDeviceSwitch(DeviceType.Gamepad, gamepadName);
            SetSelectionAllowed(true);
            return;
        }
    }

    private string GetGamepadDisplayName(InputDevice device)
    {
        switch (device)
        {
            case DualSenseGamepadHID:
                return "PlayStation 5 (DualSense)";

            case DualShockGamepad:
                return "PlayStation 4 (DualShock)";

            case XInputController:
                return "Xbox";

            default: //Fallback
                string layout = device.layout;

                if (layout.Contains("DualSense"))
                    return "PlayStation 5 (DualSense)";

                if (layout.Contains("DualShock"))
                    return "PlayStation 4 (DualShock)";

                if (layout.Contains("Switch"))
                    return "Nintendo Switch Pro Controller";

                return $"Generic Gamepad ({device.displayName})";
        }
    }

    private void HandleDeviceSwitch(DeviceType type, string name)
    {
        if (_currentDeviceType == type && _currentDeviceName == name) return;

        _currentDeviceType = type;
        _currentDeviceName = name;

        Debug.Log($"[InputManager] Gewechselt zu: {type} -> {name}");
        OnDeviceChanged?.Invoke(type, name);
    }

    private void SetSelectionAllowed(bool allowed)
    {
        if (_currentSelectionAllowed == allowed) return;

        _currentSelectionAllowed = allowed;
        OnSelectionModeChanged?.Invoke(allowed);
    }
}