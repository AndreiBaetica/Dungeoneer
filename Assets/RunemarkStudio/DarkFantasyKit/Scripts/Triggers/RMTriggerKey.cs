namespace Runemark.Common.Gameplay
{
    using UnityEngine;
    using UnityEngine.Events;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    [AddComponentMenu("Dark Fantasy Kit/Triggers/Trigger with Key Press")]
    public class RMTriggerKey : RMTrigger
    {
      

        [Header("Key Press")]
        public KeyCode Key;
        public UnityEvent OnKeyPressed;


        void OnTriggerStay(Collider other)
        {
            if (Validate(other) && Input.GetKeyUp(Key))
            {
                OnKeyPressed.Invoke();
            }
        }
    }

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMTriggerKey), true)]
    public class RMTriggerKeyEditor : RMTRiggerEditor
    {
        protected override string Title { get { return "Key Trigger"; } }
        protected override string Description { get { return "If the player presses the given key the OnKeyPressed event invokes. The player has to stay in the trigger area."; } }
    }
    #endif
}