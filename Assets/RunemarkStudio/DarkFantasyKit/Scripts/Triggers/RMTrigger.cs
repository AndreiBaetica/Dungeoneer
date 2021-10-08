namespace Runemark.Common.Gameplay
{
    using UnityEngine;
    using UnityEngine.Events;
    #if UNITY_EDITOR
    using UnityEditor;
    #endif


    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("Dark Fantasy Kit/Triggers/Simple Trigger")]
    public class RMTrigger : MonoBehaviour
    {
      
        [TagField] public string Tag;

        [Header("Trigger Enter & Exit")]
        public UnityEvent OnEnter;
        public UnityEvent OnExit;        

        void OnTriggerEnter(Collider other)
        {
            if (Validate(other))
            {
                OnEnter.Invoke();
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (Validate(other))
            {
                OnExit.Invoke();
            }
        }
        protected bool Validate(Collider other)
        {
            return other.tag == Tag;
        }
    }

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMTrigger), true)]
    public class RMTRiggerEditor : CustomInspectorBase
    {
        protected override string Title { get { return "Simple Trigger"; } }
        protected override string Description { get { return "You can create simple OnTriggerEnter and OnTriggerExit events."; } }
    }
    #endif
}
