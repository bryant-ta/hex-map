using UnityEngine;
using UnityEngine.UI;

// Main Hex Grid

public class HexGrid : MonoBehaviour
{
    public int chunkCountX = 4, chunkCountZ = 3;
    int cellCountX, cellCountZ;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Texture2D noiseSource;

    HexCell[] cells;

    Canvas gridCanvas;
    HexMesh hexMesh;

    void Awake()
    {
        HexMetrics.noiseSource = noiseSource;

        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

        CreateCells();
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void OnEnable() 
    {
        // allow noiseSource to survive recompiles
        HexMetrics.noiseSource = noiseSource;
    }

    void CreateCells()
    {
        cells = new HexCell[cellCountZ * cellCountX];

        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        // Determine cell position
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);    // Creates offset and rectangualr hex grid
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        // Create/Setup cell
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        // Set neighbors in hex grid
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z&1) == 0)     // get even numbered rows
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if (x>0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }

        // Create text label containing cell coordinate
        Text label = Instantiate(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = x.ToString() + "\n" + z.ToString();
        cell.uiRect = label.rectTransform;

        cell.Elevation = 0;
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        return cells[index];
    }

    public void Refresh()
    {
        hexMesh.Triangulate(cells);
    }
}