using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class EnhancedPlayerUi : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Canvas uiCanvas;

    [Header("Key Squares")]
    [SerializeField] private GameObject keySquaresPanel;
    [SerializeField] private Button eKeySquare;
    [SerializeField] private Button cKeySquare;
    [SerializeField] private Button fKeySquare;
    [SerializeField] private Button rKeySquare;

    [Header("Reset Hold UI")]
    [SerializeField] private GameObject resetHoldPanel;
    [SerializeField] private Image resetCircleFill;
    [SerializeField] private TextMeshProUGUI resetHoldText;

    [Header("Fade Screen")]
    [SerializeField] private Image fadeScreen;

    [Header("Animation Settings")]
    public float keySquareAnimScale = 1.2f;
    public float keySquareAnimDuration = 0.1f;
    public float textAnimDuration = 0.5f;
    public float resetHoldTime = 2f;

    // Private variables
    private bool isHoldingReset = false;
    private float currentResetHoldTime = 0f;
    private Coroutine resetHoldCoroutine;
    private PlayerInteraction playerInteraction;
    private DynamicUIManager dynamicUIManager;
    private GhostRecorder ghostRecorder;
    private GhostManager ghostManager;

    void Start()
    {
        try
        {
            SetupUI();
            GetReferences();
            SetupKeySquareAnimations();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in Start: {e.Message}");
        }
    }

    void GetReferences()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        dynamicUIManager = FindAnyObjectByType<DynamicUIManager>();
        ghostRecorder = GetComponent<GhostRecorder>();
        ghostManager = FindAnyObjectByType<GhostManager>();
    }

    void SetupUI()
    {
        if (uiCanvas == null)
        {
            Debug.LogError("Canvas reference is missing! Please assign a Canvas in the inspector.");
            return;
        }
        CreatePromptText();
        CreateKeySquares();
        CreateResetHoldUI();
        CreateFadeScreen();
    }

    void CreateUIFromScratch()
    {
        // Create Canvas if needed
        GameObject canvasObj = new GameObject("Enhanced Player UI Canvas");
        uiCanvas = canvasObj.AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uiCanvas.sortingOrder = 50;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        CreatePromptText();
        CreateKeySquares();
        CreateResetHoldUI();
        CreateFadeScreen();
    }

    void CreatePromptText()
    {
        GameObject promptObj = new GameObject("PromptText");
        promptObj.transform.SetParent(uiCanvas.transform, false);
        RectTransform promptRect = promptObj.AddComponent<RectTransform>();
        promptRect.anchorMin = new Vector2(0.5f, 0.7f);
        promptRect.anchorMax = new Vector2(0.5f, 0.7f);
        promptRect.sizeDelta = new Vector2(400, 60);

        promptText = promptObj.AddComponent<TextMeshProUGUI>();
        promptText.text = "";
        promptText.fontSize = 24;
        promptText.fontStyle = FontStyles.Bold;
        promptText.alignment = TextAlignmentOptions.Center;
        promptText.color = Color.white;

        // Add outline
        promptText.outlineWidth = 0.2f;
        promptText.outlineColor = Color.black;
    }

    void CreateKeySquares()
    {
        // Create panel for key squares
        GameObject panelObj = new GameObject("KeySquaresPanel");
        panelObj.transform.SetParent(uiCanvas.transform, false);
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0);
        panelRect.anchorMax = new Vector2(0.5f, 0);
        panelRect.anchoredPosition = new Vector2(0, 80);
        panelRect.sizeDelta = new Vector2(400, 80);

        keySquaresPanel = panelObj;

        // Create horizontal layout
        HorizontalLayoutGroup layout = panelObj.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 10;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;

        // Create key squares
        eKeySquare = CreateKeySquare(panelObj, "E", "Interact", Color.green);
        cKeySquare = CreateKeySquare(panelObj, "C", "Reset", Color.yellow);
        fKeySquare = CreateKeySquare(panelObj, "F", "Record", Color.red);
        rKeySquare = CreateKeySquare(panelObj, "R", "Replay", Color.blue);
    }

    Button CreateKeySquare(GameObject parent, string keyText, string actionText, Color keyColor)
    {
        GameObject squareObj = new GameObject($"{keyText}KeySquare");
        squareObj.transform.SetParent(parent.transform, false);

        Button button = squareObj.AddComponent<Button>();
        Image buttonImage = squareObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

        // Key text
        GameObject keyTextObj = new GameObject("KeyText");
        keyTextObj.transform.SetParent(squareObj.transform, false);
        RectTransform keyTextRect = keyTextObj.AddComponent<RectTransform>();
        keyTextRect.anchorMin = Vector2.zero;
        keyTextRect.anchorMax = Vector2.one;
        keyTextRect.offsetMin = Vector2.zero;
        keyTextRect.offsetMax = new Vector2(0, -20);

        TextMeshProUGUI keyTextComp = keyTextObj.AddComponent<TextMeshProUGUI>();
        keyTextComp.text = keyText;
        keyTextComp.fontSize = 24;
        keyTextComp.fontStyle = FontStyles.Bold;
        keyTextComp.alignment = TextAlignmentOptions.Center;
        keyTextComp.color = keyColor;

        // Action text
        GameObject actionTextObj = new GameObject("ActionText");
        actionTextObj.transform.SetParent(squareObj.transform, false);
        RectTransform actionTextRect = actionTextObj.AddComponent<RectTransform>();
        actionTextRect.anchorMin = Vector2.zero;
        actionTextRect.anchorMax = Vector2.one;
        actionTextRect.offsetMin = new Vector2(0, 0);
        actionTextRect.offsetMax = new Vector2(0, 20);

        TextMeshProUGUI actionTextComp = actionTextObj.AddComponent<TextMeshProUGUI>();
        actionTextComp.text = actionText;
        actionTextComp.fontSize = 10;
        actionTextComp.alignment = TextAlignmentOptions.Center;
        actionTextComp.color = Color.white;

        return button;
    }

    void CreateResetHoldUI()
    {
        GameObject resetPanel = new GameObject("ResetHoldPanel");
        resetPanel.transform.SetParent(uiCanvas.transform, false);
        RectTransform resetRect = resetPanel.AddComponent<RectTransform>();
        resetRect.anchorMin = new Vector2(0.5f, 0.5f);
        resetRect.anchorMax = new Vector2(0.5f, 0.5f);
        resetRect.sizeDelta = new Vector2(150, 150);

        resetHoldPanel = resetPanel;
        resetHoldPanel.SetActive(false);

        // Create background dark overlay
        GameObject bgOverlay = new GameObject("BackgroundOverlay");
        bgOverlay.transform.SetParent(resetPanel.transform, false);
        RectTransform bgOverlayRect = bgOverlay.AddComponent<RectTransform>();
        bgOverlayRect.anchorMin = Vector2.zero;
        bgOverlayRect.anchorMax = Vector2.one;
        bgOverlayRect.offsetMin = Vector2.zero;
        bgOverlayRect.offsetMax = Vector2.zero;

        Image bgOverlayImage = bgOverlay.AddComponent<Image>();
        bgOverlayImage.color = new Color(0, 0, 0, 0.7f);

        // Create outer ring (background circle border)
        GameObject outerRing = new GameObject("OuterRing");
        outerRing.transform.SetParent(resetPanel.transform, false);
        RectTransform outerRect = outerRing.AddComponent<RectTransform>();
        outerRect.anchorMin = Vector2.zero;
        outerRect.anchorMax = Vector2.one;
        outerRect.offsetMin = new Vector2(10, 10);
        outerRect.offsetMax = new Vector2(-10, -10);

        Image outerRingImage = outerRing.AddComponent<Image>();
        outerRingImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        outerRingImage.type = Image.Type.Filled;
        outerRingImage.fillMethod = Image.FillMethod.Radial360;
        outerRingImage.fillAmount = 1f;

        // Create inner background
        GameObject innerBg = new GameObject("InnerBackground");
        innerBg.transform.SetParent(resetPanel.transform, false);
        RectTransform innerBgRect = innerBg.AddComponent<RectTransform>();
        innerBgRect.anchorMin = Vector2.zero;
        innerBgRect.anchorMax = Vector2.one;
        innerBgRect.offsetMin = new Vector2(25, 25);
        innerBgRect.offsetMax = new Vector2(-25, -25);

        Image innerBgImage = innerBg.AddComponent<Image>();
        innerBgImage.color = new Color(0, 0, 0, 0.7f);
        innerBgImage.type = Image.Type.Filled;
        innerBgImage.fillMethod = Image.FillMethod.Radial360;
        innerBgImage.fillAmount = 1f;

        // Create progress fill circle - MODIFIED THIS PART
        GameObject fillCircle = new GameObject("FillCircle");
        fillCircle.transform.SetParent(resetPanel.transform, false);
        RectTransform fillRect = fillCircle.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = new Vector2(10, 10);
        fillRect.offsetMax = new Vector2(-10, -10);

        // Create and assign the Image component
        Image fillImage = fillCircle.AddComponent<Image>();
        fillImage.color = Color.yellow;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Radial360;
        fillImage.fillOrigin = 2;
        fillImage.fillClockwise = true;
        fillImage.fillAmount = 0;

        // Assign the reference
        resetCircleFill = fillImage;

        // Verify the reference was set
        if (resetCircleFill == null)
        {
            Debug.LogError("Failed to assign resetCircleFill!");
        }
        else
        {
            Debug.Log("Successfully created and assigned resetCircleFill");
        }


        // Reset text - Modified part
        GameObject resetTextObj = new GameObject("ResetText");
        resetTextObj.transform.SetParent(resetPanel.transform, false);
        RectTransform textRect = resetTextObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        resetHoldText = resetTextObj.AddComponent<TextMeshProUGUI>();

        // Create a temporary material for the text
        Material tmpMaterial = new Material(Shader.Find("TextMeshPro/Distance Field"));

        // Load the default TMP font asset
        TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        if (fontAsset != null)
        {
            resetHoldText.font = fontAsset;
            resetHoldText.material = fontAsset.material;

        }
        else
        {
            Debug.LogError("Failed to load TMP font asset!");
        }

        // Set basic properties first
        resetHoldText.text = "Hold to Reset";
        resetHoldText.fontSize = 16;
        resetHoldText.alignment = TextAlignmentOptions.Center;
        resetHoldText.color = Color.white;
        resetHoldText.fontStyle = FontStyles.Bold;

        // Start coroutine to set outline after initialization
        StartCoroutine(InitializeTextProperties(resetHoldText));
    }

    private IEnumerator InitializeTextProperties(TextMeshProUGUI textComponent)
    {
        yield return new WaitForEndOfFrame();

        if (textComponent != null && textComponent.font != null)
        {

            textComponent.outlineWidth = 0.2f;
            textComponent.outlineColor = Color.black;
          
        }
    }

    void CreateFadeScreen()
    {
        GameObject fadeObj = new GameObject("FadeScreen");
        fadeObj.transform.SetParent(uiCanvas.transform, false);
        RectTransform fadeRect = fadeObj.AddComponent<RectTransform>();
        fadeRect.anchorMin = Vector2.zero;
        fadeRect.anchorMax = Vector2.one;
        fadeRect.offsetMin = Vector2.zero;
        fadeRect.offsetMax = Vector2.zero;

        fadeScreen = fadeObj.AddComponent<Image>();
        fadeScreen.color = new Color(0, 0, 0, 0);
        fadeScreen.raycastTarget = false;
    }

    void SetupKeySquareAnimations()
    {
        // Add click animations to key squares
        if (eKeySquare) SetupKeySquareAnimation(eKeySquare);
        if (cKeySquare) SetupKeySquareAnimation(cKeySquare);
        if (fKeySquare) SetupKeySquareAnimation(fKeySquare);
        if (rKeySquare) SetupKeySquareAnimation(rKeySquare);
    }

    void SetupKeySquareAnimation(Button keySquare)
    {
        keySquare.onClick.AddListener(() => AnimateKeySquare(keySquare.transform));
    }

    void Update()
    {
        HandleInput();
        UpdateKeySquareStates();
    }

    void HandleInput()
    {
        // E key - Interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            AnimateKeySquare(eKeySquare.transform);
        }

        // F key - Recording
        if (Input.GetKeyDown(KeyCode.F))
        {
            AnimateKeySquare(fKeySquare.transform);
        }

        // R key - Replay
        if (Input.GetKeyDown(KeyCode.R))
        {
            AnimateKeySquare(rKeySquare.transform);
            if (ghostRecorder != null && ghostRecorder.recordedFrames.Count > 0)
            {
                StartCoroutine(HandleReplayTransition());
            }
        }

        // C key - Reset (hold)
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartResetHold();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            StopResetHold();
        }

        if (Input.GetKey(KeyCode.C) && isHoldingReset)
        {
            UpdateResetHold();
        }
    }

    void StartResetHold()
    {
        if (playerInteraction.currentInteractable != null && playerInteraction.currentInteractable.hasBeenInteracted)
        {
            isHoldingReset = true;
            currentResetHoldTime = 0f;
            resetHoldPanel.SetActive(true);
            resetCircleFill.fillAmount = 0;
            AnimateKeySquare(cKeySquare.transform);
        }
    }

    void UpdateResetHold()
    {
        currentResetHoldTime += Time.deltaTime;
        float fillProgress = currentResetHoldTime / resetHoldTime;
        resetCircleFill.fillAmount = fillProgress;

        if (currentResetHoldTime >= resetHoldTime)
        {
            // Reset the object
            if (playerInteraction.currentInteractable != null)
            {
                playerInteraction.currentInteractable.ResetToOriginalPosition();
            }
            StopResetHold();
        }
    }

    void StopResetHold()
    {
        isHoldingReset = false;
        currentResetHoldTime = 0f;
        resetHoldPanel.SetActive(false);
        resetCircleFill.fillAmount = 0;
    }

    void UpdateKeySquareStates()
    {
        // Update E key state based on current interactable
        if (playerInteraction != null && playerInteraction.currentInteractable != null)
        {
            bool canInteract = !playerInteraction.currentInteractable.hasBeenInteracted;
            eKeySquare.interactable = canInteract;
            eKeySquare.GetComponent<Image>().color = canInteract ?
                new Color(0.2f, 0.2f, 0.2f, 0.8f) :
                new Color(0.1f, 0.1f, 0.1f, 0.5f);
        }
        else
        {
            eKeySquare.interactable = false;
            eKeySquare.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
        }

        // Update C key state
        bool canReset = playerInteraction != null &&
                       playerInteraction.currentInteractable != null &&
                       playerInteraction.currentInteractable.hasBeenInteracted;
        cKeySquare.interactable = canReset;
        cKeySquare.GetComponent<Image>().color = canReset ?
            new Color(0.2f, 0.2f, 0.2f, 0.8f) :
            new Color(0.1f, 0.1f, 0.1f, 0.5f);

        // Update F key state (recording)
        bool isRecording = ghostRecorder != null && ghostRecorder.isRecording;
        fKeySquare.GetComponent<Image>().color = isRecording ?
            new Color(0.8f, 0.2f, 0.2f, 0.8f) :
            new Color(0.2f, 0.2f, 0.2f, 0.8f);

        // Update R key state
        bool hasRecording = ghostRecorder != null && ghostRecorder.recordedFrames.Count > 0;
        rKeySquare.interactable = hasRecording && !isRecording;
        rKeySquare.GetComponent<Image>().color = hasRecording ?
            new Color(0.2f, 0.2f, 0.2f, 0.8f) :
            new Color(0.1f, 0.1f, 0.1f, 0.5f);
    }

    void AnimateKeySquare(Transform keySquare)
    {
        keySquare.DOScale(keySquareAnimScale, keySquareAnimDuration)
                 .SetEase(Ease.OutBack)
                 .OnComplete(() => {
                     keySquare.DOScale(1f, keySquareAnimDuration)
                              .SetEase(Ease.InBack);
                 });
    }

    IEnumerator HandleReplayTransition()
    {
        // Fade to black
        yield return fadeScreen.DOFade(1f, 0.5f).WaitForCompletion();

        // Reduce timer by 5 seconds
        if (dynamicUIManager != null)
        {
            // Add the timer reduction function from the the other scripts,
            // Make sure to add a reference to the class you are using the method from at the start of this script
        }

        // Wait for 1.5 seconds
        yield return new WaitForSeconds(1.0f);

        // Fade back to transparent
        yield return fadeScreen.DOFade(0f, 0.5f).WaitForCompletion();
    }

    public void UpdateText(string promptMessage)
    {
        if (promptText == null || !gameObject.activeInHierarchy) return;

        try
        {
            if (string.IsNullOrEmpty(promptMessage))
            {
                promptText.DOFade(0f, textAnimDuration);
                return;
            }

            if (playerInteraction == null) return;

            if (playerInteraction.currentInteractable != null &&
                playerInteraction.currentInteractable.hasBeenInteracted)
            {
                ShowCoolTextAnimation("Reset Object", Color.yellow);
            }
            else
            {
                var currentInteractable = playerInteraction.currentInteractable;
                if (currentInteractable == null)
                {
                    promptText.text = promptMessage;
                    promptText.DOFade(1f, textAnimDuration);
                    return;
                }

                if (currentInteractable is ButtonInteractable)
                {
                    ShowCoolTextAnimation("Press Button", Color.cyan);
                }
                else if (currentInteractable is GrabObject)
                {
                    ShowCoolTextAnimation("Carry Object", Color.green);
                }
                else
                {
                    promptText.text = promptMessage;
                    promptText.DOFade(1f, textAnimDuration);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in UpdateText: {e.Message}");
        }
    }

    private void ShowCoolTextAnimation(string message, Color textColor)
    {
        if (promptText == null) return;

        try
        {
            promptText.text = message;
            promptText.color = textColor;

            promptText.transform.localScale = Vector3.zero;
            promptText.alpha = 0f;

            promptText.transform.DOScale(1.2f, textAnimDuration * 0.5f)
                     .SetEase(Ease.OutBack)
                     .OnComplete(() => {
                         if (promptText != null)
                         {
                             promptText.transform.DOScale(1f, textAnimDuration * 0.5f)
                                      .SetEase(Ease.InBack);
                         }
                     });

            promptText.DOFade(1f, textAnimDuration);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in ShowCoolTextAnimation: {e.Message}");
        }
    }
}
