using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class TeleportButton : MonoBehaviour
    {
        public TeleportName teleportName;
        public Animator buttonAnimator;

        private void Awake()
        {
            buttonAnimator = GetComponentInParent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            buttonAnimator.SetTrigger("isPush");
        }
    }
}