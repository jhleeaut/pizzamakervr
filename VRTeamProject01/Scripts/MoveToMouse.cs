using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class MoveToMouse : MonoBehaviour
    {

        private float moveSpeedX = 20.0f;
        private float moveSpeedY = 20.0f;
        //private float limitMinX = 0.0f;
        //private float limitMaxX = 1024;
        //private float limitMinY = 0.0f;
        //private float limitMaxY = 768;
        private float positionX;
        private float positionY;
        private Vector3 position;

        public void Init()
        {
            position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            positionX = position.x;
            positionY = position.y;
        }

        public void UpdateMove(float mouseX, float mouseY)
        {
            positionX += mouseX * moveSpeedX;
            positionY += mouseY * moveSpeedY;

            //positionX = ClampMove(positionX, limitMinX, limitMaxX);
            //positionY = ClampMove(positionY, limitMinY, limitMaxY);

            position = new Vector3(positionX, positionY, transform.position.z);
            transform.position = position;
        }

        private float ClampMove(float position, float min, float max)
        {
            return Mathf.Clamp(position, min, max);
        }
    }
}