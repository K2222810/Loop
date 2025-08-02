using UnityEngine;

public class TimeTrialTrigger : MonoBehaviour
{
    public enum TriggerType { Start, Finish }
    public TriggerType type;
    public GhostManager ghostManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.CompareTag("Player"))
            {
                if (type == TriggerType.Start)
                    ghostManager.OnStartTrigger();
                else if (type == TriggerType.Finish)
                    ghostManager.OnFinishTrigger();
                gameObject.SetActive(false); // Disable trigger after first hit
            }
        }
    }
}
