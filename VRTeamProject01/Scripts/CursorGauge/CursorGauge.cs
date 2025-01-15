using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VRTeamProject01
{
    public class CursorGauge : MonoBehaviour
    {

        public Image CursorGaugeImage;

        private bool fillOn;
        [SerializeField]private float fillAmount;
        private float prevFillAmount;

        [Range(1, 10)]
        public int fillSpeed = 1;

        public bool IsFull
        {
            private set; get;
        }

        void Start()
        {
            ExitGauge();
        }

        void Update()
        {
            if (!fillOn)
                return;

            if (fillAmount < 1.0f)
                fillAmount += 0.5f * fillSpeed * Time.deltaTime;
            else
                IsFull = true;

            if (fillAmount != prevFillAmount)
            {
                prevFillAmount = fillAmount;
                CursorGaugeImage.fillAmount = fillAmount;
            }
        }
    
        public void EnterGauge()
        {
            fillOn = true;
        }

        public void ExitGauge()
        {
            fillAmount = 0.0f;
            CursorGaugeImage.fillAmount = fillAmount;
            fillOn = false;
            IsFull = false;

            prevFillAmount = fillAmount;
        }
    }
}
