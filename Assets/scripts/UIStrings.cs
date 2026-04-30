using UnityEngine;

[CreateAssetMenu(fileName = "UIStrings", menuName = "Project/UI Strings")]
public class UIStrings : ScriptableObject
{
    public SystemLanguage language;

    [Header("Idle panel")]
    public string dropPrompt;

    [Header("Section labels")]
    public string geometryLabel;
    public string factsLabel;
    public string natureLabel;

    [Header("Stat labels")]
    public string verticesLabel;
    public string edgesLabel;
    public string facesLabel;
    public string eulerLabel;

    [Header("Buttons")]
    public string showVerticesBtn;
    public string showEdgesBtn;
    public string showNetBtn;

    [Header("Nature categories")]
    public string categoryChemistry;
    public string categoryBiology;
    public string categoryMineralogy;
    public string categoryAstronomy;
    public string categoryEngineering;
    public string categoryOther;

    public string GetCategory(NatureCategory cat)
    {
        return cat switch
        {
            NatureCategory.Chemistry   => categoryChemistry,
            NatureCategory.Biology     => categoryBiology,
            NatureCategory.Mineralogy  => categoryMineralogy,
            NatureCategory.Astronomy   => categoryAstronomy,
            NatureCategory.Engineering => categoryEngineering,
            _                          => categoryOther
        };
    }
}