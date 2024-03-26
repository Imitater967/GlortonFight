using UnityEngine;

namespace Script.Character.State
{
    public class S_Chr_Crouch : StateMachineBehaviour
    {
        protected GlortonFighter _fighter;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _fighter = animator.GetComponent<GlortonFighter>();
            _fighter.motion.EnterCrouch();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _fighter.motion.ExitCrouch();
        }
    }
}