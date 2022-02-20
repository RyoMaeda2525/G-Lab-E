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

    #endregion

    #region InputAction
    /// <summary> 移動操作の入力状況 </summary>
    static InputAction _MoveAction = default;

    #endregion

    #region プロパティ
    /// <summary> 移動操作の二次元値 </summary>
    static public Vector2 GetAxis2DMove { get => _MoveAction.ReadValue<Vector2>(); }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //入力を関連付け
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _MoveAction = actionMap[_StickNameMove];
    }

    
}
