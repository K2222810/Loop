using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DynamicUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Canvas uiCanvas;
    public Slider staminaBar;
    public Slider recordingBar;
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float maxGameTime = 120f; // 2 minutes in seconds

    [Header("Recording Settings")]
    public float maxRecordingTime = 10f; // 10 seconds max recording

    [Header("Bar Colors")]
    public Color staminaColor = Color.yellow;
    public Color recordingColor = Color.cyan;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button tryAgainButton;
    public TextMeshProUGUI gameOverText;

    // Private variables
    private float currentGameTime;
    private float currentRecordingTime;
    private bool isRecording = false;
    private GhostRecorder ghostRecorder;

    void Start()
    {
        SetupUI();
        currentGameTime = maxGameTime;

        // Find the ghost recorder in the scene
        ghostRecorder = FindAnyObjectByType<GhostRecorder>();
        if (ghostRecorder == null)
        {
            Debug.LogWarning("No GhostRecorder found in scene!");
        }
    }

    void SetupUI()
    {
        // Create UI if not assigned
        if (uiCanvas == null)
        {
            CreateUIFromScratch();
        }
        else
        {
            SetupExistingUI();
        }
    }

    void CreateUIFromScratch()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("DynamicUI Canvas");
        uiCanvas = canvasObj.AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uiCanvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Create main panel
        GameObject panelObj = new GameObject("TopPanel");
        panelObj.transform.SetParent(uiCanvas.transform, false);
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.offsetMin = new Vector2(20, -100);
        panelRect.offsetMax = new Vector2(-20, -20);

        // Create Stamina Bar
        CreateBar(panelObj, "StaminaBar", new Vector2(0, 0.6f), new Vector2(0.48f, 1), staminaColor, out staminaBar);

        // Create Recording Bar
        CreateBar(panelObj, "RecordingBar", new Vector2(0.52f, 0.6f), new Vector2(1, 1), recordingColor, out recordingBar);

        // Create Timer Text
        CreateTimerText(panelObj);

        // Create GameOver UI
        CreateGameOverUI();
    }

    void SetupExistingUI()
    {
        // Setup colors for existing UI elements
        if (staminaBar != null)
        {
            staminaBar.fillRect.GetComponent<Image>().color = staminaColor;
        }

        if (recordingBar != null)
        {
            recordingBar.fillRect.GetComponent<Image>().color = recordingColor;
            recordingBar.value = 1f; // Start full
        }
    }

    void CreateBar(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax, Color barColor, out Slider slider)
    {
        // Create slider container
        GameObject sliderObj = new GameObject(name);
        sliderObj.transform.SetParent(parent.transform, false);
        RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
        sliderRect.anchorMin = anchorMin;
        sliderRect.anchorMax = anchorMax;
        sliderRect.offsetMin = Vector2.zero;
        sliderRect.offsetMax = Vector2.zero;

        slider = sliderObj.AddComponent<Slider>();

        // Create background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(sliderObj.transform, false);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        slider.targetGraphic = bgImage;

        // Create fill area
        GameObject fillAreaObj = new GameObject("Fill Area");
        fillAreaObj.transform.SetParent(sliderObj.transform, false);
        RectTransform fillAreaRect = fillAreaObj.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = Vector2.zero;
        fillAreaRect.offsetMax = Vector2.zero;

        // Create fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(fillAreaObj.transform, false);
        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = barColor;

        slider.fillRect = fillRect;
        slider.value = 1f;

        // Add label
        CreateBarLabel(sliderObj, name.Replace("Bar", ""), barColor);
    }

    void CreateBarLabel(GameObject barParent, string labelText, Color textColor)
    {
        GameObject labelObj = new GameObject($"{labelText}Label");
        labelObj.transform.SetParent(barParent.transform, false);
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 0);
        labelRect.anchorMax = new Vector2(1, 0.4f);
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        TextMeshProUGUI label = labelObj.AddComponent<TextMeshProUGUI>();
        label.text = labelText;
        label.color = textColor;
        label.fontSize = 14;
        label.fontStyle = FontStyles.Bold;
        label.alignment = TextAlignmentOptions.Center;
    }

    void CreateTimerText(GameObject parent)
    {
        GameObject timerObj = new GameObject("Timer");
        timerObj.transform.SetParent(parent.transform, false);
        RectTransform timerRect = timerObj.AddComponent<RectTransform>();
        timerRect.anchorMin = new Vector2(0, 0);
        timerRect.anchorMax = new Vector2(1, 0.5f);
        timerRect.offsetMin = Vector2.zero;
        timerRect.offsetMax = Vector2.zero;

        timerText = timerObj.AddComponent<TextMeshProUGUI>();
        timerText.text = "2:00";
        timerText.color = Color.white;
        timerText.fontSize = 24;
        timerText.fontStyle = FontStyles.Bold;
        timerText.alignment = TextAlignmentOptions.Center;
    }

    void CreateGameOverUI()
    {
        gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(uiCanvas.transform, false);
        RectTransform panelRect = gameOverPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.3f, 0.3f);
        panelRect.anchorMax = new Vector2(0.7f, 0.7f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelImage = gameOverPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.85f);

        // Game Over Text
        GameObject textObj = new GameObject("GameOverText");
        textObj.transform.SetParent(gameOverPanel.transform, false);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0.5f);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        gameOverText = textObj.AddComponent<TextMeshProUGUI>();
        gameOverText.text = "GAME OVER\nOVERHEATED REACTOR";
        gameOverText.fontSize = 28;
        gameOverText.color = Color.red;
        gameOverText.alignment = TextAlignmentOptions.Center;
        gameOverText.fontStyle = FontStyles.Bold;

        // Try Again Button
        GameObject buttonObj = new GameObject("TryAgainButton");
        buttonObj.transform.SetParent(gameOverPanel.transform, false);
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.3f, 0);
        buttonRect.anchorMax = new Vector2(0.7f, 0.4f);
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;

        tryAgainButton = buttonObj.AddComponent<Button>();

        Image btnImage = buttonObj.AddComponent<Image>();
        btnImage.color = new Color(0.2f, 0.2f, 0.2f, 1);

        GameObject btnTextObj = new GameObject("ButtonText");
        btnTextObj.transform.SetParent(buttonObj.transform, false);
        RectTransform btnTextRect = btnTextObj.AddComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.offsetMin = Vector2.zero;
        btnTextRect.offsetMax = Vector2.zero;

        TextMeshProUGUI btnText = btnTextObj.AddComponent<TextMeshProUGUI>();
        btnText.text = "TRY AGAIN";
        btnText.fontSize = 20;
        btnText.color = Color.white;
        btnText.alignment = TextAlignmentOptions.Center;

        tryAgainButton.onClick.AddListener(RestartScene);

        gameOverPanel.SetActive(false); // hide initially
    }

    void Update()
    {
        UpdateGameTimer();
        UpdateRecordingState();
        UpdateBars();
    }

    void UpdateGameTimer()
    {
        if (currentGameTime > 0)
        {
            currentGameTime -= Time.deltaTime;
            currentGameTime = Mathf.Max(0, currentGameTime); // Prevent negative values
        }

        // Update timer display
        int minutes = Mathf.FloorToInt(currentGameTime / 60);
        int seconds = Mathf.FloorToInt(currentGameTime % 60);
        timerText.text = $"{minutes}:{seconds:00}";

        // Change timer color when time is running low
        if (currentGameTime <= 30f)
        {
            timerText.color = Color.red;
        }
        else if (currentGameTime <= 60f)
        {
            timerText.color = Color.yellow;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    void UpdateRecordingState()
    {
        // Check if recording state changed
        if (ghostRecorder != null)
        {
            bool wasRecording = isRecording;
            isRecording = ghostRecorder.isRecording;

            // Reset recording time when starting recording
            if (!wasRecording && isRecording)
            {
                currentRecordingTime = maxRecordingTime;
            }
        }

        // Update recording timer
        if (isRecording && currentRecordingTime > 0)
        {
            currentRecordingTime -= Time.deltaTime;
            currentRecordingTime = Mathf.Max(0, currentRecordingTime);

            // Stop recording if time runs out
            if (currentRecordingTime <= 0 && ghostRecorder != null)
            {
                ghostRecorder.StopRecording();
                isRecording = false;
            }
        }
    }

    void UpdateBars()
    {
        // Update stamina bar (decreases as game time decreases)
        float staminaPercentage = currentGameTime / maxGameTime;
        staminaBar.value = staminaPercentage;

        // Update recording bar
        if (isRecording && maxRecordingTime > 0)
        {
            float recordingPercentage = currentRecordingTime / maxRecordingTime;
            recordingBar.value = recordingPercentage;

            // Change recording bar color when time is running low
            Image recordingFill = recordingBar.fillRect.GetComponent<Image>();
            if (currentRecordingTime <= 3f)
            {
                recordingFill.color = Color.red;
            }
            else if (currentRecordingTime <= 5f)
            {
                recordingFill.color = Color.yellow;
            }
            else
            {
                recordingFill.color = recordingColor;
            }
        }
        else
        {
            recordingBar.value = 1f; // Full when not recording
            recordingBar.fillRect.GetComponent<Image>().color = recordingColor;
        }

        if (staminaPercentage <= 0f && !gameOverPanel.activeSelf)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
    public void RestartScene()
    {
        Time.timeScale = 1f; // Resume time before reload
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Debug.Log("Reloading Scene");
    }

    // Public methods for external control
    public void ResetGameTimer()
    {
        currentGameTime = maxGameTime;
    }

    public void ResetRecordingTimer()
    {
        currentRecordingTime = maxRecordingTime;
    }

    public float GetRemainingGameTime()
    {
        return currentGameTime;
    }

    public float GetRemainingRecordingTime()
    {
        return currentRecordingTime;
    }

    public bool IsGameTimeUp()
    {
        return currentGameTime <= 0;
    }
}