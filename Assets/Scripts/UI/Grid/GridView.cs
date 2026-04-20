using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using VContainer;
using VContainer.Unity;

public class GridView : MonoBehaviour
{
    [SerializeField] private CellView cellViewPrefab;
    [SerializeField] private Transform gridParent;

    private GameConfig _config;
    private CellView[,] _cells;
    private IObjectResolver _resolver;

    [Inject]
    public void Construct(GameConfig config, IObjectResolver resolver)
    {
        _config = config;
        _resolver = resolver;
    }

    public void BuildGrid(CellModel[,] models)
    {
        foreach (Transform child in gridParent) Destroy(child.gameObject);

        int rows = _config.row;
        int cols = _config.column;
        _cells = new CellView[rows, cols];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                var cell = Instantiate(cellViewPrefab, gridParent);
                _resolver.InjectGameObject(cell.gameObject);
                cell.Init(models[r, c]);
                _cells[r, c] = cell;
            }
    }
}