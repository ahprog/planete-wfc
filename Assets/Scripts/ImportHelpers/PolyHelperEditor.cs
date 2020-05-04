using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PolyHelper))]
public class PolyHelperEditor : Editor
{
    PolyHelper polyHelper;

    private void OnEnable()
    {
        polyHelper = (PolyHelper)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Build faces")) {
            polyHelper.BuildFaces();
        }

        if (GUILayout.Button("Link neighbours")) {
            polyHelper.LinkNeighbours();
        }
    }
}
