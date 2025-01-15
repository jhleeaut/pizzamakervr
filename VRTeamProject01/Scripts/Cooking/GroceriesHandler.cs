using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public enum Groceries
    {
        None = 0,
        Dough = 1 << 0,
        Sauce = 1 << 1,
        Cheese = 1 << 2,
        Pepperoni = 1 << 3,
        Onion = 1 << 4,
        Mushroom = 1 << 5,
        Pepper = 1 << 6,
        Tomato = 1 << 7,
    }

    public class GroceriesHandler : MonoBehaviour
    {
        public Groceries groceries;
        public void Init(Groceries groceries)
        {
            this.groceries = groceries;
        }

        public bool IsGroceries
        {
            get
            {
                if (groceries == Groceries.None)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
