﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(PlayersPositionManager))]
public class PlayersPositionManagerEditor : Editor
{

    private PlayersPositionManager playersPositionManager;

    private SerializedProperty playableCharactersProperty;

    private ReorderableList playableCharactersList;

    private void OnEnable()
    {

        playersPositionManager = (PlayersPositionManager)target;

        playableCharactersProperty = serializedObject.FindProperty(nameof(PlayersPositionManager.playableCharacters));

        playableCharactersList = new ReorderableList(serializedObject, playableCharactersProperty)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = false,
            drawHeaderCallback = rect => {
                //GUI.color = Color.cyan;
                //GUI.backgroundColor = Color.red;
                EditorGUI.LabelField(rect, playableCharactersProperty.displayName, EditorStyles.boldLabel);

            },
            drawElementCallback = (rect, index, focused, active) =>
            {

                var element = playableCharactersProperty.GetArrayElementAtIndex(index);

                var backgroundColor = GUI.backgroundColor;
                int count = 0;
                for (int i = 0; i < playableCharactersProperty.arraySize; i++)
                {
                    if (playableCharactersProperty.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                    {
                        count++;
                    }
                }
                if (element.objectReferenceValue==null || count>1)
                {
                    GUI.backgroundColor = Color.red;
                } else
                {
                    GUI.backgroundColor = backgroundColor;
                }
                //GUI.color = Color.cyan;
                GUIStyle insertVarNameHere = new GUIStyle();
                insertVarNameHere.fontStyle = FontStyle.Bold;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element, new GUIContent("Character " + (index + 1) + ": "));

                GUI.backgroundColor = backgroundColor;


                if (element.objectReferenceValue == null)
                {
                    rect.y += EditorGUI.GetPropertyHeight(element);
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Character may not be empty!", MessageType.Error);
                }
                else if (count > 1)
                {
                    rect.y += EditorGUI.GetPropertyHeight(element);
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Duplicate! Character has to be unique!", MessageType.Error);
                }
            },

            elementHeightCallback = index =>
            {
                var element = playableCharactersProperty.GetArrayElementAtIndex(index);
                int count = 0;
                for (int i = 0; i < playableCharactersProperty.arraySize; i++)
                {
                    if (playableCharactersProperty.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                    {
                        count++;
                    }
                }
                var height = EditorGUI.GetPropertyHeight(element);
                if (element.objectReferenceValue == null||count>1)
                {
                    height += EditorGUIUtility.singleLineHeight;
                }
                return height;
            },

            onAddCallback = list =>
            {
                list.serializedProperty.arraySize++;

                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                newElement.objectReferenceValue = null;
            },
            onRemoveCallback = list =>
            {
                List<GameObject> temp = new List<GameObject>();
                for (int i = 0; i < list.serializedProperty.arraySize; i++)
                {
                    temp.Add((GameObject)list.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue);
                }
                list.serializedProperty.arraySize--;
                temp.RemoveAt(list.index);
                for (int i = 0; i < temp.Count; i++)
                {
                    var d = list.serializedProperty.GetArrayElementAtIndex(i);
                    d.objectReferenceValue = temp[i];
                }
                //ReorderableList.defaultBehaviours.DoRemoveButton(list);

            }

        };

    }
    public override void OnInspectorGUI()
    {
        //base.DrawDefaultInspector();

        serializedObject.Update();


        //Rect screenRect = GUILayoutUtility.GetRect(1, 1);
        //Rect vertRect = EditorGUILayout.BeginVertical();
        //EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 17, vertRect.height + 9), new Color(0.4f, 0.4f, 0.4f));
        //Sprite test = Resources.Load<Sprite>("logo-with-outline");
        //GUIStyle headStyle = new GUIStyle();
        //headStyle.normal.textColor = Color.white;
        //headStyle.fontSize = 13;
        //headStyle.alignment = TextAnchor.MiddleCenter;
        //headStyle.fontStyle = FontStyle.Italic;

        //GUILayout.Label("See-through Shader", headStyle);
        //headStyle.fontStyle = FontStyle.Bold;
        //headStyle.fontSize = 14;
        //GUILayout.Label("Playable Characters Position Manager", headStyle);

        //GUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace();
        //GUILayout.Label(test.texture, GUILayout.Width(150), GUILayout.Height(150));
        //GUILayout.FlexibleSpace();
        //GUILayout.EndHorizontal();

        SeeThroughShaderEditorUtils.usualStart("Playable Characters Position Manager");


        Color c = new Color(1,1,1);
        GUI.backgroundColor = c;
        playableCharactersList.DoLayoutList();
        //EditorGUILayout.EndVertical();
        SeeThroughShaderEditorUtils.usualEnd();
        serializedObject.ApplyModifiedProperties();
    }
}