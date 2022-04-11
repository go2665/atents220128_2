// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerDefault"",
            ""id"": ""a92458a0-b9f9-4761-8a1f-b488ebc4982b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""402c3212-fa21-4d05-a4d8-647229b8b9fa"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""5f224c75-2011-42e1-8cb5-5f19c26a1ee1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""482950df-2da8-420c-857e-1f42127437e0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""53c03528-72d1-43dd-9f28-69b88c7a913b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7cb64dc2-422d-49c7-bea0-1b961098b957"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""203bab60-f827-4e16-ad27-999010c2c64e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""309c5a65-12cb-4d1d-83cd-1df7bfdf9986"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d9b1ff1c-51b5-48fb-a58a-2c0a2910726a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""My control scheme"",
            ""bindingGroup"": ""My control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerDefault
        m_PlayerDefault = asset.FindActionMap("PlayerDefault", throwIfNotFound: true);
        m_PlayerDefault_Move = m_PlayerDefault.FindAction("Move", throwIfNotFound: true);
        m_PlayerDefault_UseItem = m_PlayerDefault.FindAction("UseItem", throwIfNotFound: true);
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

    // PlayerDefault
    private readonly InputActionMap m_PlayerDefault;
    private IPlayerDefaultActions m_PlayerDefaultActionsCallbackInterface;
    private readonly InputAction m_PlayerDefault_Move;
    private readonly InputAction m_PlayerDefault_UseItem;
    public struct PlayerDefaultActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerDefaultActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerDefault_Move;
        public InputAction @UseItem => m_Wrapper.m_PlayerDefault_UseItem;
        public InputActionMap Get() { return m_Wrapper.m_PlayerDefault; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerDefaultActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerDefaultActions instance)
        {
            if (m_Wrapper.m_PlayerDefaultActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerDefaultActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerDefaultActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerDefaultActionsCallbackInterface.OnMove;
                @UseItem.started -= m_Wrapper.m_PlayerDefaultActionsCallbackInterface.OnUseItem;
                @UseItem.performed -= m_Wrapper.m_PlayerDefaultActionsCallbackInterface.OnUseItem;
                @UseItem.canceled -= m_Wrapper.m_PlayerDefaultActionsCallbackInterface.OnUseItem;
            }
            m_Wrapper.m_PlayerDefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @UseItem.started += instance.OnUseItem;
                @UseItem.performed += instance.OnUseItem;
                @UseItem.canceled += instance.OnUseItem;
            }
        }
    }
    public PlayerDefaultActions @PlayerDefault => new PlayerDefaultActions(this);
    private int m_MycontrolschemeSchemeIndex = -1;
    public InputControlScheme MycontrolschemeScheme
    {
        get
        {
            if (m_MycontrolschemeSchemeIndex == -1) m_MycontrolschemeSchemeIndex = asset.FindControlSchemeIndex("My control scheme");
            return asset.controlSchemes[m_MycontrolschemeSchemeIndex];
        }
    }
    public interface IPlayerDefaultActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
    }
}
