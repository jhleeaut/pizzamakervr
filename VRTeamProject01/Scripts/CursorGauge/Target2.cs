using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace VRTeamProject01
{
    public class Target2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CursorGauge cursorGauge;
        private MeshRenderer render;
        private void Awake()
        {
            render = GetComponent<MeshRenderer>();
            cursorGauge = FindObjectOfType<CursorGauge>();
        }

        private void Update()
        {
            if(cursorGauge != null)
            {
                if (cursorGauge.IsFull)
                {
                    cursorGauge.ExitGauge();
                    DestroyObject();
                }
            }
        }
        public void ChangeObjectColor()
        {
            render.material.color = new Color(Random.value, Random.value, Random.value);
        }
        public void DestroyObject()
        {
            Destroy(gameObject);
        }
        #region 방법1
        //public override void OnPointerEnter(PointerEventData eventData)
        //{ 
        //    cursorGauge.EnterGauge();
        //}

        //public override void OnPointerExit(PointerEventData eventData)
        //{
        //    cursorGauge.ExitGauge();
        //}
        #endregion
        #region 방법2
        public void OnPointerEnter(PointerEventData eventData)
        {
            cursorGauge.EnterGauge();
            ChangeObjectColor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cursorGauge.ExitGauge();
        }

        #endregion
    }
}