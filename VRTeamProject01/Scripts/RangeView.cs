using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    public class RangeView : MonoBehaviour
    {
        public Transform grabberPoint;
        float rangeSize;

        public void Updated()
        {
            rangeSize = grabberPoint.localPosition.z * 2;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
            Gizmos.DrawCube(transform.position, new Vector3(rangeSize, rangeSize, rangeSize));
        }
    }
}