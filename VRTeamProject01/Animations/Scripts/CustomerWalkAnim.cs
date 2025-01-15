using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{

    public class CustomerWalkAnim : StateMachineBehaviour
    {
        public MoveCtrl moveCtrl;
        int count;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (moveCtrl == null)
            {
                moveCtrl = animator.transform.parent.GetComponent<MoveCtrl>();
            }
            if (moveCtrl == true)
            {
                //피자를 얻었을때만 테이블과 매장밖 WayPointGroup중 고름
                if (moveCtrl.IsGetPizza)
                {
                    Debug.Log(count++);
                    moveCtrl.IsGetPizza = false;
                    moveCtrl.OrderEnd();
                }
                moveCtrl.IsWalking = true;
            }

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    }
}