using UnityEngine;

// Uses cube coordinate layout

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X { get { return x; } }
    public int Z { get { return z; } }
    public int Y { get { return -X - Z; } }     // Y derivable from X and Z in cube coords

    public HexCoordinates (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // Offset coords -> hex coords. Undos offset that forced X axis to zig-zag
    public static HexCoordinates FromOffsetCoordinates (int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    // position -> hex coords
    public static HexCoordinates FromPosition (Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);       // Some math voodoo here in this whole function...
        float y = -x;                                               // Seems needlessly complicated...
        float offset = position.z / (HexMetrics.outerRadius * 3f);  // Just use event that passes clicked cell coordinate??
        x -= offset;
        y -= offset;
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)                      // Edge cases between hexagons has rounding error?
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}