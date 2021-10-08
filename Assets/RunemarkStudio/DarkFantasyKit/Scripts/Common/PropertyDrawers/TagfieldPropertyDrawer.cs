namespace Runemark.Common
{
    using UnityEngine;
    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class TagFieldAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TagFieldAttribute))]
    public class TagFieldDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
#endif

}
