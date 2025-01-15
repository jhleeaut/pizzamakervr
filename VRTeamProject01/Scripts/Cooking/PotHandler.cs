using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    public class PotHandler : MonoBehaviour
    {
        #region Component
        #endregion
        #region Var
        [SerializeField]private Groceries putGroceries;
        [SerializeField]int none,
            dough,
            sauce,
            cheese,
            pepperoni,
            onion,
            mushroom,
            pepper,
            tomato;
        #endregion

        public void Init()
        {
            varReset();
        }
        private void varReset()
        {
            putGroceries = Groceries.None;
            none = 0;
            dough = 0;
            sauce = 0;
            cheese = 0;
            pepperoni = 0;
            onion = 0;
            mushroom = 0;
            pepper = 0;
            tomato = 0;
        }
        public void PutGroceriesToPot(Groceries groceries)
        {
            switch (groceries)
            {
                case Groceries.None:
                    none++;
                    break;
                case Groceries.Dough:
                    dough++;
                    break;
                case Groceries.Sauce:
                    sauce++;
                    break;
                case Groceries.Cheese:
                    cheese++;
                    break;
                case Groceries.Pepperoni:
                    pepperoni++;
                    break;
                case Groceries.Onion:
                    onion++;
                    break;
                case Groceries.Mushroom:
                    mushroom++;
                    break;
                case Groceries.Pepper:
                    pepper++;
                    break;
                case Groceries.Tomato:
                    tomato++;
                    break;
            }
            InsidePot(groceries);
        }

        void InsidePot(Groceries groceries)
        {
            putGroceries |= groceries;
        }

        public void Cooking()
        {
            CookingManager.Instance.Cooking(putGroceries, transform);
            varReset();
        }
    }
}