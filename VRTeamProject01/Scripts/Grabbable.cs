using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    [RequireComponent(typeof(Rigidbody))]
    //[RequireComponent(typeof(RotateToMouse))]
    public class Grabbable : MonoBehaviour
    {
        #region Conponents
        Collider _collider;
        public Collider Collider
        {
            get
            {
                return _collider;
            }
        }
        //Camera mainCamera;
        [SerializeField] Rigidbody _rigidbody;
        public Rigidbody Rigidbody
        {
            get {
                return _rigidbody;
            }
            set {
                _rigidbody = value;
            }
        }

        private RotateToMouse rotateToMouse;
        [SerializeField] private MaterialHandler materialHandler;
        [SerializeField] private GroceriesHandler groceriesHandler;
        #endregion

        #region var
        bool _isGrab;
        public bool IsGrab
        {
            get
            {
                return _isGrab;
            }
            set
            {
                _isGrab = value;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
                _rigidbody.isKinematic = false;
                //Grab상태일때
                if (_isGrab)
                {

                    _rigidbody.useGravity = false;
                    _collider.isTrigger = true;

                    //if (name.Equals("Bat"))
                    //{
                    //    GameObject batFollower = GameObject.Find("Bat Follower");
                    //    if (batFollower == null)
                    //    {
                    //        batFollower = new GameObject("Bat Follower");
                    //    }
                    //    if (batFollower != null)
                    //    {
                    //        BatCapsuleFollower[] batCapsuleFollowers = batFollower.GetComponentsInChildren<BatCapsuleFollower>();
                    //        foreach (BatCapsuleFollower batCapsuleFollower in batCapsuleFollowers)
                    //        {
                    //            batCapsuleFollower.Enable();
                    //        }
                    //    }
                    //}
                }
                //Grab상태 아닐때
                else
                {
                    if (GameManager.Instance.IsGrabbable)
                    {
                        _velocity = Vector3.zero;
                    }
                    //if (name.Equals("Bat"))
                    //{
                    //    GameObject batFollower = GameObject.Find("Bat Follower");
                    //    if (batFollower == null)
                    //    {
                    //        batFollower = new GameObject("Bat Follower");
                    //    }
                    //    if (batFollower != null)
                    //    {
                    //        BatCapsuleFollower[] batCapsuleFollowers = batFollower.GetComponentsInChildren<BatCapsuleFollower>();
                    //        foreach (BatCapsuleFollower batCapsuleFollower in batCapsuleFollowers)
                    //        {
                    //            batCapsuleFollower.Disable();
                    //        }
                    //    }
                    //}
                    _rigidbody.useGravity = true;
                    _collider.isTrigger = false;
                    StartCoroutine("DestroyGrabbable");
                    
                }
            }
        }

        bool isPossibleRotate;

        private Vector3 _velocity;
        public Vector3 Velocity
        {
            set
            {
                _velocity = value;
            }
        }
#if OpenVR
        private float _sensitivity = 100.0f;
#else
        private float _sensitivity = 20.0f;
#endif
        public Groceries groceries;
        #endregion

        public void Init()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rotateToMouse = GetComponent<RotateToMouse>();
            if (materialHandler == null)
            {
                materialHandler = gameObject.AddComponent<MaterialHandler>();
                materialHandler.Init();
            }
            if (groceriesHandler == null)
            {
                groceriesHandler = gameObject.AddComponent<GroceriesHandler>();
                groceriesHandler.Init(groceries);
            }
        }

        Vector3 lastDestination;

        public void Updated()
        {
            if (_isGrab)
            {

                Vector3 destination = transform.position;
                _velocity = (destination - lastDestination) * _sensitivity;
                //_rigidbody.velocity = _velocity;
                lastDestination = destination;
            }
            else
            {
                if (_velocity.magnitude > 0)
                {
                    _rigidbody.velocity = _velocity;
                }
            }
            //transform.rotation = transform.rotation;
        }



        public void GrabbableEnter()
        {
            materialHandler.MaterialColorChange(Color.cyan);
        }

        public void GrabbableExit()
        {
            materialHandler.MaterialColorBack();
        }

        public void GrabbableDrag()
        {
            materialHandler.MaterialColorChange(Color.yellow);
        }

        public Grabbable Grab(bool isGrab)
        {
#if UNITY_EDITOR
            Debug.Log(name + " : " + groceriesHandler.IsGroceries);
#endif
            if (groceriesHandler.IsGroceries)
            {
                Grabbable copyGrabbable = Instantiate(this);
                copyGrabbable.name = this.name;
                copyGrabbable.transform.position = transform.position;
                GameManager.Instance.InitAddGrabbable(copyGrabbable);
                copyGrabbable.IsGrab = true;
                return copyGrabbable;
            }
            else
            {
                IsGrab = true;
                return this;
            }
        }

        IEnumerator DestroyGrabbable()
        {
            yield return new WaitForSeconds(5.0f);
            Destroy(gameObject);
            GameManager.Instance.DestroyGrabbable(this);
        }

        //private void Start()
        //{
        //    mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //}

        //#region GrabbablePhysics
        //void GrabbablePhysics()
        //{
        //    RaycastHit hit;
        //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //    // Grabbable 거리 제한
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GrabberPoint")))
        //    {
        //        // 타격 위치 표시용
        //        Vector3 hitPoint = hit.point;
        //        Collider[] colliders = Physics.OverlapSphere(hitPoint, 10.0f);
        //        foreach (var collider in colliders)
        //        {
        //            Grabbable grabbable = collider.GetComponent(typeof(Grabbable)) as Grabbable;
        //            if (grabbable != null)
        //            {

        //                Rigidbody rb = grabbable.Rig3D;
        //                if (rb == this.rig3D)
        //                {
        //                    Debug.Log("rbName : " + rb.name);
        //                    continue;
        //                }


        //                rb.mass = 1.0f;
        //                rb.AddExplosionForce(200.0f, hitPoint, 10.0f, 300.0f);
        //            }
        //        }
        //    }
        //}
        //#endregion


        //private void OnTriggerEnter(Collider other)
        //{
        //    if (IsGrab)
        //    {
        //        if (other.GetComponent<GrabberPoint>())
        //        {
        //            return;
        //        }
        //        // Debug.Log("작동");
        //        //GrabbablePhysics();
        //    }
        //}
    }
}