using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    /// <summary>
    /// 잡을수 있는 손 역할 클래스
    /// </summary>
    public class Grabber : MonoBehaviour
    {
        #region Component
        public Collider followingHand;
        private Rigidbody _rigidbody;
        private Camera mainCamera;
        private MoveToMouse moveToMouse;
        private HandRotateToMouse handRotateToMouse;
#if OpenVR
        [SerializeField]private OpenVRInputManager inputManager;
#else
#endif
#endregion
#region Var
        public List<Grabbable> followingGrabbableList = new List<Grabbable>();
        public List<Grabbable> grabbableList = new List<Grabbable>();
        /// <summary>
        /// 쥘물건이 있는지 확인
        /// </summary>
        private bool isPossibleGrab = true;
        /// <summary>
        /// 손에 물건을 쥐고 있는지 확인
        /// </summary>
        private bool isGrab = false;
#endregion

        public void Init()
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
#if OpenVR
            BoxCollider boxCol = followingHand as BoxCollider;
            transform.Find("ViveHand").transform.localScale = boxCol.size;
#else
            moveToMouse = gameObject.AddComponent<MoveToMouse>();
            moveToMouse.Init();
            handRotateToMouse = gameObject.AddComponent<HandRotateToMouse>();
#endif
            Started();
        }

        public void Started()
        {
#if OpenVR
            inputManager = FindObjectOfType<OpenVRInputManager>();
#else
            GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera = cameraObj.GetComponent<Camera>() as Camera;
#endif
        }

        public void Updated()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{

            //}
            // 컨트롤키가 아니거나 우클릭이 아닐때 움직인다.
#if OpenVR
#else
            if (!GetKeyEvent.isPossibleGrabbableRotate && !GetKeyEvent.isPossibleRotate )
            {
                MoveGrabber();
                GrabRay();
            }
            if (GetKeyEvent.isPossibleGrabbableRotate)
            {
                UpdateRotate();
            }
#endif
            GrabCheck();
        }
#if OpenVR
#else
        private void UpdateRotate()
        {

            float mouseX = .0f;
            float mouseY = .0f;

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            handRotateToMouse.UpdateRotate(mouseX, mouseY);

        }

        void MoveGrabber()
        {
            float mouseX = .0f;
            float mouseY = .0f;
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            moveToMouse.UpdateMove(mouseX, mouseY);
        }

        void GrabRay()
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(transform.position);

            // Grabbable이 grabPoint보다 가까워야 잡는 용도
            float maxDistance = .0f;
            // 잡는 거리 제한
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GrabberPoint")))
            {
                maxDistance = Vector3.Distance(hit.point, mainCamera.transform.position);
                //손은 계속 따라다님
                FollowObj(followingHand, hit);

                // 잡은 물체 따라다님
                for (int i = 0; i < followingGrabbableList.Count; i++)
                {
                    FollowObj(followingGrabbableList[i], hit);
                }
            }

            // Grabbable의 거리를 잡는 거리로 제한
            if (Physics.Raycast(ray, out hit, maxDistance, 1 << LayerMask.NameToLayer("Grabbable")))
            {
                //손은 계속 따라다님
                FollowObj(followingHand, hit);

                // 잡은 물체 따라다님
                for (int i = 0; i < followingGrabbableList.Count; i++)
                {
                    FollowObj(followingGrabbableList[i], hit);
                }
            }
        }

        void FollowObj(Grabbable followObj, RaycastHit hit)
        {
            followObj.transform.position = new Vector3(
                hit.point.x,
                hit.point.y,
                hit.point.z
            );
        }

        void FollowObj(Collider followObj, RaycastHit hit)
        {
            followObj.transform.position = new Vector3(
                hit.point.x,
                hit.point.y,
                hit.point.z
            );
        }
#endif
        void GrabCheck()
        {
            // 잡을 물체들이 있을때 왼쪽버튼 클릭시 잡음
#if OpenVR
            if (!isGrab && isPossibleGrab && inputManager.isTriggerActionDown)
            {
                Grab();
            }
            // 잡았을때는 무조건 취소
            else if (isGrab && inputManager.isTriggerActionDown)
            {
                GrabCancel();
            }
#else
            if (!isGrab && isPossibleGrab && Input.GetMouseButtonDown(0))
            {
                Grab();
            }
            // 잡았을때는 무조건 취소
            else if (isGrab && Input.GetMouseButtonDown(0))
            {
                GrabCancel();
            }
#endif
        }

        void Grab()
        {
            isGrab = true;

            for (int i = 0; i < grabbableList.Count; i++)
            {
                Grabbable grabbable = grabbableList[i];
                grabbable = grabbable.Grab(isGrab);
                grabbable.transform.parent = transform;
                followingGrabbableList.Add(grabbable);
            }
            grabbableList.Clear();
        }

        public void GrabCancel()
        {
            isGrab = false;

            for (int i = 0; i < followingGrabbableList.Count; i++)
            {
                Grabbable grabbable = followingGrabbableList[i];
                grabbable.IsGrab = isGrab;
                grabbable.transform.parent = null;
            }
            followingGrabbableList.Clear();
        }



        private void OnTriggerEnter(Collider other)
        {
            // 쥘물건이 아닐때는 종료
            Grabbable grabbable = other.GetComponent<Grabbable>() ?? other.GetComponentInParent<Grabbable>();
            if (grabbable == null)
                return;
            GrabbableEnter(grabbable);
        }
        
        //연속클릭시 반응 안 해서 작성
        private void OnTriggerStay(Collider other)
        {
            // 쥘물건이 아닐때는 종료
            Grabbable grabbable = other.GetComponent<Grabbable>() ?? other.GetComponentInParent<Grabbable>();
            if (grabbable == null)
                return;
            GrabbableStay(grabbable);
        }

        void OnTriggerExit(Collider other)
        {
            // 쥘물건이 아닐때는 종료
            Grabbable grabbable = other.GetComponent<Grabbable>() ?? other.GetComponentInParent<Grabbable>();
            if (grabbable == null)
                return;
            GrabbableExit(grabbable);
        }

        public void GrabbableEnter(Grabbable grabbable)
        {
            grabbable.GrabbableEnter();
            // 쥘물건이 있을때 쥘수있는 물건 리스트에 넣음
            isPossibleGrab = true;
            if (!grabbableList.Contains(grabbable) && grabbableList.Count < 1)
            {
                grabbableList.Add(grabbable);
            }
        }

        //연속클릭시 반응 안 할때 
        public void GrabbableStay(Grabbable grabbable)
        {
            if (!grabbableList.Contains(grabbable) && grabbableList.Count < 1)
            {
                grabbable.GrabbableEnter();
                // 쥘물건이 있을때 쥘수있는 물건 리스트에 넣음
                isPossibleGrab = true;
                grabbableList.Add(grabbable);
            }
        }

        public void GrabbableExit(Grabbable grabbable)
        {
            grabbable.GrabbableExit();
            //쥘물건 리스트에 있을경우
            if (grabbableList.Count > 0)
            {
                if (grabbableList.Contains(grabbable))
                {
                    grabbableList.Remove(grabbable);
                }
            }
            else
            {
                //쥘물건이 없을경우
                isPossibleGrab = false;
            }
        }
    }
}
