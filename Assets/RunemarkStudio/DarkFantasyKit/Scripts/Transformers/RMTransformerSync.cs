namespace Runemark.Common.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;
    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    /// <summary>
    /// This component syncronizea the activation of multiple Transfromer functionality.
    /// </summary>
    [AddComponentMenu("Dark Fantasy Kit/Syncronizer")]
    public class RMTransformerSync : RMFunction
    {     
        public List<RMTransformer> List;

        public override bool InProgress
        {
            get
            {
                return List.Find(x => x.InProgress) != null;                   
            }
        }      
        public override void Activate()
        {
            foreach (var t in List) t.Activate();
        }
        public override void Deactivate()
        {
            foreach (var t in List) t.Deactivate();
        }
    }

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMTransformerSync), true)]
    public class RMTransformerSyncEditor : CustomInspectorBase
    {
        protected override string Title { get { return "Syncronizer"; } }
        protected override string Description
        {
            get
            {
                return "This component syncronizea the activation of multiple Transfromer functionality. \n" +
                    "Just drop the Transformers you want to simultaneously activate, and call the Activate method of this component.";
            }
        }
    }
    #endif
}
