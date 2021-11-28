using UnityEngine;

namespace OneEyedJoe.UI
{
    public class AnimateWindow : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Show = Animator.StringToHash("Show"); 
        private static readonly int Hide = Animator.StringToHash("Hide"); 
        private static readonly int Hidden = Animator.StringToHash("Hidden"); 
        
        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            
            _animator.SetTrigger(Show);
        }

        public void Close()
        {
            _animator.SetTrigger(Hide);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}
