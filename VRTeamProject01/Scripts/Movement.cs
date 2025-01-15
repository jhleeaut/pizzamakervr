using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    [RequireComponent(typeof(CharacterController))]

    public class Movement : MonoBehaviour
    {
        #region

        // Components
        private CharacterController controller;

        //Move
        [SerializeField] private float moveSpeed = 2;

        public float MoveSpeed
        {
            set { moveSpeed = value; }
            get { return moveSpeed; }
        }

        private Vector3 moveForce;

        //Jump & Gravity
        [SerializeField] private float jumpForce = 8;

        [SerializeField] private float gravity = -20;
        #endregion

        public void Init()
        {
            controller = this.GetComponent<CharacterController>();
        }

        public void UpdateMove(float horizontal, float vertical)
        {
            Vector3 direction = transform.rotation * new Vector3(horizontal, 0, vertical);

            moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);

            if (!controller.isGrounded)
            {
                moveForce.y += gravity * Time.deltaTime;
            }

            controller.Move(moveForce * Time.deltaTime);
        }

        public bool Jump()
        {
            if (controller.isGrounded)
            {
                moveForce.y = jumpForce;

                return true;
            }
            return false;
        }
    }
}