namespace Runemark.Common.Gameplay
{
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif


    /// <summary>
    /// This component rotates its transform.
    /// </summary>
    [AddComponentMenu("Dark Fantasy Kit/Rotator")]
    public class RMRotator : RMTransformer
    {        
        public Vector3 axis = Vector3.up;
        public float start = 0;
        public float end = 90;

        public override void DoTransform(float position)
        {
            var curvePosition = AccelerationCurve.Evaluate(position);
                       
            var q = Quaternion.AngleAxis(Mathf.Lerp(start, end, curvePosition), axis);
            transform.localRotation = q;
        }
    }

     #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMRotator), true)]
    public class RMRotatorEditor : CustomInspectorBase
    {
        protected override string Title { get { return "Rotator"; } }
        protected override string Description { get { return "This component rotates the transform.\nYou can activate this component by calling the Activate method from script."; } }
    }
    #endif
}
