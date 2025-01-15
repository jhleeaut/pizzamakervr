using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTeamProject01
{

    public class HandRotateToMouse : MonoBehaviour
    {

        #region Variables

        private float rotateSpeedX = 3;
        private float rotateSpeedZ = 5;
        private float limitMinX = -180;
        private float limitMaxX = 180;
        private float eulerAngleX;
        private float eulerAngleZ;
        private Quaternion rotation;

        #endregion Variables

        public void UpdateRotate(float mouseX, float mouseY)
        {

            eulerAngleZ += mouseX * rotateSpeedX;
            eulerAngleX -= mouseY * rotateSpeedZ;

            //마우스 위아래 휘두를때만 위로 360도 아래로 360도에 걸리고
            //마우스 좌우 휘두를때는 무한대로
            eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
            //eulerAngleZ = ClampAngle(eulerAngleZ, limitMinX, limitMaxX);
            //eulerAngleZ += RotationRatchet;

            rotation = Quaternion.Euler(eulerAngleX, 0, eulerAngleZ);
            transform.rotation = rotation;
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;

            return Mathf.Clamp(angle, min, max);
        }
    }
}