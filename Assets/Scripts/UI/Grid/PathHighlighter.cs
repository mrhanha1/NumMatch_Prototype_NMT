using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Vẽ đường nối giữa 2 CellView bằng UI Image (RectTransform stretch).
/// Gắn vào cùng Canvas layer với GridView.
/// </summary>
public class PathHighlighter : MonoBehaviour
{
    [SerializeField] private Image linePrefab;
    [SerializeField] private Transform lineParent;

    private Image _currentLine;

    public void ShowPath(RectTransform from, RectTransform to)
    {
        HidePath();

        _currentLine = Instantiate(linePrefab, lineParent);

        Vector2 posA = from.anchoredPosition;
        Vector2 posB = to.anchoredPosition;
        Vector2 dir = posB - posA;
        float dist = dir.magnitude;

        var rt = _currentLine.rectTransform;
        rt.anchoredPosition = posA + dir * 0.5f;
        rt.sizeDelta = new Vector2(dist, rt.sizeDelta.y);
        rt.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    public void HidePath()
    {
        if (_currentLine != null)
        {
            Destroy(_currentLine.gameObject);
            _currentLine = null;
        }
    }
}