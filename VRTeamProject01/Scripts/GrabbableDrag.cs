using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    public enum GrabbableDragType{
        CuttingBoard = 1,
        Pot = 2,
        Oven = 3,
        PizzaBox = 4
    }
    public class GrabbableDrag : MonoBehaviour
    {
        public static List<GrabbableDrag> grabbableDrags = new List<GrabbableDrag>();
        [SerializeField]private Grabbable refGrabbable;
        #region Component
#if OpenVR
        private OpenVRInputManager inputManager;
#else
#endif
        [SerializeField]private MaterialHandler materialHandler;
        [SerializeField]private PotHandler potHandler;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] private PizzaBoxHandler pizzaBoxHandler;
        #endregion
        #region Var
        public bool isXAxisSort;
        public bool isPivotCenter;
        GameObject copyGrabbable;
        public Vector3 boxSize = new Vector3(1.0f, 2.0f, 1.0f);
        bool isGrabbable;
        public bool IsGrabbable{ get { return isGrabbable; } }
        public GrabbableDragType GrabbableDragType;
        #endregion

        public void Init()
        {
#if OpenVR
            inputManager = FindObjectOfType<OpenVRInputManager>();
#else
#endif
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            materialHandler = gameObject.AddComponent<MaterialHandler>();
            materialHandler.Init();
            switch (GrabbableDragType)
            {
                case GrabbableDragType.CuttingBoard:
                    break;
                case GrabbableDragType.Pot:
                    potHandler = gameObject.AddComponent<PotHandler>();
                    break;
                case GrabbableDragType.Oven:
                    break;
                case GrabbableDragType.PizzaBox:
                    pizzaBoxHandler = gameObject.AddComponent<PizzaBoxHandler>();
                    break;
            }
        }

        // Drag자리에 놓을경우 참조할 값
        public void Updated()
        {
            if (isGrabbable)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position, boxSize, transform.rotation, 1 << LayerMask.NameToLayer("Grabbable"));
                if (colliders.Length < 1)
                {
#if UNITY_EDITOR
                    Debug.Log("해제");
#endif
                    GrabbableExit();
                    switch (GrabbableDragType)
                    {
                        case GrabbableDragType.CuttingBoard:
                            break;
                        case GrabbableDragType.Pot:
                            materialHandler.MaterialColorBack();
                            break;
                        case GrabbableDragType.Oven:
                            break;
                        case GrabbableDragType.PizzaBox:
                            break;
                    }
                }
#if OpenVR
                //좌클릭, 원본, 카피가 존재하고, refGrabbable이 들려있는 상태 일때 원본과 카피를 교채한다.
                if (inputManager.isTriggerActionDown && refGrabbable != null)
                {
                    switch (GrabbableDragType)
                    {
                        case GrabbableDragType.CuttingBoard:
                            if (copyGrabbable != null)
                            {
                                PutCopyGrabbable();
                            }
                            break;
                        case GrabbableDragType.Pot:
#if UNITY_EDITOR
                            Debug.Log("POT 교체");
#endif
                            PutGrabbableToPot(transform, Vector3.up);
                            materialHandler.MaterialColorBack();
                            break;
                        case GrabbableDragType.Oven:
                            if (copyGrabbable != null)
                            {
                                PutCopyGrabbable();
                                GameObject.FindObjectOfType<Oven>().BeltOn = true;
                            }
                            break;
                        case GrabbableDragType.PizzaBox:
                            if (copyGrabbable != null)
                            {
                                PutCopyGrabbable();
                                Animator animator = GetComponent<Animator>();
                                animator.SetBool("isCover", true);
                                animator.GetBehaviour<PizzaBoxAnim>().RefGrabbable = refGrabbable;
                                
                            }
                            break;
                    }
                    GrabbableExit();
                    //GameManager.Instance.GrabbableDragExit();
                }
#else
                //좌클릭, 원본, 카피가 존재하고, refGrabbable이 들려있는 상태 일때 원본과 카피를 교채한다.
                if (Input.GetMouseButtonDown(0) && refGrabbable != null)
                {
                    switch (GrabbableDragType)
                    {
                        case GrabbableDragType.CuttingBoard:
                            if (copyGrabbable != null)
                            {
                                PutCopyGrabbable();
                            }
                            break;
                        case GrabbableDragType.Pot:
#if UNITY_EDITOR
                            Debug.Log("POT 교체");
#endif
                            PutGrabbableToPot(transform, Vector3.up);
                            materialHandler.MaterialColorBack();
                            break;
                        case GrabbableDragType.Oven:
                            if (copyGrabbable != null)
                            {
                                PutCopyGrabbable();
                                GameObject.FindObjectOfType<Oven>().BeltOn = true;
                            }
                            break;
                        case GrabbableDragType.PizzaBox:
                            if (copyGrabbable != null)
                            {
                                PutCopyGrabbable();
                                Animator animator = GetComponent<Animator>();
                                animator.SetBool("isCover", true);
                                animator.GetBehaviour<PizzaBoxAnim>().RefGrabbable = refGrabbable;
                                
                            }
                            break;
                    }
                    GrabbableExit();
                    //GameManager.Instance.GrabbableDragExit();
                }
