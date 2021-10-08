namespace Runemark.Common.Gameplay
{
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    /// <summary>
    /// Base transformer class.
    /// </summary>
    public enum LoopType
    {
        Once,
        PingPong,
        Repeat
    }
    public abstract class RMTransformer : RMFunction
    {
        public bool ActivateOnStart;
        public LoopType loopType;
        public float Duration = 1f;
        public AnimationCurve AccelerationCurve = new AnimationCurve(new Keyframe(0,0), new Keyframe(1,1));
        
        float _time = 0f;
        public float Position = 0f;
        float _direction = 1f;

        bool _active;

        public override void Activate()
        {
#if UNITY_EDITOR
            var flags = GameObjectUtility.GetStaticEditorFlags(gameObject);
            if ((flags & StaticEditorFlags.BatchingStatic) == StaticEditorFlags.BatchingStatic)
            {
                Debug.LogErrorFormat("The {0} on the {1} wants to control a gameObject that is not Static. Please turn Static Batching off.",
                    GetType().Name, transform.name);
            }
#endif
            _active = true;
            InProgress = true;
        }

        public override void Deactivate()
        {         
            _active = false;
            InProgress = false;
        }

        private void Start()
        {
            if (!ActivateOnStart) return;
            Activate();
        }

        void Update()
        {
            if (_active)
            {
                _time += _direction * Time.deltaTime / Duration;
                switch (loopType)
                {
                    case LoopType.Once:
                        Position = Mathf.Clamp01(_time);
                        if(_direction > 0 && Position >= 1 || _direction < 0 && Position <= 0)
                        {
                            Deactivate();
                            _direction *= -1;
                        }   

                        break;
                    case LoopType.PingPong:
                        Position = Mathf.PingPong(_time, 1f);
                        break;
                    case LoopType.Repeat:
                        Position = Mathf.Repeat(_time, 1f);
                        break;
                }
                DoTransform(Position);
            }
        }
        
        public abstract void DoTransform(float position);



#if UNITY_EDITOR
        [Range(0,1)]
        public float PreviewPosition;
        protected Vector3 _cachedPosition;
        public void CatchPosition()
        {
            _cachedPosition = transform.position;
        }
#endif
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(RMTransformer), true)]
    public class RMTransformerEditor : CustomInspectorBase
    {
        RMTransformer myTarget;

        protected override void OnInit()
        {
            base.OnInit();

            myTarget = target as RMTransformer;
            myTarget.CatchPosition();
        }
        protected override void OnChanged()
        {
            myTarget.DoTransform(myTarget.PreviewPosition);
        }
    }
    #endif
}
