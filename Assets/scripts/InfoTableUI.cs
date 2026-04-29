using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoTableUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject idlePanel;
    public GameObject solidPanel;

    [Header("Idle panel")]
    public TextMeshProUGUI idleText;

    [Header("Solid panel")]
    public Image panelBackground;
    public TextMeshProUGUI solidNameText;
    public TextMeshProUGUI vertexStatText;
    public TextMeshProUGUI edgeStatText;
    public TextMeshProUGUI faceStatText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI eulerText;

    [Header("Buttons")]
    public Button verticesButton;
    public Button edgesButton;
    public Button netButton; // will be disabled for now

    [Header("Color settings")]
    // How much to tint — 0 = white panel, 1 = full solid color
    [Range(0f, 1f)] public float colorTintStrength = 0.25f;

    private bool showingVertices = false;
    private bool showingEdges = false;

    private void Start()
    {
        ShowIdle();
        // Wire up buttons
        verticesButton.onClick.AddListener(ToggleVertices);
        edgesButton.onClick.AddListener(ToggleEdges);
        netButton.interactable = false; // coming soon

        // Apply localized strings to buttons and idle text
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        var s = LocalizationManager.Instance.Strings;
        idleText.text = s.dropPrompt;
        verticesButton.GetComponentInChildren<TextMeshProUGUI>().text = s.showVerticesBtn;
        edgesButton.GetComponentInChildren<TextMeshProUGUI>().text = s.showEdgesBtn;
        netButton.GetComponentInChildren<TextMeshProUGUI>().text = s.showNetBtn;
    }

    public void ShowIdle()
    {
        idlePanel.SetActive(true);
        solidPanel.SetActive(false);
        showingVertices = false;
        showingEdges = false;
    }

    public void ShowSolid(SolidData data, Color solidColor)
    {
        idlePanel.SetActive(false);
        solidPanel.SetActive(true);

        var s = LocalizationManager.Instance.Strings;

        solidNameText.text = data.solidName;
        descriptionText.text = data.description;
        eulerText.text = $"{s.eulerLabel}  {data.eulerFormula}";

        vertexStatText.text = $"{s.verticesLabel}\n{data.vertexCount}";
        edgeStatText.text   = $"{s.edgesLabel}\n{data.edgeCount}";
        faceStatText.text   = $"{s.facesLabel}\n{data.faceCount}";

        // Tint the background toward the solid's color
        panelBackground.color = Color.Lerp(Color.white, solidColor, colorTintStrength);

        // Reset button states
        showingVertices = false;
        showingEdges = false;
        UpdateButtonVisuals();
    }

    private void ToggleVertices()
    {
        showingVertices = !showingVertices;
        UpdateButtonVisuals();
        // Hook this up to your highlight manager later
    }

    private void ToggleEdges()
    {
        showingEdges = !showingEdges;
        UpdateButtonVisuals();
        // Hook this up to your highlight manager later
    }

    private void UpdateButtonVisuals()
    {
        // Simple active/inactive tint to show toggle state
        SetButtonActive(verticesButton, showingVertices);
        SetButtonActive(edgesButton, showingEdges);
    }

    private void SetButtonActive(Button btn, bool active)
    {
        var colors = btn.colors;
        colors.normalColor = active
            ? new Color(0.2f, 0.8f, 0.6f) // highlighted teal when on
            : new Color(1f, 1f, 1f, 0.15f); // subtle when off
        btn.colors = colors;
    }
}