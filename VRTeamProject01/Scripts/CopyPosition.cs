﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class CopyPosition : MonoBehaviour
    {
        public bool x, y, z = false;
        public Transform target = null;
        
        void Update()
        {
            if (target)
            {
                transform.position = new Vector3(
                    (x ? target.position.x : transform.position.x),
                    (y ? target.position.y : transform.position.y),
                    (z ? target.position.z : transform.position.z));
            }
        }
    }
}