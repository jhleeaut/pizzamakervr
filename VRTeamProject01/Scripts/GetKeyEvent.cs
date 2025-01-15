using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class GetKeyEvent : MonoBehaviour
    {
        /// <summary>
        /// 컨트롤키 눌렀는지 확인
        /// </summary>
        public static bool isPossibleGrabbableRotate;
        // Use this for initialization

        /// <summary>
        /// 우클릭 눌렀는지 확인
        /// </summary>
        public static bool isPossibleRotate;

        
        // Update is called once per frame
        public void Updated()
        {
            if(Input.GetButtonDown("GrabbableRotate")){
                isPossibleGrabbableRotate = true;
            }
            if (Input.GetButtonUp("GrabbableRotate"))
            {
                isPossibleGrabbableRotate = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                isPossibleRotate = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isPossibleRotate = false;
            }
        }
    }
}