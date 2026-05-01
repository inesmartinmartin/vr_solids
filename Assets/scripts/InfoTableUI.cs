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

    [Header("Identity")]
    public Image accentBar;
    public TextMeshProUGUI solidFamilyText;
    public TextMeshProUGUI solidNameText;
    public TextMeshProUGUI descriptionText;

    [Header("Geometry")]
    public TextMeshProUGUI geometryLabel;
    public TextMeshProUGUI vertexStatText;
    public TextMeshProUGUI edgeStatText;
    public TextMeshProUGUI faceStatText;
    public TextMeshProUGUI eulerText;

    [Header("Facts")]
    public TextMeshProUGUI factsLabel;
    public TextMeshProUGUI fact1Text;
    public TextMeshProUGUI fact2Text;
    public TextMeshProUGUI fact3Text;

    [Header("Nature entries")]
    public TextMeshProUGUI natureLabel;
    public NatureEntryUI[] natureEntryUIs;  // assign 3 prefab instances in Inspector

    [Header("Buttons")]
    public Button verticesButton;
    public Button edgesButton;
    public Button netButton;

    [Header("Settings")]
    [Range(0f, 1f)] public float accentAlpha = 0.9f;

    private bool showingVertices = false;
    private bool showingEdges = false;

    private void Start()
    {
        ShowIdle();
        verticesButton.onClick.AddListener(ToggleVertices);
        edgesButton.onClick.AddListener(ToggleEdges);
        netButton.interactable = false;
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        var s = LocalizationManager.Instance.Strings;
        idleText.text               = s.dropPrompt;
        geometryLabel.text          = s.geometryLabel;
        factsLabel.text             = s.factsLabel;
        natureLabel.text            = s.natureLabel;

        verticesButton.GetComponentInChildren<TextMeshProUGUI>().text = s.showVerticesBtn;
        edgesButton.GetComponentInChildren<TextMeshProUGUI>().text    = s.showEdgesBtn;
        netButton.GetComponentInChildren<TextMeshProUGUI>().text      = s.showNetBtn;
    }

    public void ShowIdle()
    {
        idlePanel.SetActive(true);
        solidPanel.SetActive(false);
        showingVertices = false;
        showingEdges = false;
    }

    public void ShowSolid(SolidData data)
    {
        Debug.Log("ShowSolid called with: " + (data != null ? data.solidKey : "NULL DATA"));
        idlePanel.SetActive(false);
        solidPanel.SetActive(true);

        var s = LocalizationManager.Instance.Strings;
        var lang = LocalizationManager.Instance.CurrentLanguage;
        var content = data.GetContent(lang);

        // accent bar
        Color accent = data.solidColor;
        accent.a = accentAlpha;
        accentBar.color = accent;

        // identity
        solidFamilyText.text  = content.solidFamily;
        solidNameText.text    = content.solidName;
        descriptionText.text  = content.description;

        // geometry
        vertexStatText.text = $"{s.verticesLabel}\n{data.vertexCount}";
        edgeStatText.text   = $"{s.edgesLabel}\n{data.edgeCount}";
        faceStatText.text   = $"{s.facesLabel}\n{data.faceCount}";
        eulerText.text      = $"{s.eulerLabel}  {data.vertexCount} − {data.edgeCount} + {data.faceCount} = 2";

        // facts
        fact1Text.text = content.fact1;
        fact2Text.text = content.fact2;
        fact3Text.text = content.fact3;

        // nature entries
        for (int i = 0; i < natureEntryUIs.Length; i++)
        {
            bool hasEntry = content.natureEntries != null && i < content.natureEntries.Length;
            natureEntryUIs[i].gameObject.SetActive(hasEntry);
            if (hasEntry)
                natureEntryUIs[i].Populate(content.natureEntries[i], s);
        }

        showingVertices = false;
        showingEdges = false;
        UpdateButtonVisuals();
    }

    public void ToggleVertices()
    {
        InfoTableZone zone = FindFirstObjectByType<InfoTableZone>();
        if (zone == null) { Debug.LogError("ToggleVertices: InfoTableZone not found in scene"); return; }

        SolidIdentity currentSolid = zone.CurrentSolid;
        if (currentSolid == null) { Debug.LogError("ToggleVertices: CurrentSolid is null — is a solid on the table?"); return; }

        Debug.Log("ToggleVertices: found solid — " + currentSolid.gameObject.name);

        VertexHighlighter vh = currentSolid.gameObject.GetComponent<VertexHighlighter>();
        if (vh == null) { Debug.LogError("ToggleVertices: VertexHighlighter component missing on " + currentSolid.gameObject.name); return; }

        Debug.Log("ToggleVertices: calling Toggle()");
        vh.Toggle();

        showingVertices = !showingVertices;
        UpdateButtonVisuals();
    }
    private void ToggleEdges()    { showingEdges    = !showingEdges;    UpdateButtonVisuals(); }

    private void UpdateButtonVisuals()
    {
        SetButtonActive(verticesButton, showingVertices);
        SetButtonActive(edgesButton,    showingEdges);
    }

    private void SetButtonActive(Button btn, bool active)
    {
        var colors = btn.colors;
        colors.normalColor = active
            ? new Color(0.2f, 0.8f, 0.6f)
            : new Color(1f, 1f, 1f, 0.15f);
        btn.colors = colors;
    }
}