#endif
            }

            if (!isGrabbable)
            {
                // 손에 쥔물건이 가까이 올때
                Collider[] colliders = Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation, 1 << LayerMask.NameToLayer("Grabbable"));

                foreach (var collider in colliders)
                {
                    Grabbable grabbable = collider.GetComponent<Grabbable>() ?? collider.GetComponentInParent<Grabbable>();
                    if (grabbable == null)
                        continue;
                    //Grabbable이 손에 쥔상태가 아닐때는 카피복사안함
 
                    //손에 쥔 물건만 drag가능
                    if (grabbable.IsGrab)
                    {
                        //다른 테이블은 Drag 취소
                        GameManager.Instance.GrabbableDragExit();

                        //닿은 물건이 Grabbable 일때 딱하나만 자리에 놓을 수 있음
                        isGrabbable = true;
                        refGrabbable = grabbable;
                        switch (GrabbableDragType)
                        {
                            case GrabbableDragType.CuttingBoard:
#region CUTTINGBOARD
                                copyGrabbable = Instantiate(grabbable.gameObject);

                                if (isPivotCenter)
                                {
                                    GrabbableIsPivotCenterPosition(transform, copyGrabbable);
                                }
                                else
                                {
                                    GrabbableIsPivotBottomPosition(transform, copyGrabbable);
                                }
#if OpenVR
                                copyGrabbable.transform.localScale = copyGrabbable.transform.localScale;
#else
                                copyGrabbable.transform.localScale = copyGrabbable.transform.localScale * 0.1f;
#endif
                                if (isXAxisSort)
                                {
                                    copyGrabbable.transform.rotation = Quaternion.Euler(.0f, 90.0f, .0f);
                                }
                                else
                                {
                                    copyGrabbable.transform.rotation = Quaternion.identity;
                                }
                                // Drag자리를 나타낼 만들어진 copyGrabbable 색변경후 Grabbable컴포넌트 삭제하여 인식불가상태로 변경
                                grabbable = copyGrabbable.GetComponent<Grabbable>();
                                grabbable.GrabbableDrag();
                                Destroy(grabbable);
                                break;
#endregion CUTTINGBOARD
                            case GrabbableDragType.Pot:
#region POT
                                materialHandler.MaterialColorChange(Color.yellow);
#endregion
                                break;
                            case GrabbableDragType.Oven:
#region Oven
                                copyGrabbable = Instantiate(grabbable.gameObject);

                                if (isPivotCenter)
                                {
                                    GrabbableIsPivotCenterPosition(transform, copyGrabbable);
                                }
                                else
                                {
                                    GrabbableIsPivotBottomPosition(transform, copyGrabbable);
                                }
#if OpenVR
                                copyGrabbable.transform.localScale = copyGrabbable.transform.localScale;
#else
                                copyGrabbable.transform.localScale = copyGrabbable.transform.localScale * 0.1f;
#endif
                                if (isXAxisSort)
                                {
                                    copyGrabbable.transform.rotation = Quaternion.Euler(.0f, 90.0f, .0f);
                                }
                                else
                                {
                                    copyGrabbable.transform.rotation = Quaternion.identity;
                                }
                                // Drag자리를 나타낼 만들어진 copyGrabbable 색변경후 Grabbable컴포넌트 삭제하여 인식불가상태로 변경
                                grabbable = copyGrabbable.GetComponent<Grabbable>();
                                grabbable.GrabbableDrag();
                                GameObject.FindObjectOfType<Oven>().BeltOn = false;
                                Destroy(grabbable);
                                break;
#endregion Oven
                            case GrabbableDragType.PizzaBox:
#region PizzaBox
                                copyGrabbable = Instantiate(grabbable.gameObject);

                                if (isPivotCenter)
                                {
                                    GrabbableIsPivotCenterPosition(transform, copyGrabbable);
                                }
                                else
                                {
                                    GrabbableIsPivotBottomPosition(transform, copyGrabbable);
                                }
#if OpenVR
                                        copyGrabbable.transform.localScale = copyGrabbable.transform.localScale;
#else
                                copyGrabbable.transform.localScale = copyGrabbable.transform.localScale * 0.1f;
#endif
                                if (isXAxisSort)
                                {
                                    copyGrabbable.transform.rotation = Quaternion.Euler(.0f, 90.0f, .0f);
                                }
                                else
                                {
                                    copyGrabbable.transform.rotation = Quaternion.identity;
                                }
                                // Drag자리를 나타낼 만들어진 copyGrabbable 색변경후 Grabbable컴포넌트 삭제하여 인식불가상태로 변경
                                grabbable = copyGrabbable.GetComponent<Grabbable>();
                                grabbable.GrabbableDrag();
                                Destroy(grabbable);
                                break;
#endregion PizzaBox
                        }
                    }
                }
            }
        }

        void PutCopyGrabbable()
        {
            //GameObject.FindObjectOfType<Grabber>().GrabCancel();
            refGrabbable.transform.position = copyGrabbable.transform.position;
            refGrabbable.transform.rotation = copyGrabbable.transform.rotation;
            refGrabbable.transform.localScale = copyGrabbable.transform.localScale;
            refGrabbable.Velocity = Vector3.zero;
        }

        void PutGrabbableToPot(Transform transform, Vector3 addPosition)
        {
            //GameObject.FindObjectOfType<Grabber>().GrabCancel();
            refGrabbable.transform.position = transform.position + addPosition;
            refGrabbable.Velocity = Vector3.zero;
            StartCoroutine(PutGrabbableToPotAnimation(refGrabbable.gameObject));    
        }

        

        void GrabbableIsPivotCenterPosition(Transform transform, GameObject grabbable)
        {
            grabbable.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + (transform.localScale.y * 0.5f) + (grabbable.transform.localScale.y * 0.5f),
                transform.position.z
                );
        }

        void GrabbableIsPivotBottomPosition(Transform transform, GameObject grabbable)
        {
            grabbable.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + (transform.localScale.y * 0.5f),
                transform.position.z
                );
        }

        void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            //Gizmos.matrix = transform.;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
        }
        /*
        private void OnTriggerEnter(Collider other)
        {
            Grabbable grabbable = other.GetComponent<Grabbable>() ?? other.GetComponentInParent<Grabbable>();
            if (grabbable == null)
                return;
            copyGrabbable = Instantiate(grabbable.gameObject, grabbable.transform.position, grabbable.transform.rotation);
            Destroy(copyGrabbable.GetComponent<Grabbable>());
        }
        private void OnTriggerExit(Collider other)
        {
            Grabbable grabbable = other.GetComponent<Grabbable>() ?? other.GetComponentInParent<Grabbable>();
            if (grabbable == null)
                return;
            GrabbableExit();
            switch (GrabbableDragType)
            {
                case GrabbableDragType.CUTTINGBOARD:
                    break;
                case GrabbableDragType.POT:
                    materialHandler.MaterialColorBack();
                    break;
            }
            Debug.Log(GrabbableDragType);
        }
        
        */

        public void GrabbableExit()
        {
            switch (GrabbableDragType)
            {
                case GrabbableDragType.CuttingBoard:
                    if (copyGrabbable != null && refGrabbable != null && isGrabbable == true)
                    {
                        Destroy(copyGrabbable);
                        isGrabbable = false;
                        refGrabbable = null;
                    }
                    break;
                case GrabbableDragType.Pot:
                    if (refGrabbable != null && isGrabbable == true)
                    {
                        materialHandler.MaterialColorBack();
                        isGrabbable = false;
                        refGrabbable = null;
                    }
                    break;
                case GrabbableDragType.Oven:
                    if (refGrabbable != null && refGrabbable != null && isGrabbable == true)
                    {
                        Destroy(copyGrabbable);
                        isGrabbable = false;
                        refGrabbable = null;
                    }
                    break;
                case GrabbableDragType.PizzaBox:
                    if (refGrabbable != null && refGrabbable != null && isGrabbable == true)
                    {
                        Destroy(copyGrabbable);
                        isGrabbable = false;
                        refGrabbable = null;
                    }
                    break;
            }
        }

        IEnumerator PutGrabbableToPotAnimation(GameObject refGrabbableObj)
        {

            Transform refGrabbableTransform = refGrabbableObj.transform;
            float sqrMagnitude = refGrabbableTransform.localScale.sqrMagnitude;
            AudioManager.Instance.PlayEffect("효과음", 1.0f);
            while (true)
            {
                refGrabbableTransform.localScale = refGrabbableTransform.localScale * 0.9f;
                yield return new WaitForFixedUpdate();
                if (refGrabbableTransform.localScale.sqrMagnitude < sqrMagnitude * 0.01f)
                {
                    break;
                }
            }
            GroceriesHandler groceriesHandler = refGrabbableObj.GetComponent<GroceriesHandler>();
            if (groceriesHandler != null)
            {
                potHandler.PutGroceriesToPot(groceriesHandler.groceries);
            }
            GameManager.Instance.DestroyGrabbable(refGrabbableObj.GetComponent<Grabbable>());
            Destroy(refGrabbableObj);
        }
    }
}
