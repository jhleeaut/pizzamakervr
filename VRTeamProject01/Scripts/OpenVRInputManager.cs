using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if OpenVR
using Valve.VR;    // 기능 쓰기 위해
#endif

namespace VRTeamProject01
{
    /// <summary>
    /// MainCamera 컴포넌트로 있음 triggerAction 등을 세팅해야함
    /// </summary>
    public class OpenVRInputManager : MonoBehaviour
    {
#if OpenVR
        //컨트롤러 선택
        private SteamVR_Input_Sources any = SteamVR_Input_Sources.Any;              // 양쪽 다적용
        private SteamVR_Input_Sources leftHand = SteamVR_Input_Sources.LeftHand;    // 왼쪽만
        private SteamVR_Input_Sources rightHand = SteamVR_Input_Sources.RightHand;  // 오른쪽만

        //입력 참or거짓으로 받을떄
        public SteamVR_Action_Boolean triggerAction;         // InteractUI선택 - 트리거 - 검지로 딸깍하는 부분
        public SteamVR_Action_Boolean teleportAction;       // Teleport선택    - 터치패드버튼 - 동그란부분 클릭
        //public SteamVR_Action_Boolean touchpadAction;     // TouchPad선택   - 터치패드 - 전체적으로
        public SteamVR_Action_Boolean grabgripAction;      // GrabGrip선택   - 그랩그림 - 중지쪽에 버튼이 있음
        public bool isTriggerActionDown
        {
            get
            {
                Debug.Log(triggerAction.GetStateDown(any));
                // 인스펙터에서 triggerAction등을 채워야 에러안남
                return triggerAction.GetStateDown(any);
            }
        }
        public bool isTeleportActionDown
        {
            get
            {
                return teleportAction.GetStateDown(any);
            }
        }
        public void Updated()
        {
            //트리거버튼 - 클릭 - 누루고 있는 동안 계속 호출 - 어려운맛
            //내가 누루고 있는데 터치인데 ( 양쪽중 하나라면 )
            if (SteamVR_Input._default.inActions.InteractUI.GetState(SteamVR_Input_Sources.Any))
            {
                Debug.Log("트리거 버튼 클릭 - Full");
                //Debug.Log("Trigger Button Click - Full");
            }

            //트리거버튼 - 쉬운맛
            if (triggerAction.GetState(any))
            {
                Debug.Log("트리거 버튼 클릭 - Simple");
                //Debug.Log("Trigger BUtton Click - Simple");
            }
            //트리거버튼 - 쉬운맛
            if (triggerAction.GetStateDown(any))
            {
                Debug.Log("트리거 버튼 다운 - Simple");
                //Debug.Log("Trigger BUtton Click - Simple");
            }
            //-----------------------------------------------------------------------------------
            //텔레포트버튼 - 다운 or 업 - 한번 호출 - 어려운맛
            if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                Debug.Log("왼손 텔레포트 다운 - Full");
                //Debug.Log("Left Hand Teleport Down - Full");
            }

            //텔레포트버튼 - 쉬운맛
            if (teleportAction.GetStateUp(leftHand))
            {
                Debug.Log("왼손 텔레포트 업 - Simple");
                //Debug.Log("Left Hand Teleport Up - Simple");
            }

            //-----------------------------------------------------------------------------------
            /*
            //터치패드 - 어려운맛
            if (SteamVR_Input._default.inActions.TouchPad.GetState(SteamVR_Input_Sources.Any))
            {
                Vector2 pos = SteamVR_Input._default.inActions.TouchPosition.GetAxis(SteamVR_Input_Sources.Any);
                Debug.Log("TouchPading - Full");
            }
            */


            //터치패드 - 쉬운맛

            //bool isTouchPad = touchpadAction.GetStateUp(any);   //터치값 들어 왔는지 확인하고
            /*
            if (isTouchPad) //터치중이라면
            {
                Vector2 pos = SteamVR_Input._default.inActions.TouchPosition.GetAxis(any);  //XY값 받기
                Debug.Log("TouchPad Up - Simple");
            }
            */

            //-----------------------------------------------------------------------------------

            //그랩그립 - 어려운맛
            if (SteamVR_Input._default.inActions.GrabGrip.GetState(SteamVR_Input_Sources.Any))
            {
                Debug.Log("그랩그립 - Full");
                //Debug.Log("GrabGrip - Full");
            }

            //그랩그립 - 쉬운맛
            if (grabgripAction.GetState(any))
            {
                Debug.Log("그랩그립 - Simple");
                //Debug.Log("GrabGrip - Simple");
            }

        }
#endif
    }
}