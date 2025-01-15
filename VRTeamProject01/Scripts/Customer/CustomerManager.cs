using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    enum CustomerType
    {
        Man_1,
        Girl_1,
        Man_2,
        Girl_2,
        Man_1_Girl_1,
    }
    public class CustomerManager : MonoBehaviour {

        public static CustomerManager Instance;
        #region Component
        public Transform[] createPoints;
        [SerializeField] private GameObject wayPointGroup_Enter;
        [SerializeField] private Transform[] wayPointGroup_Enter_Transform;
        public Transform[] WayPointGroup_Enter_Transform
        {
            get
            {
                return wayPointGroup_Enter_Transform;
            }
        }
        [SerializeField] private GameObject wayPointGroup_ReceiveOrder;
        [SerializeField] private Transform[] wayPointGroup_ReceiveOrder_Transform;
        public Transform[] WayPointGroup_ReceiveOrder_Transform
        {
            get
            {
                return wayPointGroup_ReceiveOrder_Transform;
            }
        }
        [SerializeField] private GameObject wayPointGroup_Exit;
        [SerializeField] private Transform[] wayPointGroup_Exit_Transform;
        public Transform[] WayPointGroup_Exit_Transform
        {
            get
            {
                return wayPointGroup_Exit_Transform;
            }
        }
        [SerializeField] private GameObject[] wayPointGroup_Tables;
        public Transform[] WayPointGroup_Tables_Transform
        {
            get
            {
                int randIdx = Random.Range(0, wayPointGroup_Table_sortCount--);
                GameObject wayPointGroup_Table = wayPointGroup_Tables[randIdx];
#if UNITY_EDITOR
                Debug.Log(randIdx + " : " + wayPointGroup_Table_sortCount);
#endif
                if (randIdx != wayPointGroup_Table_sortCount)
                {
                    wayPointGroup_Tables[randIdx] = wayPointGroup_Tables[wayPointGroup_Table_sortCount];
                    wayPointGroup_Tables[wayPointGroup_Table_sortCount] = wayPointGroup_Table;
                }
                return wayPointGroup_Table.GetComponentsInChildren<Transform>();
            }
        }
        #endregion
        #region Var
        public GameObject customer;
        public GameObject[] Men;
        int menLength;
        public GameObject[] Girls;
        int girlsLength;
        public RuntimeAnimatorController malePizzaAnimator;
        public RuntimeAnimatorController girlPizzaAnimator;
        [SerializeField] private int wayPointGroup_Table_sortCount;
        public int WayPointGroup_Table_sortCount
        {
            get { return wayPointGroup_Table_sortCount; }
        }
        public bool IsWayPointGroup_Table
        {
            get
            {
                if (wayPointGroup_Table_sortCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        private void Awake()
        {
            Instance = this;
            customer = Resources.Load<GameObject>("Prefabs/Customer");
            Men = Resources.LoadAll<GameObject>("Men");
            menLength = Men.Length;
            Girls = Resources.LoadAll<GameObject>("Girls");
            girlsLength = Girls.Length;
            malePizzaAnimator = Resources.Load<RuntimeAnimatorController>("Animations/malePizzaAnimator");
            girlPizzaAnimator = Resources.Load<RuntimeAnimatorController>("Animations/girlPizzaAnimator");
            wayPointGroup_Enter = GameObject.FindGameObjectWithTag("WayPointGroup_Enter");
            wayPointGroup_Enter_Transform = wayPointGroup_Enter.GetComponentsInChildren<Transform>();

            wayPointGroup_ReceiveOrder = GameObject.FindGameObjectWithTag("WayPointGroup_ReceiveOrder");
            wayPointGroup_ReceiveOrder_Transform = wayPointGroup_ReceiveOrder.GetComponentsInChildren<Transform>();

            wayPointGroup_Exit = GameObject.FindGameObjectWithTag("WayPointGroup_Exit");
            wayPointGroup_Exit_Transform = wayPointGroup_Exit.GetComponentsInChildren<Transform>();

            wayPointGroup_Tables = GameObject.FindGameObjectsWithTag("WayPointGroup_Table");
            wayPointGroup_Table_sortCount = wayPointGroup_Tables.Length;
        }

        private void Start()
        {
            CreateCustomer();
        }

        public void CreateCustomer()
        {
            int ranIdx = Random.Range(0, System.Enum.GetValues(typeof(CustomerType)).Length);
            CustomerType customerType = (CustomerType)ranIdx;
            
            switch (customerType)
            {
                case CustomerType.Man_1:
                    CreateMan();
                    break;
                case CustomerType.Girl_1:
                    CreateGirl();
                    break;
                case CustomerType.Man_2:
                    //CreateMan();
                    CreateMan();
                    break;
                case CustomerType.Girl_2:
                    //CreateGirl();
                    CreateGirl();
                    break;
                case CustomerType.Man_1_Girl_1:
                    //CreateMan();
                    CreateGirl();
                    break;
            }
        }

        void CreateMan()
        {
            GameObject copyCustomer;
            GameObject copyMan;
            Transform createPoint = createPoints[Random.Range(0, createPoints.Length)];
            copyCustomer = Instantiate(customer);
            copyCustomer.transform.position = createPoint.position;
            copyMan = Instantiate(Men[Random.Range(0, menLength)]);

            copyMan.transform.parent = copyCustomer.transform;
            copyMan.transform.localPosition = Vector3.zero;
            copyMan.transform.localRotation = Quaternion.identity;
            Animator copyManAnimator = copyMan.GetComponent<Animator>();
            copyManAnimator.runtimeAnimatorController = malePizzaAnimator;
            copyCustomer.GetComponent<MoveCtrl>().Animator = copyMan.GetComponent<Animator>();
            copyCustomer.transform.localScale = Vector3.one * 1.5f;
        }
        void CreateGirl()
        {
            GameObject copyCustomer;
            GameObject copyGirl;
            Transform createPoint = createPoints[Random.Range(0, createPoints.Length)];
            copyCustomer = Instantiate(customer);
            copyCustomer.transform.position = createPoint.position;
            copyGirl = Instantiate(Girls[Random.Range(0, girlsLength)], Vector3.zero, Quaternion.identity, copyCustomer.transform);

            copyGirl.transform.parent = copyCustomer.transform;
            copyGirl.transform.localPosition = Vector3.zero;
            copyGirl.transform.localRotation = Quaternion.identity;
            Animator copyGirlAnimator = copyGirl.GetComponent<Animator>();
            copyGirlAnimator.runtimeAnimatorController = girlPizzaAnimator;
            copyCustomer.GetComponent<MoveCtrl>().Animator = copyGirl.GetComponent<Animator>();
            copyCustomer.transform.localScale = Vector3.one * 1.5f;
        }
    }

}
