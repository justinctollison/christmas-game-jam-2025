using UnityEngine;

public class EndDoor : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        GameProgressManager.Instance.OnAllLightsOn += OpenDoor;
    }

    private void OpenDoor()
    {
        if (_animator != null)
            _animator.SetTrigger("Open");

        // AudioManager.Instance.PlaySFX(SFXType.Door);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (GameProgressManager.Instance != null)
            GameProgressManager.Instance.OnAllLightsOn -= OpenDoor;
    }
}
