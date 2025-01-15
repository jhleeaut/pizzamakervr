using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class CookingPizza : MonoBehaviour {
        [SerializeField]PizzaType pizzaType;
        public PizzaType PizzaType
        {
            get
            {
                return pizzaType;
            }
        }
        public void Init()
        {
#if UNITY_EDITOR
            Debug.Log("구워짐");
#endif
            pizzaType = (PizzaType)System.Enum.Parse(typeof(PizzaType), name);
        }
    }
}
