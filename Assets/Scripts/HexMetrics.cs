using UnityEngine;

// Constant data for Hex Grid, i.e. hexagon corner points relative to its center

public static class HexMetrics
{
    // Hex Basics
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;
    public const float solidFactor = 0.8f;
    public const float blendFactor = 1f - solidFactor;

    // Hex Elevation
    public const float elevationStep = 3f;

    public const int terracesPerSlope = 2;
    public const int terraceSteps = terracesPerSlope * 2 + 1;
    public const float horizontalTerraceStepSize = 1f / terraceSteps;
    public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);

    // Irregularity
    public static Texture2D noiseSource;
    public const float cellPerturbStrength = 4f;
    public const float noiseScale = 0.003f;
    public const float elevationPerturbStrength = 1.5f;

    //
    public const int chunkSizeX = 5, chunkSizeZ = 5;

    // 6 corner postion vectors of a hex cell
    static Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)                        // Extra copy of first corner to simplify mesh generation
    };

    public static Vector3 GetFirstCorner(HexDirection direction) { return corners[(int)direction]; }
    public static Vector3 GetSecondCorner(HexDirection direction) { return corners[(int)direction+1]; }

    public static Vector3 GetFirstSolidCorner(HexDirection direction) { return corners[(int)direction] * solidFactor; }
    public static Vector3 GetSecondSolidCorner(HexDirection direction) { return corners[(int)direction + 1] * solidFactor;}

    public static Vector3 GetBridge(HexDirection direction)
    {
        return (corners[(int)direction] + corners[(int)direction + 1]) *
            blendFactor;
    }

    // Hex Elevation Functions

    // Handling terraced terrain, an interpolation
    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
    {
        float h = step * HexMetrics.horizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;
        float v = ((step + 1) / 2) * HexMetrics.verticalTerraceStepSize;
        a.y += (b.y - a.y) * v;
        return a;
    }

    // Handling slope color transition, an interpolation
    public static Color TerraceLerp(Color a, Color b, int step)
    {
        float h = step * HexMetrics.horizontalTerraceStepSize;
        return Color.Lerp(a, b, h);
    }

    // Type of elevation change: Flat - 0, Slope - 1, Cliff - 2+
    public static HexEdgeType GetEdgeType(int elevation1, int elevation2)
    {
        if (elevation1 == elevation2)
        {
            return HexEdgeType.Flat;
        }
        int delta = elevation2 - elevation1;
        if (delta == 1 || delta == -1)
        {
            return HexEdgeType.Slope;
        }
        return HexEdgeType.Cliff;
    }

    // Irregularity Functions

    public static Vector4 SampleNoise(Vector3 position)
    {
        return noiseSource.GetPixelBilinear(position.x * noiseScale, position.z * noiseScale);
    }
}

public enum HexEdgeType
{
    Flat, Slope, Cliff
}