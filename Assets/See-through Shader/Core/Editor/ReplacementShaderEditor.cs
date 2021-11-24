﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using static SeeThroughShaderGeneralUtils;

[CustomEditor(typeof(ReplacementShader))]
public class ReplacementShaderEditor : Editor
{
    private ReplacementShader replacementShader;
    private SerializedProperty shaderToBeReplaced;
    private SerializedProperty referenceMaterial;
    AnimBool replacementByReplacementShaderAnim;
    private SerializedProperty replacementByReplacementShader;
    private SerializedProperty layerMasksWithReplacementProperty;


    private SerializedProperty replacementCamera;

    private void OnEnable()
    {
        replacementShader = (ReplacementShader)target;
        replacementByReplacementShaderAnim = new AnimBool(false);
        replacementByReplacementShaderAnim.valueChanged.AddListener(Repaint);

        replacementByReplacementShader = serializedObject.FindProperty(nameof(ReplacementShader.replacementByReplacementShader));
        shaderToBeReplaced = serializedObject.FindProperty(nameof(ReplacementShader.replacementShader));
        referenceMaterial = serializedObject.FindProperty(nameof(ReplacementShader.referenceMaterial));

        layerMasksWithReplacementProperty = serializedObject.FindProperty(nameof(ReplacementShader.layerMasksWithReplacement));

        replacementCamera = serializedObject.FindProperty(nameof(ReplacementShader.replacementCamera));

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var oriCol = EditorStyles.label.normal.textColor;

        SeeThroughShaderEditorUtils.usualStart("Global Shader Replacement Manager");


        GUIStyle shaderStyle = new GUIStyle();
        shaderStyle.normal.textColor = Color.white;
        Color origColor = EditorStyles.label.normal.textColor;
        EditorStyles.label.normal.textColor = Color.white;
        GUIStyle replacementStyle = new GUIStyle();
        replacementStyle.normal.textColor = Color.white;
        replacementStyle.alignment = TextAnchor.MiddleCenter;
        replacementStyle.fontStyle = FontStyle.Bold;


        // maybe add description
        //GUIStyle textStyle = EditorStyles.label;
        //textStyle.wordWrap = true;
        //GUILayout.Label("This script globally replaces all shaders with the 'See-through Shader' while keeping standard shader property values. " +
        //                "The 'See-through Shader' property values are taken from the chosen reference material. Additionally you can choose to " +
        //                "apply the shader only to the materials of gameobjects on certain layers.", textStyle);

        //SeeThroughShaderEditorUtils.makeHorizontalSeparation();


        // // not necessary for user to select which version depending on RP. 
        //GUILayout.Label("Choose Which Shader To Use", replacementStyle);
        //SeeThroughShaderEditorUtils.DrawUILine();
        UnityVersionRenderPipelineShaderInfo unityVersionRenderPipelineShaderInfo = getUnityVersionAndRenderPipelineCorrectedShaderString();
        //EditorGUILayout.PropertyField(shaderToBeReplaced);
        //index = EditorGUILayout.Popup("Dedicated Trigger Type: ", index, options);
        //if (shaderToBeReplaced.objectReferenceValue == null)
        //{
        //    EditorGUILayout.HelpBox("If you don't choose a shader, the first shader found with the name '" +
        //        unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader +
        //        "' will be used. This is the recommended shader as it seems like you are using the " +
        //        unityVersionRenderPipelineShaderInfo.renderPipeline + " with Unity " + unityVersionRenderPipelineShaderInfo.unityVersion, MessageType.Info);

        //}
        //else
        //{
        //    if (!((Shader)shaderToBeReplaced.objectReferenceValue).name.Equals(unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader))
        //    {
        //        EditorGUILayout.HelpBox("It seems like you are using the " + unityVersionRenderPipelineShaderInfo.renderPipeline +
        //            " with Unity " + unityVersionRenderPipelineShaderInfo.unityVersion + ". This would make '" +
        //            unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader + "' the recommended choice!", MessageType.Warning);

        //    }

        //}


        SeeThroughShaderEditorUtils.makeHorizontalSeparation();

        GUILayout.Label("Choose The Reference Material", replacementStyle);
        SeeThroughShaderEditorUtils.DrawUILine();
        EditorGUILayout.PropertyField(referenceMaterial);
        if (referenceMaterial.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("You have to set a reference material, otherwise the 'See-through Shader' will be limited to the default shader settings and so won't work as expected!", MessageType.Error);
            
        }

        SeeThroughShaderEditorUtils.makeHorizontalSeparation();

        GUIStyle optionStyle = new GUIStyle();
        optionStyle.normal.textColor = Color.white;
        //optionStyle.fontSize = 15;
        optionStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Choose Replacement Strategy", replacementStyle);
        SeeThroughShaderEditorUtils.DrawUILine();
        if (unityVersionRenderPipelineShaderInfo.renderPipeline.Equals("URP") || unityVersionRenderPipelineShaderInfo.renderPipeline.Equals("HDRP"))
        {
            EditorGUILayout.PropertyField(layerMasksWithReplacementProperty, new GUIContent("Layer Mask"));
            if (layerMasksWithReplacementProperty.intValue == 0)
            {
                replacementByReplacementShader.boolValue = false;
                EditorGUILayout.HelpBox("You didn't select any layer! Shader replacement won't happen!", MessageType.Error);

            }
        } 
        else
        {
            replacementByReplacementShader.boolValue = EditorGUILayout.ToggleLeft("Use Replacement Shader", replacementByReplacementShader.boolValue, optionStyle);
            replacementByReplacementShaderAnim.target = !replacementByReplacementShader.boolValue;
            if (EditorGUILayout.BeginFadeGroup(replacementByReplacementShaderAnim.faded))
            {
                //EditorGUILayout.LabelField("test: ");
                EditorGUILayout.PropertyField(layerMasksWithReplacementProperty, new GUIContent("Layer Mask"));
                if (layerMasksWithReplacementProperty.intValue == 0)
                {
                    EditorGUILayout.HelpBox("You didn't select any layer! Shader replacement won't happen!", MessageType.Error);

                }
            }
            else
            {
                //EditorGUI.PropertyField(layerMasksWithReplacementProperty);
            }
            EditorGUILayout.EndFadeGroup();
            if (replacementByReplacementShader.boolValue == true)
            {

                EditorGUILayout.PropertyField(replacementCamera);
                if(replacementCamera.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("To use this feature you have to supply a camera. Please choose a camera!", MessageType.Warning);
                }

                EditorGUILayout.HelpBox("This is NOT RECOMMENDED! Every shader visible in the camera gets replaced via Unity's Camera.SetReplacementShader()! This won't work if you got Terrain or UI elements! " +
                    "Additionally this method only works with the 'Effect Radius Only' method. You won't be able to use any advanced per-building/object basis features like transitions etc.", MessageType.Warning);

            }
        }

        SeeThroughShaderEditorUtils.usualEnd();
        EditorStyles.label.normal.textColor = oriCol;
        serializedObject.ApplyModifiedProperties();
    }




}
