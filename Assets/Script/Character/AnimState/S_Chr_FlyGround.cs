using UnityEngine;

namespace Script.Character.State
{
    public class S_Chr_FlyGround : StateMachineBehaviour
    {
 
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            var glortonFighter = animator.GetComponent<GlortonFighter>();
            glortonFighter.animation.StopFly();
            glortonFighter.input.UnblockInput();
        }

 
    }
}