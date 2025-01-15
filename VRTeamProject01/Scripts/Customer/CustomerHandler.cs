using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRTeamProject01
{

    public class CustomerHandler : MonoBehaviour
    {
        public GameObject canvas_OrderObj;
        public Text text_Order;
        OrderHandler orderHandler;
        public void Init()
        {


        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("Counter"))
            {
                if (orderHandler == null)
                {
                    canvas_OrderObj.SetActive(true);
                    orderHandler = gameObject.AddComponent<OrderHandler>();
                    orderHandler.text_Order = text_Order;
                    orderHandler.canvas_OrderObj = canvas_OrderObj;
                    OrderManager.Instance.InitOrderHandler(orderHandler);
                }
            }
        }
    }
}
