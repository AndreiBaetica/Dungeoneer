using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SelectNamePopup : EditorWindow
{
    string path = "";
    string nameText = "referenceMaterial";
    GUIContent test = new GUIContent();

    bool isCreated = false;
    void OnGUI()
    {
        SeeThroughShaderEditorUtils.usualStart("Reference Material Creation");
        GUILayout.Space(10);
        System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        long ms = (long)(System.DateTime.UtcNow - epoch).TotalMilliseconds;
        long result = ms / 1000;
        if(nameText == "referenceMaterial")
        {
            nameText = nameText + result;
        }

        if(!isCreated)
        {
            nameText = EditorGUILayout.TextField("Choose a name: ", nameText);
            string locationText = EditorGUILayout.TextField("Choose a location: ", "Assets/See-through Shader/Core/");
            path = locationText + nameText + ".mat";

            EditorGUILayout.LabelField("Preview: " + path, EditorStyles.wordWrappedLabel);
            //GUILayout.Space(10);
            if (GUILayout.Button("Create"))
            {
                Material referenceMaterial = new Material(Shader.Find(SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader));
                //Material referenceMaterial = new Material(Shader.Find("Custom/SeeThroughShaderOriginal"));
                referenceMaterial.color = Color.white;
                referenceMaterial.SetFloat("_isReferenceMaterial", 1);
                AssetDatabase.CreateAsset(referenceMaterial, path);
                isCreated = true;

            }
            if (GUILayout.Button("Cancel")) this.Close();
        } 
        else
        {
            GUIStyle replacementStyle = new GUIStyle();
            replacementStyle.normal.textColor = Color.white;
            replacementStyle.alignment = TextAnchor.MiddleCenter;
            replacementStyle.fontStyle = FontStyle.Bold;

            GUIStyle centerStyle = new GUIStyle();
            centerStyle.normal.textColor = EditorStyles.wordWrappedLabel.normal.textColor;
            centerStyle.alignment = TextAnchor.MiddleCenter;
            centerStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.LabelField("Congratulations! " , centerStyle);
            EditorGUILayout.LabelField(nameText + ".mat" + " successfully got created!", replacementStyle);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Path: " + path, EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("Close")) this.Close();
        }
        //this.Repaint();
        SeeThroughShaderEditorUtils.usualEnd();


    }
}