using System.Collections;
using System.Collections.Generic;
using Script.Character;
using UnityEngine;

public class S_Chr_FlyAlignFace : StateMachineBehaviour
{
    private GlortonFighter fighter;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        fighter = animator.GetComponent<GlortonFighter>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fighter.rb.velocity.x > 0)
        {
            fighter.animation.AlignFace(false);
        }
        else
        {
            fighter.animation.AlignFace(true);
        }
    }
 
}
