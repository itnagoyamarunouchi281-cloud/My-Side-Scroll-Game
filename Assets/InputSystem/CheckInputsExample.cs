using UnityEngine;
using UnityEngine.InputSystem;

public class CheckInputsExample : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _jumpActionName = "Jump";
    [SerializeField] private string _lookActionName = "Look";

    private void Start()
    {
        // UnityEvent経由で受け取るように設定
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;

        // PlayerInputのイベント登録
        _playerInput.actions[_moveActionName].performed += OnMove;
        _playerInput.actions[_jumpActionName].performed += OnJump;
        _playerInput.actions[_lookActionName].performed += OnLook;
    }

    private void OnDestroy()
    {
        // PlayerInputのイベント解除
        _playerInput.actions[_moveActionName].performed -= OnMove;
        _playerInput.actions[_jumpActionName].performed -= OnJump;
        _playerInput.actions[_lookActionName].performed -= OnLook;
    }

    // 移動入力
    private void OnMove(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            print($"[Move/{context.control.device.name}]: {context.ReadValue<Vector2>()}");
        }
    }

    // ジャンプ入力
    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print($"Jump/{context.control.device.name}]");
        }
    }

    // 視点移動入力
    private void OnLook(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            print($"[Look/{context.control.device.name}]: {context.ReadValue<Vector2>()}");
        }
    }
}