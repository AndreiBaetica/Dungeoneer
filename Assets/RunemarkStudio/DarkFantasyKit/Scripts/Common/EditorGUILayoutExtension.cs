namespace Runemark.Common
{
    #if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using System.Collections.Generic;

    public class EditorGUILayoutExtension
    {
        public static void TagMask(GUIContent label, ref string[] selectedTags)
        {
            string[] gameTags = InternalEditorUtility.tags;
            int mask = 0;
         
            for (int i = 0; i < selectedTags.Length; i++)
            {
                for (int cnt = 0; cnt < gameTags.Length; cnt++)
                {
                    if (gameTags[cnt] == selectedTags[i])
                        mask |= 1 << cnt;
                }
            }

            int tempMask = EditorGUILayout.MaskField(label, mask, gameTags);
            
            if (tempMask != mask)
            {
                mask = tempMask;
                List<string> tags = new List<string>();
                for (int i = 0; i < gameTags.Length; i++)
                {
                    int layer = 1 << i;
                    if ((mask & layer) != 0)
                    {
                        tags.Add(gameTags[i]);
                    }
                }
                selectedTags = tags.ToArray();

            }
        }
    }

    #endif
}