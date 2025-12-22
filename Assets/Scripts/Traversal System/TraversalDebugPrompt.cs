using TMPro;
using UnityEngine;

public class TraversalDebugPrompt : MonoBehaviour
{
    [SerializeField] private TraversalDetector _detector;

    private void Update()
    {
        if (_detector.CurrentTarget != null)
        {
            this.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        else
        {
            this.GetComponent<TextMeshProUGUI>().enabled = false;

        }
    }
}
