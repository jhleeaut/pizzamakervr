using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class PizzaBoxHandler : MonoBehaviour
    {
        /// <summary>
        /// 덮는 애니메이션이 끝나면 true로 바꿈
        /// </summary>
        [SerializeField]bool isOrderComplete;
        
        public bool IsOrderComplete
        {
            get
            {
                return isOrderComplete;
            }
            set
            {
                isOrderComplete = value;
            }
        }
        private CookingPizza refCookingPizza;
        public void OrderPacking(CookingPizza refCookingPizza)
        {
            refCookingPizza.transform.parent = transform;
            refCookingPizza.GetComponent<Rigidbody>().isKinematic = true;
            refCookingPizza.transform.localPosition = Vector3.zero;
            this.refCookingPizza = refCookingPizza;
            IsOrderComplete = true;
        }

        public void OrderComplete(OrderHandler orderHandler)
        {
            if (refCookingPizza.PizzaType == orderHandler.OrderPizzaType)
            {
                orderHandler.OrderMatching(true);
            }
            else if (refCookingPizza.PizzaType != orderHandler.OrderPizzaType)
            {
                orderHandler.OrderMatching(false);
            }
            AudioManager.Instance.PlayEffect("완료음악");
        
            // 피자박스 카피함
            GameObject copy = Instantiate(gameObject);
            copy.GetComponent<Animator>().Play("PizzaBoxCovered");
            foreach (Collider col in copy.GetComponentsInChildren<Collider>())
            {
                col.isTrigger = true;
                col.enabled = false;
            }
           
            copy.transform.parent = orderHandler.transform;
            Vector3 customerPoint = orderHandler.transform.position;
            customerPoint.x = customerPoint.x + 0.5f;
            customerPoint.y = customerPoint.y + 0.5f;
            copy.transform.position = customerPoint;
            GetComponent<Animator>().SetBool("isCover", false);

            //피자 카피하고 피자박스서 피자만 지움
            Init();
            OrderManager.Instance.OrderComplete(orderHandler);
        }

        public void Init()
        {
            if (refCookingPizza != null)
            {
                GameManager.Instance.DestroyGrabbable(refCookingPizza.GetComponent<Grabbable>());
                Destroy(refCookingPizza.gameObject);
                refCookingPizza = null;
            }
            isOrderComplete = false;
        }
    }
}