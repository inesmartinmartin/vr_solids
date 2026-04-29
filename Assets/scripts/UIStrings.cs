using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UIStrings", menuName = "Project/UI Strings")]
public class UIStrings : ScriptableObject
{
    public SystemLanguage language;
    public string dropPrompt;
    public string verticesLabel;
    public string edgesLabel;
    public string facesLabel;
    public string eulerLabel;
    public string showVerticesBtn;
    public string showEdgesBtn;
    public string showNetBtn;
}