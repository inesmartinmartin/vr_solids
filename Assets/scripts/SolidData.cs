using UnityEngine;

[CreateAssetMenu(fileName = "NewSolidData", menuName = "Project/Solid Data")]
public class SolidData : ScriptableObject
{
    [Header("Identity")]
    public string solidKey;           // stable ID, e.g. "icosahedron"
    public string solidFamily;        // fallback English family name
    public string description;
    public Color solidColor = Color.white;

    [Header("Geometry")]
    public int vertexCount;
    public int edgeCount;
    public int faceCount;

    [Header("Localized content")]
    public LocalizedSolidContent[] localizations;

    public LocalizedSolidContent GetContent(SystemLanguage lang)
    {
        foreach (var l in localizations)
            if (l.language == lang) return l;
        foreach (var l in localizations)
            if (l.language == SystemLanguage.English) return l;
        return localizations.Length > 0 ? localizations[0] : null;
    }
}

[System.Serializable]
public class LocalizedSolidContent
{
    public SystemLanguage language;
    public string solidName;
    public string solidFamily;
    [TextArea(2, 4)] public string description;
    [TextArea(1, 3)] public string fact1;
    [TextArea(1, 3)] public string fact2;
    [TextArea(1, 3)] public string fact3;
    public NatureEntry[] natureEntries;
}

[System.Serializable]
public class NatureEntry
{
    public string title;
    [TextArea(1, 3)] public string description;
    public NatureCategory category;
}

public enum NatureCategory
{
    Chemistry,
    Biology,
    Mineralogy,
    Astronomy,
    Engineering,
    Other
}