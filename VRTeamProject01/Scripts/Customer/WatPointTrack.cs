using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class WatPointTrack : MonoBehaviour
    {
        public Color lineColor = Color.yellow;
        private Transform[] points;

        private void OnDrawGizmos()
        {
            Gizmos.color = lineColor;
            points = GetComponentsInChildren<Transform>();

            int nextIdx = 1;

            Vector3 currPos = points[nextIdx].position;
            Vector3 nextPos;

            for (int i = 0; i <= points.Length; i++)
            {
                nextPos = (++nextIdx >= points.Length) ? points[points.Length - 1].position : points[nextIdx].position;

                Gizmos.DrawLine(currPos, nextPos);

                currPos = nextPos;
            }
        }
    }
}