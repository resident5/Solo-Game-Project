using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        //animator.GetComponent<NewCharacters>().attack = false;

        //animator.GetComponent<NewCharacters> ().attack = true;

        //animator.SetFloat ("speed", 0f);

        //if (animator.gameObject.tag == "Player")
        //{
        //	if (NewPlayer.Instance.OnGround)
        //	{
        //		NewPlayer.Instance.myRigidbody.velocity = Vector2.zero;
        //	}
        //}
    }

    //	 OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //	}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<NewCharacters> ().attack = false;
		//animator.GetComponent<NewCharacters> ().Attack ();
		animator.GetComponent<NewCharacters> ().attackCollider.enabled = false;
		//animator.ResetTrigger (NewPlayer.Instance.currentAttack.getTriggerName() + NewPlayer.Instance.currentAttackIndex);

		
	}
	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
