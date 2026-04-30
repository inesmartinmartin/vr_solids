using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public UIStrings[] availableLanguages;
    private UIStrings current;

    public static LocalizationManager Instance { get; private set; }
    public SystemLanguage CurrentLanguage => current.language;

    private void Awake()
    {
        Instance = this;
        // Try to match system language, fall back to first in list
        current = System.Array.Find(availableLanguages,
            l => l.language == Application.systemLanguage)
            ?? availableLanguages[0];
    }

    public UIStrings Strings => current;
}