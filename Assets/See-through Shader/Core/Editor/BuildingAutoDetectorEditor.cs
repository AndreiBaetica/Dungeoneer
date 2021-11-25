using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingAutoDetector))]
public class BuildingAutoDetectorEditor : Editor
{
    private SerializedProperty showDebugRays;

    private void OnEnable()
    {
        showDebugRays = serializedObject.FindProperty(nameof(BuildingAutoDetector.showDebugRays));

    }
    public override void OnInspectorGUI()
    {


        serializedObject.Update();
        var oriCol = EditorStyles.label.normal.textColor;
        SeeThroughShaderEditorUtils.usualStart("Building Auto-Detector");
        EditorStyles.label.normal.textColor = Color.white;
        EditorGUILayout.PropertyField(showDebugRays);
        SeeThroughShaderEditorUtils.usualEnd();
        EditorStyles.label.normal.textColor = oriCol;
        serializedObject.ApplyModifiedProperties();
    }

}
