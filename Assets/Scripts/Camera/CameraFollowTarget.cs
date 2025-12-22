using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _player;

    [Header("Rotation")]
    [SerializeField] private float _mouseSensitivity = 2.5f;
    [SerializeField] private float _minPitch = -30f;
    [SerializeField] private float _maxPitch = 60f;

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
        // Mouse input ONLY
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * _mouseSensitivity * 100f * Time.deltaTime;
        _pitch -= mouseY * _mouseSensitivity * 100f * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    private void LateUpdate()
    {
        // Follow player position WITHOUT parenting
        transform.position = _player.position;
    }
}
