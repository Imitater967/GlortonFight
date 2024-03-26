using UnityEngine;

namespace Script.FX
{
    public class S_FX_AutoDestroy : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            Destroy(animator.transform.parent.gameObject);
        }
 
    }
}