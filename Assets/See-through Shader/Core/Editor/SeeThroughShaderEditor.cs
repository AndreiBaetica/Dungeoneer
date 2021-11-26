using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;

public class SeeThroughShaderEditor : ShaderGUI
{


    private static class Styles
    {
        // Standard
        public static GUIContent albedoText = EditorGUIUtility.TrTextContent("Albedo", "Albedo (RGB)");
        public static GUIContent metallicMapText = EditorGUIUtility.TrTextContent("Metallic", "Metallic value");
        public static GUIContent smoothnessText = EditorGUIUtility.TrTextContent("Smoothness", "Smoothness value");
        public static GUIContent normalMapText = EditorGUIUtility.TrTextContent("Normal Map", "Normal Map");

        // See-through Shader
        public static GUIContent dissolveText = EditorGUIUtility.TrTextContent("Dissolve Effect Texture", "Dissolve Effect Texture");
        public static GUIContent dissolveSizeText = EditorGUIUtility.TrTextContent("Dissolve Texture Scale", "Dissolve Texture Scale");
        public static GUIContent dissolveEmissionText = EditorGUIUtility.TrTextContent("Strenght", "Dissolve Emission Strength");
        public static GUIContent dissolveEmissionBoosterText = EditorGUIUtility.TrTextContent("Dissolve Emission Booster", "Dissolve Emission Booster");
        public static GUIContent dissolveColorSaturationText = EditorGUIUtility.TrTextContent("Saturation", "Dissolve Color Saturation");

        public static string standardShaderText = "Standard Shader Properties";
        public static string advancedText = "Advanced Options";
        public static string stsShaderText = "See-through Shader Properties";

    }

    MaterialProperty isReferenceMaterialMat = null;

    // Standard
    MaterialProperty albedoMap = null;
    MaterialProperty albedoColor = null;
    MaterialProperty metallic = null;
    MaterialProperty smoothness = null;
    MaterialProperty bumpScale = null;
    MaterialProperty bumpMap = null;

    // See-through Shader
    MaterialProperty dissolveMap = null;
    MaterialProperty dissolveColor = null;
    MaterialProperty dissolveSize = null;
    MaterialProperty dissolveColorSaturation = null;

    MaterialProperty dissolveEmmission = null;
    MaterialProperty dissolveEmmissionBooster = null;
    MaterialProperty dissolveTexturedEmissionEdge = null;
    AnimBool dissolveTexturedEmissionEdgeAnimBool;
    MaterialProperty dissolveTexturedEmissionEdgeStrength = null;

    MaterialProperty dissolveClippedShadowsEnabled = null;

    MaterialProperty dissolveTextureAnimationEnabled = null;
    AnimBool dissolveTextureAnimationEnabledAnimBool;
    MaterialProperty dissolveTextureAnimationSpeed = null;
    MaterialProperty dissolveTransitionDuration = null;


    MaterialProperty obstructionMode = null;
    MaterialProperty angleStrength = null;
    MaterialProperty coneStrength = null;
    MaterialProperty coneObstructionDestroyRadius = null;
    MaterialProperty cylinderStrength = null;
    MaterialProperty cylinderObstructionDestroyRadius = null;
    MaterialProperty circleStrength = null;
    MaterialProperty circleObstructionDestroyRadius = null;
    MaterialProperty dissolveFallOff = null;
    MaterialProperty intrinsicDissolveStrength = null;

    MaterialProperty floorMode = null;
    MaterialProperty floorY = null;
    MaterialProperty playerPosYOffset = null;
    MaterialProperty floorYTextureGradientLength = null;

    MaterialProperty preview = null;

    MaterialEditor m_MaterialEditor;

    bool m_FirstTimeApply = true;

    Color oriCol;

    bool isReferenceMaterial;


