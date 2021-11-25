using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SeeThroughShaderEditorUtils
{
    public static void usualStart(string name)
    {

        Rect screenRect = GUILayoutUtility.GetRect(1, 1);
        Rect vertRect = EditorGUILayout.BeginVertical();
        Color backgroundColor = new Color(0.4f, 0.4f, 0.4f);
        EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 17, vertRect.height + 9), backgroundColor);
        Sprite test = Resources.Load<Sprite>("logo-with-outline");
        if(test == null )
        {
            test = (Sprite)AssetDatabase.LoadAssetAtPath("Packages/See-through Shader/Core/Icon/logo-with-outline.png", typeof(Sprite));
        }

        if (test == null)
        {
            test = (Sprite)AssetDatabase.LoadAssetAtPath("Packages/See-through Shader/Core/Icon/logo-with-outline", typeof(Sprite));
        }

        if (test == null)
        {
            test = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Core/Icon/logo-with-outline", typeof(Sprite));
        }

        GUIStyle headStyle = new GUIStyle();
        headStyle.normal.textColor = Color.white;
        headStyle.fontSize = 13;
        headStyle.alignment = TextAnchor.MiddleCenter;
        headStyle.fontStyle = FontStyle.Italic;


        GUILayout.Label("See-through Shader", headStyle);
        headStyle.fontStyle = FontStyle.Bold;
        headStyle.fontSize = 14;
        GUILayout.Label(name, headStyle);

        if (test != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(test.texture, GUILayout.Width(150), GUILayout.Height(150));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        //EditorGUILayout.Space(30);

        Rect rect = EditorGUILayout.BeginVertical();
        GUI.Box(rect, GUIContent.none);
    }

    public static void usualEnd()
    {
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }


    public static void DrawUILine(int thickness = 1, int padding = 10)
    {
        Color color = Color.black;
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        EditorGUI.DrawRect(r, color);
    }

    public static void DrawUILineGray(int thickness = 2, int padding = 10)
    {
        Color color = new Color(0.2f, 0.2f, 0.2f, 1);
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        EditorGUI.DrawRect(r, color);
    }



    public static void DrawUILineSubMenu(int thickness = 2, int padding = 1)
    {
        Color color = new Color(0.28f, 0.28f, 0.28f, 1);

        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        //r.y += padding / 2;
        float marginLeft = 20;
        r.width = r.width - marginLeft;
        r.x += marginLeft;

        EditorGUI.DrawRect(r, color);
    }


    public static void makeHorizontalSeparation()
    {
        GUIStyle horizontalLine;
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 10;

        var c = GUI.color;
        GUI.color = new Color(0.4f, 0.4f, 0.4f);
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = c;
    }


}
