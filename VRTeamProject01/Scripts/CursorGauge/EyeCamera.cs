using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace VRTeamProject01
{
    public class EyeCamera : MonoBehaviour
    {
        #region Component
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CursorGauge cursorGauge;
        #endregion
        #region var
        public List<RaycastResult> raycastResult;
        #endregion
        private void Start()
        {
            cursorGauge = GameObject.Find("CursorGauge").GetComponent<CursorGauge>();
            mainCamera = GetComponent<Camera>() as Camera;
        }

        private void Update()
        {

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mainCamera.transform.forward);

            float maxDistance = .0f;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("UI")))
            {
                if (cursorGauge.IsFull)
                {
                    cursorGauge.ExitGauge();
                    Collider col = hit.collider;
                    if (col.name.Contains("Order"))
                    {
                        OrderManager.Instance.ReceiveOrder(col.GetComponentInParent<OrderHandler>());
                    }
                }
                maxDistance = Vector3.Distance(hit.point, mainCamera.transform.position);
                cursorGauge.EnterGauge();
#if UNITY_EDITOR
                Debug.Log(hit.collider.name);
#endif
            }
            else
            {
                cursorGauge.ExitGauge();
            }
        }
    }
}