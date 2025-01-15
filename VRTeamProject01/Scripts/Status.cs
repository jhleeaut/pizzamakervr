using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{

    [System.Serializable]
    public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

    public class Status : MonoBehaviour
    {

        #region Variables

        [Header("Walk & Run Speed")]
        [SerializeField] private float walkSpeed = 2.0f;

        [SerializeField] private float runSpeed = 2.5f;

        public float WalkSpeed { get { return walkSpeed; } }
        public float RunSpeed { get { return runSpeed; } }

        [Header("Health")]
        [SerializeField] private int maxHP = 100;

        private int currentHP;

        [HideInInspector] public HPEvent onHPEvent = new HPEvent();

        #endregion Variables

        public void Init()
        {
            currentHP = maxHP;
        }

        public void DecreaseHP(int decreaseHP)
        {
            int previouseHP = currentHP;

            if (currentHP - decreaseHP > 0)
            {
                currentHP -= decreaseHP;
            }
            else
            {
                currentHP = 0;
                Debug.Log("사망!");
            }

            onHPEvent.Invoke(previouseHP, currentHP);
        }
    }
}