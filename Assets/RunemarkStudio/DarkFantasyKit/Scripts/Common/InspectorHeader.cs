namespace Runemark.Common
{
    #if UNITY_EDITOR
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class InspectorHeader 
    {
        GUIStyle background;
        string title;

        public InspectorHeader(string title)
        {
            this.title = title;

            Texture2D bg = Resources.Load<Texture2D>("InspectorHeader");  
            background = new GUIStyle();
            background.padding = new RectOffset(15, 3, 3, 3);
            background.normal.background = bg; 


            Font lFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            if (lFont == null) { lFont = EditorStyles.standardFont; }

            background.font = lFont;
            background.fontSize = 18;
            background.fontStyle = FontStyle.Bold;
            background.normal.textColor = new Color(0.85f, 0.85f, 0.85f, 1.00f); 
            background.fixedHeight = 50f;
            background.alignment = TextAnchor.MiddleLeft;     
        }
        
        public void Draw()
        {
            GUILayout.Box(title, background);   
        }
    
    }
    #endif
}
