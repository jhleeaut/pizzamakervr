using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    public class PizzaBoxAnim : StateMachineBehaviour {
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        PizzaBoxHandler pizzaBoxHandler;
        [SerializeField]private Grabbable refGrabbable;
        public Grabbable RefGrabbable
        {
            set
            {
                refGrabbable = value;
            }
        }

        // 
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (pizzaBoxHandler == null)
            {
                pizzaBoxHandler = animator.GetComponent<PizzaBoxHandler>();
            }
            if (refGrabbable != null)
            {
                AudioManager.Instance.PlayEffect("효과음", 1.0f);
                CookingPizza cookingPizza = refGrabbable.GetComponent<CookingPizza>();
                if (cookingPizza != null)
                {
                    pizzaBoxHandler.OrderPacking(cookingPizza);
                    refGrabbable = null;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("피자가 아닙니다.");
#endif
                    animator.SetBool("isCover", false);
                    Destroy(refGrabbable.gameObject);
                    refGrabbable = null;
                }
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