    public void FindProperties(MaterialProperty[] props)
    {
        isReferenceMaterialMat = FindProperty("_isReferenceMaterial", props);

        // Standard
        albedoMap = FindProperty("_MainTex", props);
        albedoColor = FindProperty("_Color", props);
        metallic = FindProperty("_Metallic", props);
        smoothness = FindProperty("_Glossiness", props);
        bumpScale = FindProperty("_BumpScale", props);
        bumpMap = FindProperty("_BumpMap", props);

        // See-through Shader
        dissolveMap = FindProperty("_DissolveTex", props);
        dissolveColor = FindProperty("_DissolveColor", props);
        dissolveSize = FindProperty("_UVs", props);
        dissolveColorSaturation = FindProperty("_DissolveColorSaturation", props);

        dissolveEmmission = FindProperty("_DissolveEmission", props);
        dissolveEmmissionBooster = FindProperty("_DissolveEmissionBooster", props);
        dissolveTexturedEmissionEdge = FindProperty("_TexturedEmissionEdge", props);
        dissolveTexturedEmissionEdgeStrength = FindProperty("_TexturedEmissionEdgeStrength", props);

        dissolveClippedShadowsEnabled = FindProperty("_hasClippedShadows", props);
        

        dissolveTextureAnimationEnabled = FindProperty("_AnimationEnabled", props);
        dissolveTextureAnimationSpeed = FindProperty("_AnimationSpeed", props);
        dissolveTransitionDuration = FindProperty("_TransitionDuration", props);



        obstructionMode = FindProperty("_Obstruction", props);
        angleStrength = FindProperty("_AngleStrength", props);
        coneStrength = FindProperty("_ConeStrength", props);
        coneObstructionDestroyRadius = FindProperty("_ConeObstructionDestroyRadius", props);

        cylinderStrength = FindProperty("_CylinderStrength", props);
        cylinderObstructionDestroyRadius = FindProperty("_CylinderObstructionDestroyRadius", props);

        circleStrength = FindProperty("_CircleStrength", props);
        circleObstructionDestroyRadius = FindProperty("_CircleObstructionDestroyRadius", props);

        dissolveFallOff = FindProperty("_DissolveFallOff", props);
        intrinsicDissolveStrength = FindProperty("_IntrinsicDissolveStrength", props);

        floorMode = FindProperty("_Floor", props);
        floorY = FindProperty("_FloorY", props);
        playerPosYOffset = FindProperty("_PlayerPosYOffset", props);
        floorYTextureGradientLength = FindProperty("_FloorYTextureGradientLength", props);

        preview = FindProperty("_PreviewMode", props);       

    }

    void DoSetup(MaterialEditor materialEditor)
    {
        dissolveTexturedEmissionEdgeAnimBool = new AnimBool(false);
        dissolveTexturedEmissionEdgeAnimBool.valueChanged.AddListener(materialEditor.Repaint);

        dissolveTextureAnimationEnabledAnimBool = new AnimBool(false);
        dissolveTextureAnimationEnabledAnimBool.valueChanged.AddListener(materialEditor.Repaint);

        oriCol = EditorStyles.label.normal.textColor;
    }
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {

        FindProperties(properties);
        m_MaterialEditor = materialEditor;
        Material material = materialEditor.target as Material;

        if (m_FirstTimeApply)
        {
            MaterialChanged(material);
            DoSetup(materialEditor);
            m_FirstTimeApply = false;
        }

        if (isReferenceMaterialMat.floatValue == 1)
        {
            isReferenceMaterial = true;
        }
        string name = isReferenceMaterial ? "Reference Shader" : "The Shader";


        SeeThroughShaderEditorUtils.usualStart(name);
        EditorStyles.label.normal.textColor = Color.white;


        if (!isReferenceMaterial)
        {
            StandardShaderPropertiesGUI(material);
            SeeThroughShaderEditorUtils.makeHorizontalSeparation();
        }


        STSShaderPropertiesGUI(material);

        SeeThroughShaderEditorUtils.makeHorizontalSeparation();



        EditorStyles.label.normal.textColor = oriCol;
        SeeThroughShaderEditorUtils.usualEnd();
    }



    void StandardShaderPropertiesGUI(Material material)
    {
        EditorGUIUtility.labelWidth = 0f;

        EditorGUI.BeginChangeCheck();
        {
            GUIStyle replacementStyle = new GUIStyle();
            replacementStyle.normal.textColor = Color.white;
            replacementStyle.alignment = TextAnchor.MiddleCenter;
            replacementStyle.fontStyle = FontStyle.Bold;

            GUILayout.Label(Styles.standardShaderText, replacementStyle);
            SeeThroughShaderEditorUtils.DrawUILine();
            DoAlbedoArea(material);
            EditorGUI.indentLevel += 2;
            DoSpecularMetallicArea();
            EditorGUI.indentLevel -= 2;
            DoNormalArea();
            if (albedoMap.textureValue != null || bumpMap.textureValue != null)
            {
                EditorGUI.indentLevel += 2;
                m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);
                EditorGUI.indentLevel -= 2;
            }

        }
        if (EditorGUI.EndChangeCheck())
        {
            MaterialChanged(material);
        }

