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
    string _StickNameMove = "Move";

    [SerializeField, Tooltip("InputActionにおける、カメラ移動入力名")]
    string _StickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputActionにおける、ジャンプ入力名")]
    string _ButtonNameJump = "Jump";

    [SerializeField, Tooltip("InputActionにおける、「上の変身先」への変身入力名")]
    string _ButtonNameMorphUp = "MorphUp";

    [SerializeField, Tooltip("InputActionにおける、「下の変身先」への変身入力名")]
    string _ButtonNameMorphDown = "MorphDown";

    [SerializeField, Tooltip("InputActionにおける、「右の変身先」への変身入力名")]
    string _ButtonNameMorphRight = "MorphRight";
    #endregion

    #region コントローラー振動用メンバ
    /// <summary> コントローラー </summary>
    static Gamepad _Gamepad = default;

    [SerializeField, Range(0, 1), Tooltip("コントローラーの右側の振動の強さ")]
    float _RightShakePower = 0.5f;

    [SerializeField, Range(0, 1), Tooltip("コントローラーの左側の振動の強さ")]
    float _LeftShakePower = 0.5f;

    [SerializeField, Tooltip("コントローラーの振動の間隔")]
    float _ShakeInterval = 0.2f;
    #endregion

    #region InputAction
    /// <summary> 移動操作の入力状況 </summary>
    static InputAction _MoveAction = default;

    /// <summary> カメラ移動操作の入力状況 </summary>
    static InputAction _CameraMoveAction = default;

    /// <summary> ジャンプの入力状況 </summary>
    static InputAction _JumpAction = default;

    /// <summary> 「上の変身先」への変身の入力状況 </summary>
    static InputAction _MorphUpAction = default;

    /// <summary> 「下の変身先」への変身の入力状況 </summary>
    static InputAction _MorphDownAction = default;

    /// <summary> 「右の変身先」への変身の入力状況 </summary>
    static InputAction _MorphRightAction = default;
    #endregion

    #region プロパティ
    /// <summary> 移動操作の二次元値 </summary>
    static public Vector2 GetAxis2DMove { get => _MoveAction.ReadValue<Vector2>(); }
    /// <summary> カメラ移動操作の二次元値 </summary>
    static public Vector2 GetAxis2DCameraMove { get => _CameraMoveAction.ReadValue<Vector2>(); }
    /// <summary> ジャンプボタン押下直後 </summary>
    static public bool GetDownJump { get => _JumpAction.triggered; }
    /// <summary> ジャンプボタン押下中 </summary>
    static public bool GetJump { get => _JumpAction.IsPressed(); }
    /// <summary> 「上の変身先」への変身ボタン押下直後 </summary>
    static public bool GetDownMorphUp { get => _MorphUpAction.triggered; }
    /// <summary> 「下の変身先」への変身ボタン押下直後 </summary>
    static public bool GetDownMorphDown { get => _MorphDownAction.triggered; }
    /// <summary> 「右の変身先」への変身ボタン押下直後 </summary>
    static public bool GetDownMorphRight { get => _MorphRightAction.triggered; }
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
        _MorphUpAction = actionMap[_ButtonNameMorphUp];
        _MorphDownAction = actionMap[_ButtonNameMorphDown];
        _MorphRightAction = actionMap[_ButtonNameMorphRight];

        //ゲームパッド情報を取得
        _Gamepad = Gamepad.current;

        StartCoroutine(PalusShake());
    }

    /// <summary> コントローラーの振動を促す。ただし、数値は0～1の範囲で。範囲を超える場合はClampする。 </summary>
    /// <param name="leftPower">左側のモーター強度</param>
    /// <param name="rightPower">右側のモーター強度</param>
    static public void SimpleShakeController(float leftPower, float rightPower)
    {
        if (_Gamepad == null) _Gamepad = Gamepad.current;
        if (_Gamepad != null)
        {
            _Gamepad.SetMotorSpeeds(Mathf.Clamp01(leftPower), Mathf.Clamp01(rightPower));
            
        }
    }

    /// <summary>コントローラーの振動を止める</summary>
    static public void StopShakeController()
    {
        if (_Gamepad == null) _Gamepad = Gamepad.current;
        if (_Gamepad != null)
        {
            _Gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    /// <summary>コントローラーの振動を一定間隔で</summary>
    IEnumerator PalusShake()
    {
        while (enabled)
        {
            SimpleShakeController(_LeftShakePower, _RightShakePower);

            yield return new WaitForSeconds(_ShakeInterval);

            StopShakeController();

            yield return new WaitForSeconds(_ShakeInterval);
        }
    }
}
