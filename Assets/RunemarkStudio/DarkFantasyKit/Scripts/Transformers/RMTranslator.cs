namespace Runemark.Common.Gameplay
{
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    /// <summary>
    /// This component translates the target transform from the start position to the end position.
    /// </summary>   
    [AddComponentMenu("Dark Fantasy Kit/Translator")]
    public class RMTranslator : RMTransformer
    {     
        public Transform Target;
        public Vector3 Start = Vector3.zero;
        public Vector3 End = Vector3.zero;

        public override void DoTransform(float position)
        {
            float curvePosition = AccelerationCurve.Evaluate(position);
            Target.localPosition = Vector3.Lerp(Start, End, curvePosition);
        }
    }

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMTranslator), true)]
    public class RMTranslatorEditor : RMTransformerEditor
    {
        protected override string Title { get { return "Translator"; } }
        protected override string Description
        {
            get
            {
                return "This component translates the target transform from the start position to the end position. \n"+
                    "You can activate this component by calling the Activate method from script.";
            }
        }
    
        void OnSceneGUI()
        {
            var t = target as RMTranslator;
            
            var start = t.transform.TransformPoint(t.Start);
            var end = t.transform.TransformPoint(t.End);

            using (var cc = new EditorGUI.ChangeCheckScope())
            {
                start = Handles.PositionHandle(start, Quaternion.AngleAxis(180, t.transform.up) * t.transform.rotation);
                Handles.Label(start, "Start", "button");
                Handles.Label(end, "End", "button");
                end = Handles.PositionHandle(end, t.transform.rotation);
                if (cc.changed)
                {
                    Undo.RecordObject(t, "Move Handles");
                    t.Start = t.transform.InverseTransformPoint(start);
                    t.End = t.transform.InverseTransformPoint(end);
                    t.DoTransform(t.PreviewPosition);
                }
            }
            Handles.color = Color.yellow;
            Handles.DrawDottedLine(start, end, 5);
            Handles.Label(Vector3.Lerp(start, end, 0.5f), "Distance:" + (end - start).magnitude);
        }
    }
    #endif
}


