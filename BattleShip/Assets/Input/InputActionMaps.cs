//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Input/InputActionMaps.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputActionMaps : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActionMaps()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActionMaps"",
    ""maps"": [
        {
            ""name"": ""BattleField"",
            ""id"": ""ad57ac18-4493-46a8-904a-a86933da8649"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""b1e52697-a6e3-4bb7-95b7-f207437265df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""Value"",
                    ""id"": ""d41a0cd0-7bb1-4184-97d1-c8e40b3aad77"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseWheel"",
                    ""type"": ""Value"",
                    ""id"": ""65257828-f7be-4df1-a9be-e2250fbfe960"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8631c86d-9e6c-4de7-a16f-8233d7398941"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eacfe3ed-c80f-4843-912a-705d094cd1fd"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a8af40d-ba00-42d1-9707-4b0691eff09e"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // BattleField
        m_BattleField = asset.FindActionMap("BattleField", throwIfNotFound: true);
        m_BattleField_Click = m_BattleField.FindAction("Click", throwIfNotFound: true);
        m_BattleField_MouseMove = m_BattleField.FindAction("MouseMove", throwIfNotFound: true);
        m_BattleField_MouseWheel = m_BattleField.FindAction("MouseWheel", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // BattleField
    private readonly InputActionMap m_BattleField;
    private IBattleFieldActions m_BattleFieldActionsCallbackInterface;
    private readonly InputAction m_BattleField_Click;
    private readonly InputAction m_BattleField_MouseMove;
    private readonly InputAction m_BattleField_MouseWheel;
    public struct BattleFieldActions
    {
        private @InputActionMaps m_Wrapper;
        public BattleFieldActions(@InputActionMaps wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_BattleField_Click;
        public InputAction @MouseMove => m_Wrapper.m_BattleField_MouseMove;
        public InputAction @MouseWheel => m_Wrapper.m_BattleField_MouseWheel;
        public InputActionMap Get() { return m_Wrapper.m_BattleField; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleFieldActions set) { return set.Get(); }
        public void SetCallbacks(IBattleFieldActions instance)
        {
            if (m_Wrapper.m_BattleFieldActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnClick;
                @MouseMove.started -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnMouseMove;
                @MouseMove.performed -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnMouseMove;
                @MouseMove.canceled -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnMouseMove;
                @MouseWheel.started -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnMouseWheel;
                @MouseWheel.performed -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnMouseWheel;
                @MouseWheel.canceled -= m_Wrapper.m_BattleFieldActionsCallbackInterface.OnMouseWheel;
            }
            m_Wrapper.m_BattleFieldActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @MouseMove.started += instance.OnMouseMove;
                @MouseMove.performed += instance.OnMouseMove;
                @MouseMove.canceled += instance.OnMouseMove;
                @MouseWheel.started += instance.OnMouseWheel;
                @MouseWheel.performed += instance.OnMouseWheel;
                @MouseWheel.canceled += instance.OnMouseWheel;
            }
        }
    }
    public BattleFieldActions @BattleField => new BattleFieldActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IBattleFieldActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnMouseMove(InputAction.CallbackContext context);
        void OnMouseWheel(InputAction.CallbackContext context);
    }
}