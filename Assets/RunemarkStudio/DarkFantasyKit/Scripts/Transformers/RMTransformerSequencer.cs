namespace Runemark.Common.Gameplay
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    using UnityEditorInternal;
#endif

    /// <summary>
    /// This component iterates through the given functions and activates them one by one.
    /// Only activates the next one if the previous one is finished.
    /// </summary>
    [AddComponentMenu("Dark Fantasy Kit/Sequencer")]
    public class RMTransformerSequencer : MonoBehaviour
    {
        [Tooltip("If this is enabled, the sequencer will start activating the transformers from back, in every odd activation time.")]
        public bool InverseRepeate;
        
        public List<RMFunction> List;

        public bool InProgress;

        int _index = 0;
        int _direction = 1;

        Coroutine _current;

        public void Activate()
        {
            if (InProgress) return;

            InProgress = true;
            _current = StartCoroutine(ActivateTransformer());
        }
                

        IEnumerator ActivateTransformer()
        {
            RMFunction current = List[_index];
            current.Activate();
            while (current.InProgress) yield return null;

            _index += _direction;

            if (_index < List.Count && _index >= 0)
            {                
                _current = StartCoroutine(ActivateTransformer());
            }
            else
            {
                _index = Mathf.Clamp(_index, 0, List.Count-1);

                InProgress = false;

                if (InverseRepeate) _direction *= -1;
                else _index = 0;               
            }
        }         
    }

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMTransformerSequencer), true)]
    public class RMTransformerSequencerEditor : CustomInspectorBase
    {
        protected override string Title { get { return "Sequencer"; } }
        protected override string Description
        {
            get
            {
                return "This component iterates through the given functions and activates them one by one. Only activates the next one if the previous one is finished.";
            }
        }
    

        ReorderableList _list;

        protected override void OnInit()
        {
            base.OnInit();

            var property = FindProperty("List");
            _list = new ReorderableList(serializedObject, property, true, true, true, true);
            _list.drawHeaderCallback = rect => 
            {
                EditorGUI.LabelField(rect, "Sequence", EditorStyles.boldLabel);
            };

            _list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
            {
                var element = _list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);                   
            };
            
            AddProperty("InverseRepeate");
            AddCustomField("List", ()=> { _list.DoLayoutList(); });            
        }    
    }
    #endif

}
