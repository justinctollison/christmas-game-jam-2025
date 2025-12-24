using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 90f;

    [Header("Floating")]
    [SerializeField] private float _floatAmplitude = 0.25f;
    [SerializeField] private float _floatFrequency = 2f;

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }


    private void Start()
    {
        GameProgressManager.Instance.RegisterCoin();
    }

    private void Update()
    {
        HandleRotation();
        HandleFloating();
    }

    private void HandleRotation()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
    }

    private void HandleFloating()
    {
        float offset = Mathf.Sin(Time.time * _floatFrequency) * _floatAmplitude;
        transform.position = _startPosition + Vector3.up * offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameProgressManager.Instance.AddCoin();
        // AudioManager.Instance.PlaySFX(SFXType.Coin);

        Destroy(gameObject);
    }
}
