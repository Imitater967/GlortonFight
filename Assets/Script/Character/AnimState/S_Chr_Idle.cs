using UnityEngine;

namespace Script.Character.State
{
    public class S_Chr_Idle : StateMachineBehaviour
    {
        
        private GlortonFighter _fighter;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {   _fighter = animator.GetComponent<GlortonFighter>();            
              
        }
 
    }
}