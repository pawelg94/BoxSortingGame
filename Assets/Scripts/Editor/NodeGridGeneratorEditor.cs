using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeGridGenerator))]
public class NodeGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NodeGridGenerator generator = (NodeGridGenerator)target;

        if (GUILayout.Button("Generate Node Grid"))
        {
            generator.GenerateGridInEditor();
        }

        if (GUILayout.Button("Clear Existing Nodes"))
        {
            generator.ClearGridInEditor();
        }
    }
}
