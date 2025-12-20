using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    #region Settings
    [SerializeField] private float _mouseSensitivity = 120f;
    [SerializeField] private float _minPitch = -30f;
    [SerializeField] private float _maxPitch = 65f;
    #endregion

    private float _yaw;
    private float _pitch;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * _mouseSensitivity * Time.deltaTime;
        _pitch -= mouseY * _mouseSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }
}
