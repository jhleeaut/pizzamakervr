using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
namespace VRTeamProject01
{
    public enum PizzaType
    {
        Pizza,
        CheesePizza,
        MushroomPizza,
        Pepper_OnionPizza,
        PepperoniPizza,
        Tomato_PeperoniPizza,
        Tomato_PepperPizza,
        TomatoPizza,
    }
    public class OrderHandler : MonoBehaviour
    {
        #region Component
        public GameObject canvas_OrderObj;
        public Text text_Order;
        #endregion
        #region Var
        public PizzaType[] pizzaOrder;
        [SerializeField]private PizzaType orderPizzaType;
        public PizzaType OrderPizzaType
        {
            get
            {
                return orderPizzaType;
            }
        }
        #endregion
	    public void Init () {
            pizzaOrder = System.Enum.GetValues(typeof(PizzaType)) as PizzaType[];
            orderPizzaType = pizzaOrder[Random.Range(1, pizzaOrder.Length)];
            Order();
        }

        void Order()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PizzaTypeString(orderPizzaType));
            sb.Append(" 주세요");
            text_Order.text = sb.ToString();
        }

        public void ReceiveOrder()
        {
            canvas_OrderObj.SetActive(false);
        }

        string PizzaTypeString(PizzaType pizzaType)
        {
            string pizzaTypeString = string.Empty;
            switch (pizzaType)
            {
                case PizzaType.CheesePizza:
                    pizzaTypeString = "치즈피자";
                    break;
                case PizzaType.MushroomPizza:
                    pizzaTypeString = "버섯피자";
                    break;
                case PizzaType.Pepper_OnionPizza:
                    pizzaTypeString = "페퍼오니온피자";
                    break;
                case PizzaType.PepperoniPizza:
                    pizzaTypeString = "페페로니피자";
                    break;
                case PizzaType.Tomato_PeperoniPizza:
                    pizzaTypeString = "페페로니토마토피자";
                    break;
                case PizzaType.Tomato_PepperPizza:
                    pizzaTypeString = "페퍼토마토피자";
                    break;
                case PizzaType.TomatoPizza:
                    pizzaTypeString = "토마토피자";
                    break;
                default:
                    break;
            }
            return pizzaTypeString;
        }

        public void OrderMatching(bool isOrderMatching)
        {
            MoveCtrl moveCtrl = GetComponent<MoveCtrl>();
            if (isOrderMatching)
            {
                moveCtrl.AnimatorSetTrigger("islaugh");
            }
            else
            {
                moveCtrl.AnimatorSetTrigger("isTakn");
            }
            moveCtrl.IsGetPizza = true;
        }


        //박스포장이 끝나고 물건받는곳에 서있을때
        private void OnTriggerEnter(Collider other)
        {
            PizzaBoxHandler pizzaBoxHandler = other.GetComponent<PizzaBoxHandler>() ?? other.GetComponentInParent<PizzaBoxHandler>();
            if (pizzaBoxHandler == null)
            {
                return;
            }
            
            if (pizzaBoxHandler.IsOrderComplete)
            {
                //물건받을때 딱한번하고 false로 바꿈
                pizzaBoxHandler.IsOrderComplete = false;
                pizzaBoxHandler.OrderComplete(this);
            }
        }

        //박스포장이 끝나고 물건받는곳에 서있을때
        private void OnTriggerStay(Collider other)
        {
            PizzaBoxHandler pizzaBoxHandler = other.GetComponent<PizzaBoxHandler>() ?? other.GetComponentInParent<PizzaBoxHandler>();
            if (pizzaBoxHandler == null)
            {
                return;
            }
            
            if (pizzaBoxHandler.IsOrderComplete)
            {
                //물건받을때 딱한번하고 false로 바꿈
                pizzaBoxHandler.IsOrderComplete = false;
                pizzaBoxHandler.OrderComplete(this);
            }
        }
    }
}
