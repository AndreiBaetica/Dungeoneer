using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerById))]
public class TriggerByIdEditor : Editor
{
    private SerializedProperty showDebugRays;

    private void OnEnable()
    {
        showDebugRays = serializedObject.FindProperty(nameof(BuildingAutoDetector.showDebugRays));

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SeeThroughShaderEditorUtils.usualStart("Trigger By Id");
        var oriCol = EditorStyles.label.normal.textColor;
        EditorStyles.label.normal.textColor = Color.white;
        base.DrawDefaultInspector();
        SeeThroughShaderEditorUtils.usualEnd();
        EditorStyles.label.normal.textColor = oriCol;
        serializedObject.ApplyModifiedProperties();
    }

}
