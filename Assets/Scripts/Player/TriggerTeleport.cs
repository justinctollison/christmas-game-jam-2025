using UnityEngine;

public class TriggerTeleport : MonoBehaviour
{
    [SerializeField] private Transform _platformTeleport;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            other.transform.position = _platformTeleport.position;
            controller.enabled = true;
        }

        PlayerController player = other.GetComponent<PlayerController>();
        player?.ResetVerticalVelocity();

        Debug.Log("We triggered a death zone.");
    }
}
