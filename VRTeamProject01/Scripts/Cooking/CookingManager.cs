using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class CookingManager : MonoBehaviour
    {

        public static CookingManager Instance;
        [SerializeField]private Dictionary<string, GameObject> cookingPrefabs = new Dictionary<string, GameObject>();
        public void Init()
        {
            Instance = this;
            cookingPrefabs.Add(
                "Pizza",
                Resources.Load("Prefabs/CheesePizza") as GameObject
                );
            cookingPrefabs.Add(
                "CheesePizza", 
                Resources.Load("Prefabs/CheesePizza") as GameObject
                );
            cookingPrefabs.Add(
                "MushroomPizza",
                Resources.Load("Prefabs/MushroomPizza") as GameObject
                );
            cookingPrefabs.Add(
                "Pepper_OnionPizza",
                Resources.Load("Prefabs/Pepper_OnionPizza") as GameObject
                );
            cookingPrefabs.Add(
                "PepperoniPizza",
                Resources.Load("Prefabs/PepperoniPizza") as GameObject
                );
            cookingPrefabs.Add(
                "Tomato_PeperoniPizza",
                Resources.Load("Prefabs/Tomato_PeperoniPizza") as GameObject
                );
            cookingPrefabs.Add(
                "Tomato_PepperPizza",
                Resources.Load("Prefabs/Tomato_PepperPizza") as GameObject
                );
            cookingPrefabs.Add(
                "TomatoPizza",
                Resources.Load("Prefabs/TomatoPizza") as GameObject
                );
#if UNITY_EDITOR
            print(cookingPrefabs.Count);
#endif
        }

        public void Cooking(Groceries putGroceries, Transform transform)
        {
            GameObject copyModel = null;
            string cookingFoodName = string.Empty;
            switch (putGroceries)
            {
                case Groceries.None:
                    break;
                case Groceries.Dough | Groceries.Sauce | Groceries.Cheese:
                    cookingFoodName = "CheesePizza";
                    break;
                case Groceries.Dough | Groceries.Sauce | Groceries.Cheese | Groceries.Mushroom:
                    cookingFoodName = "MushroomPizza";
                    break;
                case Groceries.Dough | Groceries.Sauce | Groceries.Cheese | Groceries.Onion | Groceries.Pepper:
                    cookingFoodName = "Pepper_OnionPizza";
                    break;
                case Groceries.Dough | Groceries.Sauce | Groceries.Cheese | Groceries.Pepperoni:
                    cookingFoodName = "PepperoniPizza";
                    break;
                case Groceries.Dough | Groceries.Sauce | Groceries.Cheese | Groceries.Pepperoni | Groceries.Tomato:
                    cookingFoodName = "Tomato_PeperoniPizza";
                    break;
                case Groceries.Dough | Groceries.Sauce | Groceries.Cheese | Groceries.Pepper | Groceries.Tomato:
                    cookingFoodName = "Tomato_PepperPizza";
                    break;
                case Groceries.Dough | Groceries.Sauce | Groceries.Cheese | Groceries.Tomato:
                    cookingFoodName = "TomatoPizza";
                    break;
                //case Groceries.Dough | Groceries.Sauce | Groceries.Pepperoni:
                //    cookingFoodName = "PepperoniPizza2";
                //    break;
                //case Groceries.Dough | Groceries.Cheese | Groceries.Pepperoni:
                //    cookingFoodName = "PepperoniPizza3";
                //    break;
                default:
                    cookingFoodName = "Pizza";
                    break;
            }
            if (cookingPrefabs.ContainsKey(cookingFoodName))
            {
                copyModel = cookingPrefabs[cookingFoodName];
            }

            if (copyModel != null)
            {
                GameObject cookingFood = Instantiate(copyModel);
                cookingFood.name = cookingFoodName; // Clone 때기
                cookingFood.transform.position = transform.position;
                //cookingFood.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                GameManager.Instance.InitAddGrabbable(cookingFood.GetComponent<Grabbable>());
                AudioManager.Instance.PlayEffect("온갖알람소리", 2.0f, 1.0f);
            }
        }
    }
}
