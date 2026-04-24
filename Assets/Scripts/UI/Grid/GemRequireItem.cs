using UnityEngine;
using UnityEngine.UI;

public class GemRequireItem : MonoBehaviour
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
