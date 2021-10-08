namespace Runemark.Common.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// Base function class.
    /// </summary>
    public class RMFunction : MonoBehaviour
    {
        public virtual bool InProgress { get { return _inProgress; } set { _inProgress = value; } }
        bool _inProgress;

        public virtual void Activate() { }
        public virtual void Deactivate() { }        
    }
}