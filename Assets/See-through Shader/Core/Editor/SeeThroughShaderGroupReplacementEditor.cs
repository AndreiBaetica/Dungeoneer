using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SeeThroughShaderGeneralUtils;

[CustomEditor(typeof(SeeThroughShaderGroupReplacement))]
public class SeeThroughShaderGroupReplacementEditor : Editor
{
    private SeeThroughShaderGroupReplacement seeThroughShaderGroupReplacement;
    private SerializedProperty seeThroughShader;
    private SerializedProperty referenceMaterial;
    private SerializedProperty parentTransform;
    private SerializedProperty materialExemptions;
    private SerializedProperty layerMaskToAdd;


    private void OnEnable()
    {
        seeThroughShaderGroupReplacement = (SeeThroughShaderGroupReplacement)target;

        seeThroughShader = serializedObject.FindProperty(nameof(SeeThroughShaderGroupReplacement.seeThroughShader));
        referenceMaterial = serializedObject.FindProperty(nameof(SeeThroughShaderGroupReplacement.referenceMaterial));

        parentTransform = serializedObject.FindProperty(nameof(SeeThroughShaderGroupReplacement.parentTransform));

        materialExemptions = serializedObject.FindProperty(nameof(SeeThroughShaderGroupReplacement.materialExemptions));
        layerMaskToAdd = serializedObject.FindProperty(nameof(SeeThroughShaderGroupReplacement.layerMaskToAdd));

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SeeThroughShaderEditorUtils.usualStart("Shader Replacement By Group");
        var oriCol = EditorStyles.label.normal.textColor;
        EditorStyles.label.normal.textColor = Color.white;

        GUIStyle replacementStyle = new GUIStyle();
        replacementStyle.normal.textColor = Color.white;
        replacementStyle.alignment = TextAnchor.MiddleCenter;
        replacementStyle.fontStyle = FontStyle.Bold;

        //GUILayout.Label("Choose The Shader Version", replacementStyle);
        //SeeThroughShaderEditorUtils.DrawUILine();
        UnityVersionRenderPipelineShaderInfo unityVersionRenderPipelineShaderInfo = getUnityVersionAndRenderPipelineCorrectedShaderString();
        //EditorGUILayout.PropertyField(seeThroughShader);
        //if (seeThroughShader.objectReferenceValue == null)
        //{
        //    EditorGUILayout.HelpBox("If you don't choose a shader, the first shader found with the name '" +
        //        unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader +
        //        "' will be used. This is the recommended shader as it seems like you are using the " +
        //        unityVersionRenderPipelineShaderInfo.renderPipeline + " with Unity " + unityVersionRenderPipelineShaderInfo.unityVersion, MessageType.Info);

        //}
        //else
        //{
        //    if (!((Shader)seeThroughShader.objectReferenceValue).name.Equals(unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader))
        //    {
        //        EditorGUILayout.HelpBox("It seems like you are using the " + unityVersionRenderPipelineShaderInfo.renderPipeline +
        //            " with Unity " + unityVersionRenderPipelineShaderInfo.unityVersion + ". This would make '" +
        //            unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader + "' the recommended choice!", MessageType.Warning);

        //    }

        //}

        //SeeThroughShaderEditorUtils.makeHorizontalSeparation();

        GUILayout.Label("Choose The Reference Material", replacementStyle);
        SeeThroughShaderEditorUtils.DrawUILine();
        EditorGUILayout.PropertyField(referenceMaterial);
        if (referenceMaterial.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("You have to set a reference material, otherwise the 'See-through Shader' will be limited to the default shader settings and so won't work as expected!", MessageType.Error);

        }


        SeeThroughShaderEditorUtils.makeHorizontalSeparation();

        GUILayout.Label("Choose The Parent", replacementStyle);
        SeeThroughShaderEditorUtils.DrawUILine();
        EditorGUILayout.PropertyField(parentTransform);
        if (parentTransform.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("You didn't choose a parent transform, that means that this GameObject, '" + 
                seeThroughShaderGroupReplacement.name + "' will be the parent. Every child  of it that isn't in" +
                " the exemption list will get the See-through Shader assigned to its material during runtime.", MessageType.Info);
        }

        SeeThroughShaderEditorUtils.makeHorizontalSeparation();

        GUILayout.Label("Choose The Exemptions", replacementStyle);
        SeeThroughShaderEditorUtils.DrawUILine();
        EditorGUILayout.PropertyField(materialExemptions);
        // TODO: add info text
        //if (materialExemptions.objectReferenceValue == null)
        //{
        //    EditorGUILayout.HelpBox("", MessageType.Info);
        //}
        EditorGUILayout.PropertyField(layerMaskToAdd);
        if (layerMaskToAdd.intValue == 0)
        {
            EditorGUILayout.HelpBox("You didn't select any layer! Shader assignment won't happen!", MessageType.Error);
        }
        SeeThroughShaderEditorUtils.makeHorizontalSeparation();

        SeeThroughShaderEditorUtils.usualEnd();
        EditorStyles.label.normal.textColor = oriCol;
        serializedObject.ApplyModifiedProperties();
    }
}
