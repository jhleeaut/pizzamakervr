using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class VelocityDebugger : MonoBehaviour
    {
        [SerializeField]
        private float maxVelocity = 20f;

        public void Updated()
        {
            GetComponent<Renderer>().material.color = ColorForVelocity();
        }

        private Color ColorForVelocity()
        {
            float velocity = GetComponent<Rigidbody>().velocity.magnitude;

            return Color.Lerp(Color.green, Color.red, velocity / maxVelocity);
        }
    }
}