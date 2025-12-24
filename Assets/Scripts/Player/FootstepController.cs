using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private float _rayDistance = 1.2f;
    [SerializeField] private LayerMask _groundMask;

    public void OnFootstep()
    {
        if (!GetComponent<PlayerController>().IsGrounded)
            return;

        AudioManager.Instance.PlaySFX(SFXType.Footstep);
    }
}