        EditorGUILayout.Space();

        GUILayout.Label(Styles.advancedText, EditorStyles.boldLabel);
        m_MaterialEditor.RenderQueueField();
        m_MaterialEditor.EnableInstancingField();
        m_MaterialEditor.DoubleSidedGIField();
    }

    void STSShaderPropertiesGUI(Material material)
    {
        EditorGUIUtility.labelWidth = 0f;

        EditorGUI.BeginChangeCheck();
        {
            GUIStyle replacementStyle = new GUIStyle();
            replacementStyle.normal.textColor = Color.white;
            replacementStyle.alignment = TextAnchor.MiddleCenter;
            replacementStyle.fontStyle = FontStyle.Bold;

            GUILayout.Label(Styles.stsShaderText, replacementStyle);
            SeeThroughShaderEditorUtils.DrawUILine();
            DoDissolveArea();
            EditorGUILayout.Space();
            DoPlayerObstructionOptionsArea();
            EditorGUILayout.Space();
            DoAnimationArea();
            EditorGUILayout.Space();
        }
        if (EditorGUI.EndChangeCheck())
        {
            MaterialChanged(material);
        }

    }

    void DoAlbedoArea(Material material)
    {
        float oriLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
        m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
        EditorGUIUtility.labelWidth = oriLabelWidth;
    }
    void DoNormalArea()
    {
        m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
    }

    void DoSpecularMetallicArea()
    {
        m_MaterialEditor.ShaderProperty(metallic, Styles.metallicMapText);
        m_MaterialEditor.ShaderProperty(smoothness, Styles.smoothnessText);
    }

    void DoDissolveArea()
    {
        GUILayout.Label("Dissolve Effect Texture and Styling", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        float originalLabelWidht = EditorGUIUtility.labelWidth;
        if(!isReferenceMaterial)
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
        } else
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 80;
        }

        m_MaterialEditor.TexturePropertySingleLine(Styles.dissolveText, dissolveMap, dissolveColor);

        //EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
        if (dissolveMap.textureValue==null)
        {
            EditorGUILayout.HelpBox("You didn't select any dissolve texture! The 'See-through Shader' effect won't work without it!", MessageType.Error);
        } else
        {
            //EditorGUI.indentLevel += 2;
            //m_MaterialEditor.TextureScaleOffsetProperty(dissolveMap);
            //EditorGUI.indentLevel -= 2;
            EditorGUI.indentLevel += 2;
            m_MaterialEditor.ShaderProperty(dissolveSize, Styles.dissolveSizeText);
            m_MaterialEditor.ShaderProperty(dissolveColorSaturation,Styles.dissolveColorSaturationText);
            EditorGUILayout.Space();

                    SeeThroughShaderEditorUtils.DrawUILineSubMenu();

            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUILayout.LabelField("Emission");
            EditorStyles.label.normal.textColor = Color.white;
            EditorGUI.indentLevel += 1;

            m_MaterialEditor.ShaderProperty(dissolveEmmission, Styles.dissolveEmissionText);

            if(dissolveEmmission.floatValue > 0)
            {
                m_MaterialEditor.ShaderProperty(dissolveEmmissionBooster, Styles.dissolveEmissionBoosterText);

                EditorGUILayout.Space();
                dissolveTexturedEmissionEdge.floatValue = Convert.ToSingle(EditorGUILayout.Toggle("Textured Emission Edge", Convert.ToBoolean(dissolveTexturedEmissionEdge.floatValue)));
                dissolveTexturedEmissionEdgeAnimBool.target = dissolveTexturedEmissionEdge.floatValue == 1;
                if (EditorGUILayout.BeginFadeGroup(dissolveTexturedEmissionEdgeAnimBool.faded))
                {
                    //EditorGUI.indentLevel++;
                    m_MaterialEditor.ShaderProperty(dissolveTexturedEmissionEdgeStrength, "Strength");
                    //EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFadeGroup();

                EditorGUILayout.Space();

                SeeThroughShaderEditorUtils.DrawUILineSubMenu();

                EditorGUI.indentLevel -= 1;
                EditorStyles.label.normal.textColor = oriCol;
                EditorGUILayout.LabelField("Shadows");
                EditorStyles.label.normal.textColor = Color.white;
                EditorGUI.indentLevel += 1;

                //dissolveClippedShadowsEnabled.floatValue = Convert.ToSingle(EditorGUILayout.Toggle("Has Clipped Shadows", Convert.ToBoolean(dissolveClippedShadowsEnabled.floatValue)));

                m_MaterialEditor.ShaderProperty(dissolveClippedShadowsEnabled, "Has Clipped Shadows");

                EditorGUI.indentLevel -= 2;
            }

        
        }
        //m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);

    }

    void DoAnimationArea()
    {
        SeeThroughShaderEditorUtils.DrawUILineGray();
        GUILayout.Label("Dissolve Effect Animations", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        //EditorGUI.indentLevel += 2;

        EditorGUI.indentLevel -= 1;
        EditorStyles.label.normal.textColor = oriCol;
        EditorGUILayout.LabelField("Dissolve Texture");
        EditorStyles.label.normal.textColor = Color.white;
        EditorGUI.indentLevel += 1;

        GUIStyle optionStyle = new GUIStyle();
        optionStyle.normal.textColor = Color.white;
        //optionStyle.fontSize = 15;
        optionStyle.fontStyle = FontStyle.Bold;
        //m_MaterialEditor.ShaderProperty(dissolveTextureAnimationEnabled, Styles.dissolveEmissionBoosterText);
        dissolveTextureAnimationEnabled.floatValue = Convert.ToSingle(EditorGUILayout.Toggle("Animation Enabled", Convert.ToBoolean(dissolveTextureAnimationEnabled.floatValue)));
        dissolveTextureAnimationEnabledAnimBool.target = dissolveTextureAnimationEnabled.floatValue == 1;
        if (EditorGUILayout.BeginFadeGroup(dissolveTextureAnimationEnabledAnimBool.faded))
        {
            //EditorGUI.indentLevel++;
            m_MaterialEditor.ShaderProperty(dissolveTextureAnimationSpeed, "Speed");
            //EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        EditorGUILayout.Space();

        SeeThroughShaderEditorUtils.DrawUILineSubMenu();

        EditorGUI.indentLevel -= 1;
        EditorStyles.label.normal.textColor = oriCol;
        EditorGUILayout.LabelField("Enter/Exit Transition");
        EditorStyles.label.normal.textColor = Color.white;
        EditorGUI.indentLevel += 1;

        m_MaterialEditor.ShaderProperty(dissolveTransitionDuration, "Transition Duration In Seconds");
        //if (dissolveTextureAnimationEnabled.floatValue==1)
        //{
        //    m_MaterialEditor.ShaderProperty(dissolveTextureAnimationSpeed, Styles.dissolveEmissionBoosterText);
        //}
        EditorGUI.indentLevel -= 2;
    }

    void DoPlayerObstructionOptionsArea()
    {
        SeeThroughShaderEditorUtils.DrawUILineGray();
        GUILayout.Label("Player Obstruction Options", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUI.indentLevel += 2;

        EditorGUI.indentLevel -= 1;
        EditorStyles.label.normal.textColor = oriCol;
        EditorGUILayout.LabelField("Obstruction Settings");
        EditorStyles.label.normal.textColor = Color.white;
        EditorGUI.indentLevel += 1;

        float oriLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2;
        m_MaterialEditor.ShaderProperty(obstructionMode, "Obstruction Mode");
        EditorGUIUtility.labelWidth = oriLabelWidth;

        EditorGUI.indentLevel += 1;
        EditorGUI.indentLevel += 1;

        if(obstructionMode.floatValue != 0)
        {
            Rect rect = EditorGUILayout.BeginVertical();
            rect.width -= 40;
            rect.x += 40;
            GUI.Box(rect, GUIContent.none);
        }


        if (obstructionMode.floatValue == 1 || obstructionMode.floatValue == 3 || obstructionMode.floatValue == 5)
        {
            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUILayout.LabelField("Angle Obstruction");
            EditorStyles.label.normal.textColor = Color.white;
            EditorGUI.indentLevel += 1;
        }
        m_MaterialEditor.ShaderProperty(angleStrength, "Strength");

        if (obstructionMode.floatValue == 2 || obstructionMode.floatValue == 3)
        {
            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUILayout.LabelField("Cone Obstruction");
            EditorStyles.label.normal.textColor = Color.white;
            EditorGUI.indentLevel += 1;
        }

        m_MaterialEditor.ShaderProperty(coneStrength, "Strength");
        m_MaterialEditor.ShaderProperty(coneObstructionDestroyRadius, "Obstruction Destroy Radius");

        if (obstructionMode.floatValue == 4 || obstructionMode.floatValue == 5)
        {
            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUILayout.LabelField("Cylinder Obstruction");
            EditorStyles.label.normal.textColor = Color.white;
            EditorGUI.indentLevel += 1;
        }

        m_MaterialEditor.ShaderProperty(cylinderStrength, "Strength");
        m_MaterialEditor.ShaderProperty(cylinderObstructionDestroyRadius, "Obstruction Destroy Radius");

        if (obstructionMode.floatValue == 6)
        {
            EditorGUI.indentLevel -= 1;
            EditorStyles.label.normal.textColor = oriCol;
            EditorGUILayout.LabelField("Circle Obstruction");
            EditorStyles.label.normal.textColor = Color.white;
            EditorGUI.indentLevel += 1;
        }

        m_MaterialEditor.ShaderProperty(circleStrength, "Strength");
        m_MaterialEditor.ShaderProperty(circleObstructionDestroyRadius, "Obstruction Destroy Radius");


        makeAlwaysPositiv(coneObstructionDestroyRadius);
        makeAlwaysPositiv(cylinderObstructionDestroyRadius);
        makeAlwaysPositiv(circleObstructionDestroyRadius);
        //EditorGUILayout.Space();

        m_MaterialEditor.ShaderProperty(dissolveFallOff, "FallOff");

        if (obstructionMode.floatValue != 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        EditorGUI.indentLevel -= 1;

        EditorGUI.indentLevel -= 1;
        EditorStyles.label.normal.textColor = oriCol;
        EditorGUILayout.LabelField("Intrinsic Dissolve Obstruction");
        EditorStyles.label.normal.textColor = Color.white;
        EditorGUI.indentLevel += 1;

        m_MaterialEditor.ShaderProperty(intrinsicDissolveStrength, "Strength");

        EditorGUI.indentLevel -= 1;

        EditorGUILayout.Space();

        SeeThroughShaderEditorUtils.DrawUILineSubMenu();

        EditorGUI.indentLevel -= 1;
        EditorStyles.label.normal.textColor = oriCol;
        EditorGUILayout.LabelField("Floor Settings");
        EditorStyles.label.normal.textColor = Color.white;
        EditorGUI.indentLevel += 1;


        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2;
        m_MaterialEditor.ShaderProperty(floorMode, "Floor Mode");
        EditorGUIUtility.labelWidth = oriLabelWidth;
        m_MaterialEditor.ShaderProperty(floorY, "FloorY");
        m_MaterialEditor.ShaderProperty(playerPosYOffset, "PlayerPos Y Offset");
        m_MaterialEditor.ShaderProperty(floorYTextureGradientLength, "FloorY Texture Gradient Length");


        EditorGUILayout.Space();

        SeeThroughShaderEditorUtils.DrawUILineSubMenu();

        EditorGUI.indentLevel -= 1;
        EditorStyles.label.normal.textColor = oriCol;
        EditorGUILayout.LabelField("Debug");
        EditorStyles.label.normal.textColor = Color.white;
        EditorGUI.indentLevel += 1;

        m_MaterialEditor.ShaderProperty(preview, preview.displayName);

    }

    static void MaterialChanged(Material material)
    {      
        SetMaterialKeywords(material);
    }

    static void SetMaterialKeywords(Material material)
    {
        SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap"));
    }
    static void SetKeyword(Material m, string keyword, bool state)
    {
        if (state)
            m.EnableKeyword(keyword);
        else
            m.DisableKeyword(keyword);
    }

    void makeAlwaysPositiv(MaterialProperty materialProperty)
    {
        materialProperty.floatValue = Mathf.Max(materialProperty.floatValue, 0);
    }

}

