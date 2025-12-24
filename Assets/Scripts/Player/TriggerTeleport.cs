using UnityEngine;

public class TriggerTeleport : MonoBehaviour
{
    [SerializeField] private Transform _platformTeleport;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            other.gameObject.transform.position = _platformTeleport.transform.position;
        }
    }
}
