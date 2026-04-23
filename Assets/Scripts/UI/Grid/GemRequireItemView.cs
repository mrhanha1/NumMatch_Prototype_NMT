using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View for a single gem requirement entry in the Gameplay HUD.
/// Bind to a prefab that contains:
///   - Image (gemIcon, direct child)
///   - Text (countText, direct child)
/// </summary>
public class GemRequireItemView : MonoBehaviour
{
    [SerializeField] private Image gemIcon;
    [SerializeField] private Text countText;

    /// <summary>Sets the gem sprite and remaining count. Hides the item if count is already 0.</summary>
    public void Bind(Sprite sprite, int remainingCount)
    {
        gemIcon.sprite = sprite;
        countText.text = remainingCount.ToString();
        gameObject.SetActive(remainingCount > 0);
    }

    /// <summary>Updates only the count text without re-assigning the sprite.</summary>
    public void UpdateCount(int remainingCount)
    {
        countText.text = remainingCount.ToString();
        gameObject.SetActive(remainingCount > 0);
    }
}
