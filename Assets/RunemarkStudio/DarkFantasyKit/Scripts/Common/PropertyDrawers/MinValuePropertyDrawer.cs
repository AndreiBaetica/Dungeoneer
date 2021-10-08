namespace Runemark.Common
{
    using UnityEngine;
    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class MinAttribute : PropertyAttribute
    {
        public bool isInt;
        public int minValueInt;
        public float minValueFloat;

        public MinAttribute(int min)
        {
            minValueInt = min;
            isInt = true;
        }
        
        public MinAttribute(float min)
        {
            minValueFloat = min;
            isInt = false;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MinAttribute))]
    public class MinDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinAttribute min = attribute as MinAttribute;

            if (min.isInt)
            {
                property.intValue = EditorGUI.IntField(position, label, Mathf.Max(property.intValue, min.minValueInt));
            }
            else
            {
                property.floatValue = EditorGUI.FloatField(position, label, Mathf.Max(property.floatValue, min.minValueFloat));
            }
        }
    }
#endif

}
