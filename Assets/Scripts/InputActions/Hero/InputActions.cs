// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputActions/Hero/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Cannon"",
            ""id"": ""0d65baf9-b6cd-4660-990a-9ff9fd69c2fd"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""958809c7-044b-461b-9f8e-e74de2b3ee10"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""994c6972-2ff5-4729-bc78-ae35ca27f896"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""a6e89d8c-be9a-487e-8514-48cccd111c4c"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6b6439c4-b102-455f-a206-9f8833ef4e60"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""520cbc26-2206-42cb-ad7e-bd578b62e3e4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8d856d1a-97bf-4211-af8b-f9f94c464ecd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""97005938-5af4-4dd4-b459-c3fbde144b7f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""96a2518f-8c82-40ae-961a-09640b0b0b30"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""efa3d2e0-10f6-4038-985d-5d45f278525d"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""104f191b-42c5-4caa-9643-8aaa73866146"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0e8faf79-590a-4b2a-aa33-f1c608af83d3"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Horse"",
            ""id"": ""645e2f87-424c-4aa7-ad28-16dd94d76307"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""e8a81a77-39e6-49e3-a0b8-ec0eb4587d4f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aaa74ee3-6136-4a3e-b18b-61def3c7325e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""ac38da3a-5b32-42d4-9864-789a0cc6bd80"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d1750354-98cf-4f4b-a1a5-a56a938fb19e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fca28aeb-b4f7-456e-b6b2-a02d74bcc8b8"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""93b20d32-acf0-4414-8bab-1d3d99519515"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""efb1fa2b-1d1e-4325-9b91-f51298420edd"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ad1dfde1-4f8e-4036-8346-f28efbfe94b0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ee2c7eb2-2178-4bca-b74d-3d1aaed1c986"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2a14195f-df3b-4bee-bf7c-51789a597f8f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""33b6646d-9791-44be-8d25-7a0c2311bbbf"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Human"",
            ""id"": ""61c399be-65e2-4a51-87f0-a464ee41b9e8"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""dd6c743b-bd80-4a21-af63-ad4b59b6bb00"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""176238a0-3302-482c-bdf6-d0e784587f89"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gas"",
                    ""type"": ""Button"",
                    ""id"": ""0df5cc1a-472c-4b37-85c9-4b8644b2f2e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e61bca94-440a-4621-9aef-46b73413a305"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""8d8efd07-1e09-4ab5-8631-838b04387cf4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Salute"",
                    ""type"": ""Button"",
                    ""id"": ""57d72e55-cd54-4c4c-baee-02c768d889ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""11f0e088-0d2b-426e-90f9-b6bcc1b24286"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reel MnK"",
                    ""type"": ""Value"",
                    ""id"": ""25ac873f-4bcc-4699-a8df-01a7e58ad379"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reel In"",
                    ""type"": ""Button"",
                    ""id"": ""4ed94db8-e47c-4bfa-99b9-f7d4311d1df4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reel Out"",
                    ""type"": ""Button"",
                    ""id"": ""3f0af906-4de9-4c1f-a52e-001aa3d927c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""03147d22-7901-4c5c-88d1-57fed5a2287e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Special Attack"",
                    ""type"": ""Button"",
                    ""id"": ""714cff8d-3d95-40d8-802a-9796d66f6f46"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hook Left"",
                    ""type"": ""Button"",
                    ""id"": ""6c0408b2-97e6-45e2-9f6c-6537a34bbd37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hook Right"",
                    ""type"": ""Button"",
                    ""id"": ""f22e2fd3-946c-4686-a47f-812e53267acd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hook Both"",
                    ""type"": ""Button"",
                    ""id"": ""454961c1-f543-4abe-9388-1c93882c44b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Item 1"",
                    ""type"": ""Button"",
                    ""id"": ""891abdc1-e574-4d28-bbe7-66adb5b36b1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Item 2"",
                    ""type"": ""Button"",
                    ""id"": ""d16a1ffc-d508-4f7a-8320-86737c336ffd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Item 3"",
                    ""type"": ""Button"",
                    ""id"": ""041d3015-4695-49dd-a9c2-3d15b37a4cf9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Focus"",
                    ""type"": ""Button"",
                    ""id"": ""99a2e8f1-94c4-4de6-9ef1-a20b01a0bd6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gas Burst Double Tap"",
                    ""type"": ""Value"",
                    ""id"": ""6c74a549-3296-4c4f-b367-355ccbaa4a4e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(pressPoint=0.2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""978bfe49-cc26-4a3d-ab7b-7d7a29327403"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.5),NormalizeVector2"",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Directionals"",
                    ""id"": ""00ca640b-d935-4593-8157-c05846ea39b3"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e2062cb9-1b15-46a2-838c-2f8d72a0bdd9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""320bffee-a40b-4347-ac70-c210eb8bc73a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d2581a9b-1d11-4566-b27d-b92aff5fabbc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcfe95b8-67b9-4526-84b5-5d0bc98d6400"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c1f7a91b-d0fd-4a62-997e-7fb9b69bf235"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c8e490b-c610-4785-884f-f04217b23ca4"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60fa84b5-c5e0-4a48-9403-78b66c3db634"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Gas"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca2c5d95-d1cc-4ff6-ae95-4b177ae025e7"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Gas"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74b97249-6b00-4850-8198-7a46f95957bc"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Hook Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""689e0bcd-31ff-4087-97c5-565fdaebc259"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Hook Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b26c817f-a159-4fdc-9248-38d331b3752f"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d9d3744-d814-47c2-82bc-3624b2d6824f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""258e7008-9bca-4aa9-8687-b343eff9b5d0"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7e44ab5-e863-4151-9929-9a91e2b4c775"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73df3ba3-13b8-44bb-9684-890fdff3c6eb"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Salute"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2723cdc3-0f41-4928-b881-d202db84ffdf"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Salute"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9932489-568c-46c5-b70f-1089181f7fb6"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acfe0338-5727-4c2c-ac1c-0b9e34f7c007"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2634e3e0-57da-4ffe-8975-04c1cc795392"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone(min=0.5)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reel In"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a179e90-a97e-4d92-9b07-d55765e8f44f"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone(min=0.5)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reel Out"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f84e21c3-3b86-4de1-87ef-309f48420061"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""984d1fc4-1cad-407c-952a-e3fd45f5c206"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66aa541d-aa2a-4e02-b327-5afaa42f417c"",
                    ""path"": ""<Mouse>/backButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Special Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1a4d67f-d521-4f43-941f-ebd66af9c435"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Special Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66b831f8-346d-42c8-9499-ce744ae7fc2a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Hook Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3363847a-c6c3-44ba-b5c8-9bb8cf655f87"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Hook Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c7a0760-95c9-4af8-83f7-372106b413d6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Hook Both"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""422c5d50-b4bc-4b49-b38b-022c1c08d1c5"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Hook Both"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5440495d-e2cf-4c9c-ad64-3e18e3556402"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Item 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a968fcb1-8e61-449c-a2be-4e8a801ea2f1"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Item 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbc73dd8-89f1-423c-960a-bb2b96d4fb7b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Item 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b247615-1339-4dcb-be05-12bfc667965d"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Item 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4da6fa0a-36ab-4f24-8ddd-064ebfcbcb11"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Item 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f131b2cd-70da-4737-b932-f1059f9d30cb"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Item 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e342d598-641c-4860-9506-a68708853a51"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Focus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2eea8017-73ad-4ad0-9544-d9d77336b495"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Focus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Direction"",
                    ""id"": ""775dae95-7369-4bd9-9f37-0817fed0cf79"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gas Burst Double Tap"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cea5fdef-8b7d-4a1a-a0c1-b3f58e4d95a9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gas Burst Double Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""51c47d29-1b8f-4d4f-a842-c4528ae69c5d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gas Burst Double Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3fc2b502-f8e1-4c68-9d2f-8ef60d525d6b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gas Burst Double Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2c82eed8-4465-436d-b794-5f6a175686ab"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gas Burst Double Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""29cb5e46-9c9f-4cdd-abce-903cfeecfcd5"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Reel MnK"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Titan"",
            ""id"": ""6c2330f0-c4ac-4dba-aa4e-fd6ed446ef40"",
            ""actions"": [],
            ""bindings"": []
        },
        {
            ""name"": ""UI"",
            ""id"": ""b74f6dc4-544d-4174-8422-9463852b9e4d"",
            ""actions"": [
                {
                    ""name"": ""Chat"",
                    ""type"": ""Button"",
                    ""id"": ""b52c63d4-005a-4db1-befb-8c2d660594e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle Cursor"",
                    ""type"": ""Button"",
                    ""id"": ""516b6c76-563e-4a17-ab26-d915129db970"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Live Camera"",
                    ""type"": ""Button"",
                    ""id"": ""5385bfec-f097-46d8-bdf1-fa15a9e45de6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Minimap"",
                    ""type"": ""Button"",
                    ""id"": ""c37f4758-c2bf-40da-a5f4-9b2a0a636b37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fullscreen"",
                    ""type"": ""Button"",
                    ""id"": ""ac460f60-1ceb-4a79-b183-ac92dd31be1e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera Mode"",
                    ""type"": ""Button"",
                    ""id"": ""a6442db8-af21-4504-a242-832de0ae3f76"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""becb342c-5b0d-4116-bfb1-4dd7cfc148b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""76cf9f0e-f771-4912-b88c-3dd82064fa7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interaction Wheel"",
                    ""type"": ""Button"",
                    ""id"": ""b48e7565-8f83-43d7-ac66-8e1c11d31695"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Screenshot"",
                    ""type"": ""Button"",
                    ""id"": ""ae615db7-46ad-4ac2-a0c3-8c293f319ec7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""944bbf27-e206-4a58-bcdb-e92a60ac0091"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d1656b06-1b1a-49f7-a249-b479753af391"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""64e16d16-f213-4083-9d37-fcfa50309dd7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""06479da2-6d9b-4229-b4e9-8bedb9b4bb9c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5d8fbad3-8b4e-4803-9210-1fb422eac44e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2aaa6871-74f0-494a-8f1b-50e9b8079262"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9ed53074-0033-40e4-bb10-f837bd847dc6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""a71ec49f-dc60-4b6b-92ce-9996ba94b817"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""af5666b3-8424-4579-ad5b-85910da176ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""ba05ed05-35aa-468e-9f7c-3f8c85bb4fac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""24f0c687-3710-4408-95c3-bad1e2714041"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Chat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be018e7c-7e00-47b1-a376-d76434280bb5"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Toggle Cursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0260df6c-ae9c-419b-ac4b-da531d9fb32f"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Live Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""584d0c1c-ebf5-4bd5-9cd2-6d5acd48273e"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Minimap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""694f8233-26ef-4a63-bf42-82b1d0e447fa"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Fullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50675986-c0ac-4eec-9c34-c1e2ab0e2cba"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Camera Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be850e69-95b4-48f7-aed8-fd738476a57f"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac367ffa-a11b-48db-b792-bb1e4b2e80d8"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""524c7a09-9571-40d8-98ec-99fe5a17cbab"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e053001-6653-4320-afc5-95eaf66e3d0b"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Interaction Wheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73e99ce1-a3f0-455d-8110-bc1f5a0a7664"",
                    ""path"": ""<Keyboard>/f12"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Screenshot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b562b2b-f6e9-4d72-ae67-67a342423bbc"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Screenshot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""e4db7457-eba9-4170-9bfb-a21f83503a3e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""65169160-8a9c-4335-b345-4b690a09d516"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""de5632da-997f-4dd0-8067-42b4206736ad"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""35aca343-dda7-4311-8346-15da2ed7679b"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3ef44da6-8dd3-4016-857f-91125ac97e19"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""59aa2e34-7029-4bf2-a354-61b14b2eedd2"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d47bf276-ee90-44e6-8702-51c56fe2d85e"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f925060e-f998-44d6-a919-e1aa1bac8778"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cdfa00b2-5fdd-4ea8-9b73-bfeabed6cd17"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""00447264-d548-489c-90f0-f4a44607ef47"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13d662ab-40bc-4b2e-aa68-53dfba4f9696"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""476dfca6-6715-4fa7-821c-7837aca1ba9d"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
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
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Cannon
        m_Cannon = asset.FindActionMap("Cannon", throwIfNotFound: true);
        m_Cannon_Move = m_Cannon.FindAction("Move", throwIfNotFound: true);
        // Horse
        m_Horse = asset.FindActionMap("Horse", throwIfNotFound: true);
        m_Horse_Move = m_Horse.FindAction("Move", throwIfNotFound: true);
        // Human
        m_Human = asset.FindActionMap("Human", throwIfNotFound: true);
        m_Human_Move = m_Human.FindAction("Move", throwIfNotFound: true);
        m_Human_Look = m_Human.FindAction("Look", throwIfNotFound: true);
        m_Human_Gas = m_Human.FindAction("Gas", throwIfNotFound: true);
        m_Human_Jump = m_Human.FindAction("Jump", throwIfNotFound: true);
        m_Human_Dodge = m_Human.FindAction("Dodge", throwIfNotFound: true);
        m_Human_Salute = m_Human.FindAction("Salute", throwIfNotFound: true);
        m_Human_Reload = m_Human.FindAction("Reload", throwIfNotFound: true);
        m_Human_ReelMnK = m_Human.FindAction("Reel MnK", throwIfNotFound: true);
        m_Human_ReelIn = m_Human.FindAction("Reel In", throwIfNotFound: true);
        m_Human_ReelOut = m_Human.FindAction("Reel Out", throwIfNotFound: true);
        m_Human_Attack = m_Human.FindAction("Attack", throwIfNotFound: true);
        m_Human_SpecialAttack = m_Human.FindAction("Special Attack", throwIfNotFound: true);
        m_Human_HookLeft = m_Human.FindAction("Hook Left", throwIfNotFound: true);
        m_Human_HookRight = m_Human.FindAction("Hook Right", throwIfNotFound: true);
        m_Human_HookBoth = m_Human.FindAction("Hook Both", throwIfNotFound: true);
        m_Human_Item1 = m_Human.FindAction("Item 1", throwIfNotFound: true);
        m_Human_Item2 = m_Human.FindAction("Item 2", throwIfNotFound: true);
        m_Human_Item3 = m_Human.FindAction("Item 3", throwIfNotFound: true);
        m_Human_Focus = m_Human.FindAction("Focus", throwIfNotFound: true);
        m_Human_GasBurstDoubleTap = m_Human.FindAction("Gas Burst Double Tap", throwIfNotFound: true);
        // Titan
        m_Titan = asset.FindActionMap("Titan", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Chat = m_UI.FindAction("Chat", throwIfNotFound: true);
        m_UI_ToggleCursor = m_UI.FindAction("Toggle Cursor", throwIfNotFound: true);
        m_UI_LiveCamera = m_UI.FindAction("Live Camera", throwIfNotFound: true);
        m_UI_Minimap = m_UI.FindAction("Minimap", throwIfNotFound: true);
        m_UI_Fullscreen = m_UI.FindAction("Fullscreen", throwIfNotFound: true);
        m_UI_CameraMode = m_UI.FindAction("Camera Mode", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Restart = m_UI.FindAction("Restart", throwIfNotFound: true);
        m_UI_InteractionWheel = m_UI.FindAction("Interaction Wheel", throwIfNotFound: true);
        m_UI_Screenshot = m_UI.FindAction("Screenshot", throwIfNotFound: true);
        m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
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

    // Cannon
    private readonly InputActionMap m_Cannon;
    private ICannonActions m_CannonActionsCallbackInterface;
    private readonly InputAction m_Cannon_Move;
    public struct CannonActions
    {
        private @InputActions m_Wrapper;
        public CannonActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Cannon_Move;
        public InputActionMap Get() { return m_Wrapper.m_Cannon; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CannonActions set) { return set.Get(); }
        public void SetCallbacks(ICannonActions instance)
        {
            if (m_Wrapper.m_CannonActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CannonActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CannonActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CannonActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_CannonActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public CannonActions @Cannon => new CannonActions(this);

    // Horse
    private readonly InputActionMap m_Horse;
    private IHorseActions m_HorseActionsCallbackInterface;
    private readonly InputAction m_Horse_Move;
    public struct HorseActions
    {
        private @InputActions m_Wrapper;
        public HorseActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Horse_Move;
        public InputActionMap Get() { return m_Wrapper.m_Horse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HorseActions set) { return set.Get(); }
        public void SetCallbacks(IHorseActions instance)
        {
            if (m_Wrapper.m_HorseActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_HorseActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_HorseActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_HorseActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_HorseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public HorseActions @Horse => new HorseActions(this);

    // Human
    private readonly InputActionMap m_Human;
    private IHumanActions m_HumanActionsCallbackInterface;
    private readonly InputAction m_Human_Move;
    private readonly InputAction m_Human_Look;
    private readonly InputAction m_Human_Gas;
    private readonly InputAction m_Human_Jump;
    private readonly InputAction m_Human_Dodge;
    private readonly InputAction m_Human_Salute;
    private readonly InputAction m_Human_Reload;
    private readonly InputAction m_Human_ReelMnK;
    private readonly InputAction m_Human_ReelIn;
    private readonly InputAction m_Human_ReelOut;
    private readonly InputAction m_Human_Attack;
    private readonly InputAction m_Human_SpecialAttack;
    private readonly InputAction m_Human_HookLeft;
    private readonly InputAction m_Human_HookRight;
    private readonly InputAction m_Human_HookBoth;
    private readonly InputAction m_Human_Item1;
    private readonly InputAction m_Human_Item2;
    private readonly InputAction m_Human_Item3;
    private readonly InputAction m_Human_Focus;
    private readonly InputAction m_Human_GasBurstDoubleTap;
    public struct HumanActions
    {
        private @InputActions m_Wrapper;
        public HumanActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Human_Move;
        public InputAction @Look => m_Wrapper.m_Human_Look;
        public InputAction @Gas => m_Wrapper.m_Human_Gas;
        public InputAction @Jump => m_Wrapper.m_Human_Jump;
        public InputAction @Dodge => m_Wrapper.m_Human_Dodge;
        public InputAction @Salute => m_Wrapper.m_Human_Salute;
        public InputAction @Reload => m_Wrapper.m_Human_Reload;
        public InputAction @ReelMnK => m_Wrapper.m_Human_ReelMnK;
        public InputAction @ReelIn => m_Wrapper.m_Human_ReelIn;
        public InputAction @ReelOut => m_Wrapper.m_Human_ReelOut;
        public InputAction @Attack => m_Wrapper.m_Human_Attack;
        public InputAction @SpecialAttack => m_Wrapper.m_Human_SpecialAttack;
        public InputAction @HookLeft => m_Wrapper.m_Human_HookLeft;
        public InputAction @HookRight => m_Wrapper.m_Human_HookRight;
        public InputAction @HookBoth => m_Wrapper.m_Human_HookBoth;
        public InputAction @Item1 => m_Wrapper.m_Human_Item1;
        public InputAction @Item2 => m_Wrapper.m_Human_Item2;
        public InputAction @Item3 => m_Wrapper.m_Human_Item3;
        public InputAction @Focus => m_Wrapper.m_Human_Focus;
        public InputAction @GasBurstDoubleTap => m_Wrapper.m_Human_GasBurstDoubleTap;
        public InputActionMap Get() { return m_Wrapper.m_Human; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HumanActions set) { return set.Get(); }
        public void SetCallbacks(IHumanActions instance)
        {
            if (m_Wrapper.m_HumanActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnLook;
                @Gas.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnGas;
                @Gas.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnGas;
                @Gas.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnGas;
                @Jump.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnJump;
                @Dodge.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnDodge;
                @Salute.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnSalute;
                @Salute.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnSalute;
                @Salute.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnSalute;
                @Reload.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnReload;
                @ReelMnK.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelMnK;
                @ReelMnK.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelMnK;
                @ReelMnK.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelMnK;
                @ReelIn.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelIn;
                @ReelIn.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelIn;
                @ReelIn.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelIn;
                @ReelOut.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelOut;
                @ReelOut.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelOut;
                @ReelOut.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnReelOut;
                @Attack.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnAttack;
                @SpecialAttack.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnSpecialAttack;
                @SpecialAttack.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnSpecialAttack;
                @SpecialAttack.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnSpecialAttack;
                @HookLeft.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookLeft;
                @HookLeft.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookLeft;
                @HookLeft.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookLeft;
                @HookRight.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookRight;
                @HookRight.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookRight;
                @HookRight.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookRight;
                @HookBoth.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookBoth;
                @HookBoth.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookBoth;
                @HookBoth.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnHookBoth;
                @Item1.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem1;
                @Item1.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem1;
                @Item1.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem1;
                @Item2.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem2;
                @Item2.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem2;
                @Item2.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem2;
                @Item3.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem3;
                @Item3.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem3;
                @Item3.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnItem3;
                @Focus.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnFocus;
                @Focus.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnFocus;
                @Focus.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnFocus;
                @GasBurstDoubleTap.started -= m_Wrapper.m_HumanActionsCallbackInterface.OnGasBurstDoubleTap;
                @GasBurstDoubleTap.performed -= m_Wrapper.m_HumanActionsCallbackInterface.OnGasBurstDoubleTap;
                @GasBurstDoubleTap.canceled -= m_Wrapper.m_HumanActionsCallbackInterface.OnGasBurstDoubleTap;
            }
            m_Wrapper.m_HumanActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Gas.started += instance.OnGas;
                @Gas.performed += instance.OnGas;
                @Gas.canceled += instance.OnGas;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @Salute.started += instance.OnSalute;
                @Salute.performed += instance.OnSalute;
                @Salute.canceled += instance.OnSalute;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @ReelMnK.started += instance.OnReelMnK;
                @ReelMnK.performed += instance.OnReelMnK;
                @ReelMnK.canceled += instance.OnReelMnK;
                @ReelIn.started += instance.OnReelIn;
                @ReelIn.performed += instance.OnReelIn;
                @ReelIn.canceled += instance.OnReelIn;
                @ReelOut.started += instance.OnReelOut;
                @ReelOut.performed += instance.OnReelOut;
                @ReelOut.canceled += instance.OnReelOut;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @SpecialAttack.started += instance.OnSpecialAttack;
                @SpecialAttack.performed += instance.OnSpecialAttack;
                @SpecialAttack.canceled += instance.OnSpecialAttack;
                @HookLeft.started += instance.OnHookLeft;
                @HookLeft.performed += instance.OnHookLeft;
                @HookLeft.canceled += instance.OnHookLeft;
                @HookRight.started += instance.OnHookRight;
                @HookRight.performed += instance.OnHookRight;
                @HookRight.canceled += instance.OnHookRight;
                @HookBoth.started += instance.OnHookBoth;
                @HookBoth.performed += instance.OnHookBoth;
                @HookBoth.canceled += instance.OnHookBoth;
                @Item1.started += instance.OnItem1;
                @Item1.performed += instance.OnItem1;
                @Item1.canceled += instance.OnItem1;
                @Item2.started += instance.OnItem2;
                @Item2.performed += instance.OnItem2;
                @Item2.canceled += instance.OnItem2;
                @Item3.started += instance.OnItem3;
                @Item3.performed += instance.OnItem3;
                @Item3.canceled += instance.OnItem3;
                @Focus.started += instance.OnFocus;
                @Focus.performed += instance.OnFocus;
                @Focus.canceled += instance.OnFocus;
                @GasBurstDoubleTap.started += instance.OnGasBurstDoubleTap;
                @GasBurstDoubleTap.performed += instance.OnGasBurstDoubleTap;
                @GasBurstDoubleTap.canceled += instance.OnGasBurstDoubleTap;
            }
        }
    }
    public HumanActions @Human => new HumanActions(this);

    // Titan
    private readonly InputActionMap m_Titan;
    private ITitanActions m_TitanActionsCallbackInterface;
    public struct TitanActions
    {
        private @InputActions m_Wrapper;
        public TitanActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_Titan; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TitanActions set) { return set.Get(); }
        public void SetCallbacks(ITitanActions instance)
        {
            if (m_Wrapper.m_TitanActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_TitanActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public TitanActions @Titan => new TitanActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Chat;
    private readonly InputAction m_UI_ToggleCursor;
    private readonly InputAction m_UI_LiveCamera;
    private readonly InputAction m_UI_Minimap;
    private readonly InputAction m_UI_Fullscreen;
    private readonly InputAction m_UI_CameraMode;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Restart;
    private readonly InputAction m_UI_InteractionWheel;
    private readonly InputAction m_UI_Screenshot;
    private readonly InputAction m_UI_TrackedDeviceOrientation;
    private readonly InputAction m_UI_TrackedDevicePosition;
    private readonly InputAction m_UI_RightClick;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Navigate;
    public struct UIActions
    {
        private @InputActions m_Wrapper;
        public UIActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Chat => m_Wrapper.m_UI_Chat;
        public InputAction @ToggleCursor => m_Wrapper.m_UI_ToggleCursor;
        public InputAction @LiveCamera => m_Wrapper.m_UI_LiveCamera;
        public InputAction @Minimap => m_Wrapper.m_UI_Minimap;
        public InputAction @Fullscreen => m_Wrapper.m_UI_Fullscreen;
        public InputAction @CameraMode => m_Wrapper.m_UI_CameraMode;
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Restart => m_Wrapper.m_UI_Restart;
        public InputAction @InteractionWheel => m_Wrapper.m_UI_InteractionWheel;
        public InputAction @Screenshot => m_Wrapper.m_UI_Screenshot;
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Chat.started -= m_Wrapper.m_UIActionsCallbackInterface.OnChat;
                @Chat.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnChat;
                @Chat.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnChat;
                @ToggleCursor.started -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleCursor;
                @ToggleCursor.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleCursor;
                @ToggleCursor.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleCursor;
                @LiveCamera.started -= m_Wrapper.m_UIActionsCallbackInterface.OnLiveCamera;
                @LiveCamera.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnLiveCamera;
                @LiveCamera.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnLiveCamera;
                @Minimap.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMinimap;
                @Minimap.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMinimap;
                @Minimap.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMinimap;
                @Fullscreen.started -= m_Wrapper.m_UIActionsCallbackInterface.OnFullscreen;
                @Fullscreen.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnFullscreen;
                @Fullscreen.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnFullscreen;
                @CameraMode.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCameraMode;
                @CameraMode.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCameraMode;
                @CameraMode.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCameraMode;
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Restart.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRestart;
                @Restart.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRestart;
                @Restart.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRestart;
                @InteractionWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnInteractionWheel;
                @InteractionWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnInteractionWheel;
                @InteractionWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnInteractionWheel;
                @Screenshot.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScreenshot;
                @Screenshot.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScreenshot;
                @Screenshot.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScreenshot;
                @TrackedDeviceOrientation.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDevicePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Chat.started += instance.OnChat;
                @Chat.performed += instance.OnChat;
                @Chat.canceled += instance.OnChat;
                @ToggleCursor.started += instance.OnToggleCursor;
                @ToggleCursor.performed += instance.OnToggleCursor;
                @ToggleCursor.canceled += instance.OnToggleCursor;
                @LiveCamera.started += instance.OnLiveCamera;
                @LiveCamera.performed += instance.OnLiveCamera;
                @LiveCamera.canceled += instance.OnLiveCamera;
                @Minimap.started += instance.OnMinimap;
                @Minimap.performed += instance.OnMinimap;
                @Minimap.canceled += instance.OnMinimap;
                @Fullscreen.started += instance.OnFullscreen;
                @Fullscreen.performed += instance.OnFullscreen;
                @Fullscreen.canceled += instance.OnFullscreen;
                @CameraMode.started += instance.OnCameraMode;
                @CameraMode.performed += instance.OnCameraMode;
                @CameraMode.canceled += instance.OnCameraMode;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Restart.started += instance.OnRestart;
                @Restart.performed += instance.OnRestart;
                @Restart.canceled += instance.OnRestart;
                @InteractionWheel.started += instance.OnInteractionWheel;
                @InteractionWheel.performed += instance.OnInteractionWheel;
                @InteractionWheel.canceled += instance.OnInteractionWheel;
                @Screenshot.started += instance.OnScreenshot;
                @Screenshot.performed += instance.OnScreenshot;
                @Screenshot.canceled += instance.OnScreenshot;
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface ICannonActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IHorseActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IHumanActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnGas(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnSalute(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnReelMnK(InputAction.CallbackContext context);
        void OnReelIn(InputAction.CallbackContext context);
        void OnReelOut(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnSpecialAttack(InputAction.CallbackContext context);
        void OnHookLeft(InputAction.CallbackContext context);
        void OnHookRight(InputAction.CallbackContext context);
        void OnHookBoth(InputAction.CallbackContext context);
        void OnItem1(InputAction.CallbackContext context);
        void OnItem2(InputAction.CallbackContext context);
        void OnItem3(InputAction.CallbackContext context);
        void OnFocus(InputAction.CallbackContext context);
        void OnGasBurstDoubleTap(InputAction.CallbackContext context);
    }
    public interface ITitanActions
    {
    }
    public interface IUIActions
    {
        void OnChat(InputAction.CallbackContext context);
        void OnToggleCursor(InputAction.CallbackContext context);
        void OnLiveCamera(InputAction.CallbackContext context);
        void OnMinimap(InputAction.CallbackContext context);
        void OnFullscreen(InputAction.CallbackContext context);
        void OnCameraMode(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
        void OnInteractionWheel(InputAction.CallbackContext context);
        void OnScreenshot(InputAction.CallbackContext context);
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
    }
}
