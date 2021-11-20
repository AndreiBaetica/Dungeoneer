using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerObjectId))]
public class TriggerObjectIdEditor : Editor
{
    private SerializedProperty showDebugRays;

    private void OnEnable()
    {
        showDebugRays = serializedObject.FindProperty(nameof(BuildingAutoDetector.showDebugRays));

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SeeThroughShaderEditorUtils.usualStart("Object Id For Trigger by Id");
        var oriCol = EditorStyles.label.normal.textColor;
        EditorStyles.label.normal.textColor = Color.white;
        base.DrawDefaultInspector();
        SeeThroughShaderEditorUtils.usualEnd();
        EditorStyles.label.normal.textColor = oriCol;
        serializedObject.ApplyModifiedProperties();
    }

}
