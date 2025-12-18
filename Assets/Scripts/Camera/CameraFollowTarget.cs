using UnityEngine;

/// <summary>
/// Controls a camera follow target used by Cinemachine.
/// Handles smooth rotation based on player input.
/// </summary>
public class CameraFollowTarget : MonoBehaviour
{
    #region Settings
    [Header("Rotation Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _verticalClampMin = -30f;
    [SerializeField] private float _verticalClampMax = 60f;
    #endregion

    #region State
    private float _yaw;
    private float _pitch;
    #endregion

    private void Start()
    {
        Vector3 rotation = transform.eulerAngles;
        _yaw = rotation.y;
        _pitch = rotation.x;

        // Optional: lock cursor for game feel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleRotation();
    }

    #region Core Logic

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _yaw += mouseX;
        _pitch -= mouseY;

        _pitch = Mathf.Clamp(_pitch, _verticalClampMin, _verticalClampMax);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    #endregion
}
