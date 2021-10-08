namespace Runemark.Common
{
    using System;
#if UNITY_EDITOR
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;


    public class CustomInspectorBase : Editor
    {
        #region Auto Inspector Builder
        protected delegate void CustomFieldDelegate();
        protected delegate bool FieldFilterDelegate();

        class PropertyData
        {
            public GUIContent label;
            public SerializedProperty property;

            public CustomFieldDelegate customField;
            public bool useCustomField;

            public string key;
            public int group = 0;
            public int index = 0;

            public FieldFilterDelegate filter;
        }
        List<PropertyData> properties = new List<PropertyData>();
        int minGroup = 1000, maxGroup = 0;

        List<string> existingNames = new List<string>();
        Dictionary<int, int> lastIndexByGroup = new Dictionary<int, int>();

        protected void AddProperty(string name, GUIContent label = null, int group = 0)
        {
            if (existingNames.Contains(name)) return;
            if (!lastIndexByGroup.ContainsKey(group))
            {
                lastIndexByGroup.Add(group, 0);
                if (group > maxGroup) maxGroup = group;
                if (group < minGroup) minGroup = group;
            }

            int index = lastIndexByGroup[group]++;
            var property = FindProperty(name);
            properties.Add(new PropertyData() { key = name, label = label, index = index, group = group, property = property });
            existingNames.Add(name);
        }
        protected void AddCustomField(string name, CustomFieldDelegate customField, int group = 0)
        {
            if (existingNames.Contains(name)) return;
            if (!lastIndexByGroup.ContainsKey(group))
            {
                lastIndexByGroup.Add(group, 0);
                if (group > maxGroup) maxGroup = group;
                if (group < minGroup) minGroup = group;
            }

            int index = lastIndexByGroup[group]++;
            properties.Add(new PropertyData()
            {
                key = name,
                index = index,
                group = group,
                useCustomField = true,
                customField = customField
            });
            existingNames.Add(name);
        }
        protected void AddHeader(string title, int group = 0)
        {
            if (existingNames.Contains(title)) return;
            if (!lastIndexByGroup.ContainsKey(group))
            {
                lastIndexByGroup.Add(group, 0);
                if (group > maxGroup) maxGroup = group;
                if (group < minGroup) minGroup = group;
            }
     
            properties.Add(new PropertyData()
            {
                key = title,
                index = -1,
                group = group,
                useCustomField = true,            
                customField = () => { GUILayout.Label(title, EditorStyles.boldLabel); }
            });
            existingNames.Add(name);
        }
        protected void AddSpace(float pixels, int group = 0)
        {            
            if (!lastIndexByGroup.ContainsKey(group))
            {
                lastIndexByGroup.Add(group, 0);
                if (group > maxGroup) maxGroup = group;
                if (group < minGroup) minGroup = group;
            }

            int index = lastIndexByGroup[group]++;
            properties.Add(new PropertyData()
            {
                key = "Space",
                index = index,
                group = group,
                useCustomField = true,
                customField = () => { GUILayout.Space(pixels); }
            });
            existingNames.Add(name);
        }

        protected void AddFilter(string name, FieldFilterDelegate filter)
        {
            foreach (var p in properties)
            {
                if (p.key == name)
                {
                    p.filter = filter;
                }
            }
        }

        protected SerializedProperty FindProperty(string name)
        {
            if (!name.Contains("."))
            {
                return serializedObject.FindProperty(name);
            }
            else
            {
                string[] names = name.Split('.');
                SerializedProperty prop = serializedObject.FindProperty(names[0]);
                for (int i = 1; i < names.Length; i++)
                {
                    var tempProp = prop.FindPropertyRelative(names[i]);
                    if (tempProp != null) prop = tempProp;
                }
                return prop;
            }
        }
        protected void ReorderProperties()
        {
            properties = properties.OrderBy(x => x.group).ThenBy(x => x.index).ToList();
        }

        void DrawPropertyField(PropertyData prop)
        {
            if (prop.filter != null && !prop.filter()) return;

            if (prop.useCustomField)
                prop.customField();
            else if(prop.label == null)
                EditorGUILayout.PropertyField(prop.property, true);
            else
                EditorGUILayout.PropertyField(prop.property, prop.label, true);
        }    
       
        #endregion

        #region Inspector Functions
        protected virtual string Title { get { return target.GetType().Name ; } }
        protected virtual string Description { get { return ""; } }

        InspectorHeader header;

        void OnEnable()
        {
            OnInit();
            ReorderProperties();   
            header = new InspectorHeader(Title);            
        }

        public sealed override void OnInspectorGUI()
        {
            using (var cc = new EditorGUI.ChangeCheckScope())
            {
                serializedObject.Update();
                header.Draw();

                if(Description != "")
                    EditorGUILayout.HelpBox(Description, MessageType.Info);

                if (properties.Count == 0)
                {
                    DrawDefaultInspector();
                    return;
                }

                foreach(var property in properties)
                {
                    DrawPropertyField(property);
                }
                serializedObject.ApplyModifiedProperties();

                if (cc.changed)
                {                    
                    OnChanged();
                }
            }
        }
        protected virtual void OnInit(){}
        protected virtual void OnChanged(){}


        #endregion
    }

    #endif
}