using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputUtility : MonoBehaviour
{
    #region InputActionのActionsキー
    [Header("以下、InputActionのActionsに登録した名前を登録")]
    [SerializeField, Tooltip("InputActionにおける、移動入力名")]
    private string _StickNameMove = "Move";

    [SerializeField, Tooltip("InputActionにおける、カメラ移動入力名")]
    private string _StickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputActionにおける、ジャンプ入力名")]
    private string _ButtonNameJump = "Jump";

    #endregion

    #region InputAction
    /// <summary> 移動操作の入力状況 </summary>
    static InputAction _MoveAction = default;

    /// <summary> カメラ移動操作の入力状況 </summary>
    static InputAction _CameraMoveAction = default;

    /// <summary> ジャンプの入力状況 </summary>
    static InputAction _JumpAction = default;

    #endregion

    #region プロパティ
    /// <summary> 移動操作の二次元値 </summary>
    static public Vector2 GetAxis2DMove { get => _MoveAction.ReadValue<Vector2>(); }
    /// <summary> カメラ移動操作の二次元値 </summary>
    static public Vector2 GetAxis2DCameraMove { get => _CameraMoveAction.ReadValue<Vector2>(); }
    /// <summary> ジャンプボタン押下直後 </summary>
    static public bool GetDownJump { get => _JumpAction.triggered; }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //入力を関連付け
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _MoveAction = actionMap[_StickNameMove];
        _CameraMoveAction = actionMap[_StickNameCameraMove];
        _JumpAction = actionMap[_ButtonNameJump];
    }

    
}
