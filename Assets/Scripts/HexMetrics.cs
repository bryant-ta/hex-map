using UnityEngine;

// Constant data for Hex Grid, i.e. hexagon corner points relative to its center

public static class HexMetrics
{
    public const float outerRadius = 10f;

    public const float innerRadius = outerRadius * 0.866025404f;

    // 6 corner postion vectors of a hex cell
    public static Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)                        // Extra copy of first corner to simplify mesh generation
    };
}