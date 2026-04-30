using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NatureEntryUI : MonoBehaviour
{
    public Image categoryIcon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    // One sprite per category — assign in Inspector
    public Sprite[] categorySprites; // order matches NatureCategory enum

    public void Populate(NatureEntry entry, UIStrings strings)
    {
        titleText.text = entry.title;
        descriptionText.text = entry.description;

        int idx = (int)entry.category;
        if (categorySprites != null && idx < categorySprites.Length)
            categoryIcon.sprite = categorySprites[idx];

        categoryIcon.color = GetCategoryColor(entry.category);
    }

    private Color GetCategoryColor(NatureCategory cat)
    {
        return cat switch
        {
            NatureCategory.Chemistry   => new Color(0.22f, 0.62f, 0.46f),
            NatureCategory.Biology     => new Color(0.24f, 0.62f, 0.22f),
            NatureCategory.Mineralogy  => new Color(0.73f, 0.35f, 0.19f),
            NatureCategory.Astronomy   => new Color(0.22f, 0.47f, 0.85f),
            NatureCategory.Engineering => new Color(0.73f, 0.46f, 0.09f),
            _                          => new Color(0.53f, 0.53f, 0.53f)
        };
    }
}