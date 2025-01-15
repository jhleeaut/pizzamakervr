using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace VRTeamProject01
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        #region Component
#if OpenVR
        [SerializeField] private GameObject playerObj;
#else
        [SerializeField] private GetKeyEvent getKeyEvent;
        [SerializeField] private PlayerController playerController;
#endif
        [SerializeField] private List<Grabber>  grabberList = new List<Grabber>();

        [SerializeField] private List<Grabbable> grabbableList = new List<Grabbable>();
        [SerializeField] private List<GrabbableDrag> grabbableDragList = new List<GrabbableDrag>();
        [SerializeField] private TeleportManager teleportManager;
        [SerializeField] private CookingManager cookingManager;
        [SerializeField] private OrderManager orderManager;
        #endregion
        #region Var
        public bool IsGrabbable
        {
            get
            {
                for (int i = 0; i < grabbableDragList.Count; i++)
                {
                    GrabbableDrag grabbableDrag = grabbableDragList[i];
                    if (grabbableDrag.IsGrabbable)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public Transform PlayerTransform
        {
            get
            {
    #if OpenVR
                return playerObj.transform;
    #else
                return playerController.transform;
    #endif
            }
        }
        [SerializeField]private int score;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {

                score = value;
                if(score == 10)
                {
                    //SceneManager.LoadScene("Win");
                }
            }
        }
        #endregion

        private void Awake()
        {
            // First you get the MonoScript of your MonoBehaviour
            //MonoScript monoScript = MonoScript.FromMonoBehaviour(this);
            // Getting the current execution order of that MonoScript
            //int currentExecutionOrder = MonoImporter.GetExecutionOrder(monoScript);
            // Changing the MonoScript's execution order
            //MonoImporter.SetExecutionOrder(monoScript, -10000);

            Instance = this;
            Application.runInBackground = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            GameObject emptyGameObject = new GameObject("Bat Follower");
#if OpenVR
#else
            getKeyEvent = gameObject.AddComponent<GetKeyEvent>();
#endif
            teleportManager = GetComponent<TeleportManager>();
            cookingManager = GetComponent<CookingManager>();
            orderManager = GetComponent<OrderManager>();
        }

        private void Start()
        {
#if OpenVR
            playerObj = GameObject.FindGameObjectWithTag("Player");
#else
            playerController = FindObjectOfType<PlayerController>();
            playerController.Init();
#endif
            Grabber[] grabbers = FindObjectsOfType<Grabber>();
            for (int i = 0; i < grabbers.Length; i++)
            {
                InitAddGrabber(grabbers[i]);
            }


            Grabbable[] grabbables = FindObjectsOfType<Grabbable>();
            for (int i = 0; i < grabbables.Length; i++)
            {
                InitAddGrabbable(grabbables[i]);
            }
            GrabbableDrag[] grabbableDrags = FindObjectsOfType<GrabbableDrag>();
            for (int i = 0; i < grabbableDrags.Length; i++)
            {
                InitAddGrabbableDrag(grabbableDrags[i]);
            }
            
            teleportManager.Init();
            cookingManager.Init();
            orderManager.Init();
        }



        private void Update()
        {
#if OpenVR
#else
            getKeyEvent.Updated();
            playerController.Updated();
#endif
            for (int i = 0; i < grabberList.Count; i++)
            {
                Grabber grabber = grabberList[i];
                grabber.Updated();
            }
            for (int i = 0; i < grabbableList.Count; i++)
            {
                Grabbable grabbable = grabbableList[i];
                grabbable.Updated();
            }
            for (int i = 0; i < grabbableDragList.Count; i++)
            {
                GrabbableDrag grabbableDrag = grabbableDragList[i];
                grabbableDrag.Updated();
            }
        }

        public void InitAddGrabber(Grabber grabber)
        {
            grabber.Init();
            grabberList.Add(grabber);
        }

        public void InitAddGrabbable(Grabbable grabbable)
        {
            grabbable.Init();
            grabbableList.Add(grabbable);
        }

        public void DestroyGrabbable(Grabbable grabbable)
        {
            grabbableList.Remove(grabbable);
        }

        private void InitAddGrabbableDrag(GrabbableDrag grabbableDrag)
        {
            grabbableDrag.Init();
            grabbableDragList.Add(grabbableDrag);
        }


        public void GrabbableDragExit()
        {
            for (int i = 0; i < grabbableDragList.Count; i++)
            {
                GrabbableDrag grabbableDrag = grabbableDragList[i];
                grabbableDrag.GrabbableExit();
            }
        }

        public void Teleport(TeleportPoint teleportPoint)
        {
#if OpenVR
            playerObj.transform.position = teleportPoint.transform.position;
#else
            playerController.transform.position = teleportPoint.transform.position;
#endif
        }
    }
}