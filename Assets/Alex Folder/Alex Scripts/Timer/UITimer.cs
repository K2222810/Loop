using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float startTime = 60f; // seconds
    [SerializeField] private bool countDown = true;

    private float currentTime;
    private bool isRunning = true;

    private void Start()
    {
        currentTime = startTime;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (!isRunning) return;

        // Update timer
        currentTime += countDown ? -Time.deltaTime : Time.deltaTime;

        // Clamp timer if counting down
        if (countDown && currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTimerEnd();
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer finished!");
        // You can call other methods here (e.g., end game, show UI, etc.)
    }

    // Optional public methods
    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public void ResetTimer() => currentTime = startTime;
}
