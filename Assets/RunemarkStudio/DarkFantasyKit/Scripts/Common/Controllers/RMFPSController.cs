namespace Runemark.Common.Gameplay
{
    using UnityEngine;
    
    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class RMFPSController : MonoBehaviour
    {       
        public float mouseSensitivityX = 1.0f;
        public float mouseSensitivityY = 1.0f;
        public float walkSpeed = 10.0f;

        public float JumpForce = 250.0f;
        public LayerMask GroundedMask;

        Vector3 _moveAmount;
        Vector3 _smoothMoveVelocity;

        Transform _cameraTransform;
        float _verticalLookRotation;

        Rigidbody _rigidbody;

        
        bool _grounded;
        bool _cursorVisible;

        // Use this for initialization
        void Start()
        {
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            _rigidbody = GetComponent<Rigidbody>();
            LockMouse();
        }

        // Update is called once per frame
        void Update()
        {
            // rotation
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
            _verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -60, 60);
            _cameraTransform.localEulerAngles = Vector3.left * _verticalLookRotation;

            // movement
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 targetMoveAmount = moveDir * walkSpeed;
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetMoveAmount, ref _smoothMoveVelocity, .15f);

            // jump
            if (Input.GetButtonDown("Jump"))
            {
                if (_grounded)
                {
                    _rigidbody.AddForce(transform.up * JumpForce);
                }
            }

            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1 + .1f, GroundedMask))
            {
                _grounded = true;
            }
            else
            {
                _grounded = false;
            }

            /* Lock/unlock mouse on click */
            if (Input.GetMouseButtonUp(0))
            {
                if (!_cursorVisible)
                {
                    UnlockMouse();
                }
                else
                {
                    LockMouse();
                }
            }
        }

        void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
        }

        void UnlockMouse()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cursorVisible = true;
        }

        void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cursorVisible = false;
        }
    }

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RMFPSController), true)]
    public class RMFPSControllerEditor : CustomInspectorBase
    {   
        protected override string Title { get { return "First Person Controller"; } }
        protected override string Description { get { return "This is a simple FPS controller."; } }
    }
    #endif
}
