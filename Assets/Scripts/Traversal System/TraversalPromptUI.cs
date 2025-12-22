using UnityEngine;
using UnityEngine.UI;

public class TraversalPromptUI : MonoBehaviour
{
    [SerializeField] private TraversalDetector _detector;
    [SerializeField] private GameObject _promptRoot;

    private void Update()
    {
        ShowPromptUI();
    }

    private void ShowPromptUI()
    {
        _promptRoot.SetActive(_detector.CurrentTarget != null);
    }
}
