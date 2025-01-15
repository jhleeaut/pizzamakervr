using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    public class PlayerController : MonoBehaviour
    {
        #region Component
        private RotateToMouse rotateToMouse;

        private Movement movement;
        private Status status;
        //private WeaponAssaultRifle weapon;
        private RangeView rangeView;
        private GrabberPoint[] grabberPoints;
        #endregion Component
        #region Var

        public float GrabDistance = 0.0f;
        /// <summary>
        /// 키보드를 사용할 때의 회전 속도.
        /// </summary>
        public float RotationRatchet = 45.0f;
        private bool isPossibleRotate;

        //Input Key
        private KeyCode keyCodeRun = KeyCode.LeftShift;
        private KeyCode keyCodeJump = KeyCode.Space;
        private KeyCode keyCodeReload = KeyCode.R;
        #endregion Var

        public void Init()
        {
            rotateToMouse = this.GetComponent<RotateToMouse>();
            movement = this.GetComponent<Movement>();
            movement.Init();
            status = this.GetComponent<Status>();
            status.Init();
            //weapon = this.GetComponentInChildren<WeaponAssaultRifle>();
            rangeView = this.GetComponent<RangeView>();
            grabberPoints = GetComponentsInChildren<GrabberPoint>();
            foreach (GrabberPoint grabberPoint in grabberPoints)
            {
                Transform grabberPointTransform = grabberPoint.transform;
                switch (grabberPoint.name)
                {
                    case "GrabberPointForward":
                        grabberPointTransform.localPosition = Vector3.forward * GrabDistance;
                        grabberPointTransform.localRotation = Quaternion.identity;
                        break;
                    case "GrabberPointBack":
                        grabberPointTransform.localPosition = Vector3.back * GrabDistance;
                        grabberPointTransform.localRotation = Quaternion.identity;
                        break;
                    case "GrabberPointUp":
                        grabberPointTransform.localPosition = Vector3.up * GrabDistance;
                        grabberPointTransform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                        break;
                    case "GrabberPointDown":
                        grabberPointTransform.localPosition = Vector3.down * GrabDistance;
                        grabberPointTransform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                        break;
                    case "GrabberPointLeft":
                        grabberPointTransform.localPosition = Vector3.left * GrabDistance;
                        grabberPointTransform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                        break;
                    case "GrabberPointRight":
                        grabberPointTransform.localPosition = Vector3.right * GrabDistance;
                        grabberPointTransform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                        break;
                }
                grabberPointTransform.localScale = new Vector3(GrabDistance * 2.0f, GrabDistance * 2.0f, .1f);
            }
        }

        public void Updated()
        {
            if (!GetKeyEvent.isPossibleGrabbableRotate)
            {
                UpdateRotate();
            }
            UpdateMove();
            UpdateJump();
            UpdateAttack();
            UpdateReload();

            rangeView.Updated();
        }

        private void UpdateRotate()
        {
            float rotationRatchet = .0f;
            float mouseX = .0f;
            float mouseY = .0f;

            // 회전을 래칫하는 데 키 사용
            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotationRatchet -= RotationRatchet;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                rotationRatchet += RotationRatchet;
            }

            if (GetKeyEvent.isPossibleRotate)
            {
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
            }

            rotateToMouse.UpdateRotate(mouseX, mouseY, rotationRatchet);
            
        }

        private void UpdateMove()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                if (Input.GetKey(keyCodeRun))
                {
                    movement.MoveSpeed = status.RunSpeed;
                    //weapon.Animator.SetFloat("movementSpeed", 1.0f);
                }
                else
                {
                    movement.MoveSpeed = status.WalkSpeed;
                    //weapon.Animator.SetFloat("movementSpeed", 0.5f);
                }

                movement.UpdateMove(horizontal, vertical);
            }
            else
            {
                //weapon.Animator.SetFloat("movementSpeed", 0.0f);
            }

            movement.UpdateMove(horizontal, vertical);
        }

        private void UpdateJump()
        {
            if (Input.GetKeyDown(keyCodeJump))
            {
                bool isPossibleJump = movement.Jump();

                if (isPossibleJump)
                {
                    //weapon.Animator.SetTrigger("onJump");
                }
            }
        }

        private void UpdateAttack()
        {
            if (Input.GetMouseButton(0))
            {
                //weapon.StartAttack();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //weapon.StopAttack();
            }
        }

        private void UpdateReload()
        {
            if (Input.GetKeyDown(keyCodeReload))
            {
                //weapon.StartReload();
            }
        }
    }
}