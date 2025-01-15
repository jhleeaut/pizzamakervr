using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    public class PizzaBox
    {
        public GameObject orderPizza;
        public GrabbableDrag grabbableDrag;
    }
    public class OrderManager : MonoBehaviour
    {
        public static OrderManager Instance;
        //public List<OrderHandler> customerList;
        public List<OrderHandler> orderHandlerList;
        public List<GrabbableDrag> grabbableDragList;

        public void Init () {
            Instance = this;
            orderHandlerList = new List<OrderHandler>();
        }

        public void InitOrderHandler(OrderHandler orderHandler)
        {
            orderHandler.Init();
        }

        public void ReceiveOrder(OrderHandler orderHandler)
        {
            if(orderHandler.name.Contains("Customer"))
            {
                orderHandler.GetComponent<MoveCtrl>().PointsSetting(PointType.ReceiveOrder);
                orderHandler.ReceiveOrder();
            }
            
            orderHandlerList.Add(orderHandler);
        }

        public void OrderComplete(OrderHandler orderHandler)
        {
            if (orderHandlerList.Contains(orderHandler))
            {
                orderHandlerList.Remove(orderHandler);
            }
            GameManager.Instance.Score++;
            CustomerManager.Instance.CreateCustomer();
        }
    }
}
