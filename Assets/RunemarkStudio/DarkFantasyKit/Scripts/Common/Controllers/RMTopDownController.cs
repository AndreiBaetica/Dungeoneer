namespace Runemark.Common.Gameplay
{
    using UnityEngine;
    using UnityEngine.AI;
    
    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class RMTopDownController : MonoBehaviour
    {
        Animator _animator;
        NavMeshAgent _agent;

        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator != null) _animator.applyRootMotion = false;
            _agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    _agent.SetDestination(hit.point);
                }
            }

            if(_animator !=null)
                _animator.SetFloat("Speed", _agent.velocity.sqrMagnitude);
        }
    }

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMTopDownController), true)]
    public class RMTopDownControllerEditor : CustomInspectorBase
    {           
        protected override string Title { get { return "Top-Down Character Controller"; } }
        protected override string Description { get { return "Simple character controller for top-down demo scenes."; } }

    }
    #endif
}