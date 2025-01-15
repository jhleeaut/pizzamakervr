using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class TeleportButtonAnim : StateMachineBehaviour
    {
        int count;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (count != 0)
            {
                if (animator.name.Equals("VRButtonCooking"))
                {
#if UNITY_EDITOR
                    Debug.Log("애니메이션 재생");
#endif
                    GameObject.Find("Pizza Tray_01").SendMessage("Cooking", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("애니메이션 재생");
#endif
                    TeleportManager.Instance.Teleport(animator.GetComponentInChildren<TeleportButton>().teleportName);
                }
            }
            count++;
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