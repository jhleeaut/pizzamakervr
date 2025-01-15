using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTeamProject01
{

    public class RotateToMouse : MonoBehaviour
    {

        #region Variables

        private float rotateSpeedX = 3;
        private float rotateSpeedY = 5;
        private float limitMinX = -80;
        private float limitMaxX = 50;
        private float eulerAngleX;
        private float eulerAngleY;
        private Quaternion rotation;

        #endregion Variables

        public void UpdateRotate(float mouseX, float mouseY, float RotationRatchet)
        {

            eulerAngleY += mouseX * rotateSpeedX;
            eulerAngleX -= mouseY * rotateSpeedY;

            eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
            eulerAngleY += RotationRatchet;

            rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
            transform.rotation = rotation;
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360) angle += 360;
            if (angle > 360) angle -= 360;

            return Mathf.Clamp(angle, min, max);
        }
    }
}