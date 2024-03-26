using UnityEngine;

namespace Script.Character.State
{
    public class S_Chr_RangedAttack : StateMachineBehaviour
    {
        private GlortonFighter _fighter;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.transform.rotation=Quaternion.identity;
            _fighter = animator.GetComponent<GlortonFighter>();
            _fighter.input.BlockInput(1);
            _fighter.motion.StartSpecialAttack();

            //奇怪的是，进入specialattack的时候，input仍然在更新，导致animation进入jump
            //会在极短的时间按下W+J的时候出现，W释放不完全
            //故添加此变量
            // _fighter.animation.StartSpecialAttack();
            // Debug.LogWarning("State Enter");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.GetComponent<GlortonFighter>().motion.StopSpecialAttack();
            _fighter.input.UnblockInput();         

            // _fighter.animation.StopSpecialAttack();
            // Debug.LogWarning("State Exit");

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}