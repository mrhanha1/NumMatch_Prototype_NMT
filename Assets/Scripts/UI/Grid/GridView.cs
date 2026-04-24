using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class GridView : MonoBehaviour
{
    [SerializeField] private CellView cellViewPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private ScrollRect scrollRect;

    private CellView[,] _cells;
    private IObjectResolver _resolver;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        _resolver = resolver;
    }

    public void BuildGrid(CellModel[,] models)
    {
        foreach (Transform child in gridParent) Destroy(child.gameObject);

        int rows = models.GetLength(0);
        int cols = models.GetLength(1);
        _cells = new CellView[rows, cols];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                var cell = Instantiate(cellViewPrefab, gridParent);
                _resolver.InjectGameObject(cell.gameObject);
                cell.Init(models[r, c]);
                _cells[r, c] = cell;
            }

        RefreshContentSize();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void RefreshContentSize()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(gridParent as RectTransform);
    }

}