using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{

    public enum PointType
    {
        Enter,
        ReceiveOrder,
        Exit,
        Table,
    }

    public class MoveCtrl : MonoBehaviour
    {
        public enum MoveType
        {
            WAY_POINT,
            LOOK_AT,
            DAYDREAM,
        }
        #region Component
        private Transform tr;
        private Transform camTr;
        private CharacterController cc;
        [SerializeField] private Transform[] points;
        private Animator animator;
        public Animator Animator
        {
            set
            {
                animator = value;
            }
        }
        #endregion
        #region Var
        public MoveType moveType = MoveType.WAY_POINT;
        public PointType pointType = PointType.Enter;
        public float speed = 1.0f;
        public float damping = 3.0f;
        
        [SerializeField]private int nextIdx = 1;

        public static bool isStopped = false;
        #endregion



        public void AnimatorSetTrigger(string trigger)
        {
            animator.SetTrigger(trigger);
        }

        [SerializeField]private bool isWalking;
        public bool IsWalking
        {
            set
            {
                isWalking = value;
            }
        }
        private bool isGetPizza = false;
        public bool IsGetPizza
        {
            get
            {
                return isGetPizza;
            }
            set
            {
                isGetPizza = value;
            }
        }

        private void Awake()
        {
            tr = GetComponent<Transform>();
            cc = GetComponent<CharacterController>();
        }

        void Start()
        {
            camTr = Camera.main.GetComponent<Transform>();
            
            //총에 추가된 Animator 컴포넌트 추출
            //WayPointGroup 게임오브젝트 아래에 있는 모든 Point의 Transform 컴포넌트를 추출

            PointsSetting(pointType);
        }

        public void PointsSetting(PointType pointType)
        {
            this.pointType = pointType;
            isStopped = false;
            nextIdx = 1;
            isWalking = true;
            animator.SetBool("isWalking", true);
            Vector3 direction;
            Quaternion rot;
            switch (pointType)
            {
                case PointType.Enter:
                    points = CustomerManager.Instance.WayPointGroup_Enter_Transform;
                    break;
                case PointType.ReceiveOrder:
                    points = CustomerManager.Instance.WayPointGroup_ReceiveOrder_Transform;
                    break;
                case PointType.Exit:
                    points = CustomerManager.Instance.WayPointGroup_Exit_Transform;
                    direction = points[nextIdx].position - tr.position;
                    rot = Quaternion.LookRotation(direction);
                    tr.rotation = rot;
                    break;
                case PointType.Table:
                    if (CustomerManager.Instance.WayPointGroup_Table_sortCount > 0)
                    {
                        points = CustomerManager.Instance.WayPointGroup_Tables_Transform;
                        direction = points[nextIdx].position - tr.position;
                        rot = Quaternion.LookRotation(direction);
                        tr.rotation = rot;
                    }
                    break;
            }
        }

        void Update()
        {
            //총에 추가된 Animator 컴포넌트의 활성화/비활성화

            if (isStopped)
                return;

            switch (moveType)
            {
                case MoveType.WAY_POINT:
                    if (isWalking)
                    {
                        MoveWayPoint();
                    }
                    break;
                case MoveType.LOOK_AT:
                    MoveLookAt();
                    break;
                case MoveType.DAYDREAM:
                    break;
            }
        }

        void MoveWayPoint()
        {
            Vector3 direction = points[nextIdx].position - tr.position;
            Quaternion rot = Quaternion.LookRotation(direction);
            //rot.eulerAngles = new Vector3(tr.rotation.eulerAngles.x, rot.y, tr.rotation.eulerAngles.z);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping * speed);
            tr.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        void MoveLookAt()
        {

            Vector3 dir = camTr.TransformDirection(Vector3.forward);
            // dir 벡터의 방향으로 초당 speed만큼씩이동
            cc.SimpleMove(dir * speed);
        }

        public void OrderEnd()
        {
            if (CustomerManager.Instance.IsWayPointGroup_Table)
            {
                if (Random.Range(0, 2) % 2 == 0)
                {
                    PointsSetting(PointType.Exit);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("테이블로이동");
#endif
                    PointsSetting(PointType.Table);
                }
            }
            else
            {
                PointsSetting(PointType.Exit);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("WAY_POINT"))
            {
#if UNITY_EDITOR
                print(points[0].transform.name);
#endif
                if (other.transform.parent.name.Contains(points[0].transform.name))
                {
                    isWalking = false;
                    if (pointType == PointType.Table)
                    {
                        if (nextIdx == 1 && points[1] != other.transform)
                        {
                            return;
                        }
                    }
                    nextIdx = (++nextIdx >= points.Length) ? points.Length : nextIdx;
                    //WAY_POINT 마지막일때
                    if (nextIdx == points.Length)
                    {
                        switch (pointType)
                        {
                            case PointType.Enter:
                            case PointType.ReceiveOrder:
                                LookPlayer();
                                break;
                            case PointType.Table:
                                tr.parent = other.transform;
                                tr.localPosition = Vector3.zero;
                                tr.localRotation = Quaternion.identity;
                                animator.SetTrigger("SitTrigger");
                                Destroy(GetComponentInChildren<PizzaBoxHandler>().gameObject);
                                break;
                            case PointType.Exit:
                                Destroy(gameObject);
                                break;

                        }
                        if (pointType == PointType.Table)
                        {
                            return;
                        }
                    }
                    
                    //이게 실행되야 Idle로 넘어감
                    animator.SetTrigger("DanceTrigger");
                }
            }
        }

        void LookPlayer()
        {
            isStopped = true;
            animator.SetBool("isWalking", !isStopped);
            Vector3 playerPoint = GameManager.Instance.PlayerTransform.position;
            playerPoint.y = 0.0f;
            Vector3 direction = GameManager.Instance.PlayerTransform.position - tr.position;
            Quaternion rot = Quaternion.LookRotation(direction);
            tr.rotation = rot;
        }
    }
}