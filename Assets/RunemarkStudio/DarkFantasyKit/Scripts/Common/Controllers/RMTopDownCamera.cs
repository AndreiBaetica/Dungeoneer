namespace Runemark.Common.Gameplay
{
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class RMTopDownCamera : MonoBehaviour
    {     
        public float height = 20;
        public float distance = 15;
        public Transform target;
        Camera _cam;

        private void OnEnable()
        {
            _cam = GetComponentInChildren<Camera>();
        }

        void Update()
        {
            Vector3 pos = target.position;
            pos.y += height;
            pos.z -= distance;

            transform.position = pos;
            _cam.transform.LookAt(target.position);
        }
    }

      #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMTopDownCamera), true)]
    public class RMTopDownCameraEditor : CustomInspectorBase
    {   
        protected override string Title { get { return "Top-Down Camera Controller"; } }
        protected override string Description { get { return "Simple camera controller for top-down demo scenes."; } }
    }
    #endif
}
