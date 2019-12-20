using UnityEngine;
using UnityEditor;

// Display Hex Grid coordinates using cube coordinates (x, y, z)

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {   
        HexCoordinates coordinates = new HexCoordinates(    // Extracting data
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
        );

        position = EditorGUI.PrefixLabel(position, label);  // Draws field title
        GUI.Label(position, coordinates.ToString());        // Draws field data
    }
}