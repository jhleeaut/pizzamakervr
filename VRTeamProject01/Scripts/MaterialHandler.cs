using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    public class MaterialHandler : MonoBehaviour
    {

        [SerializeField]
        private List<Renderer> m_renderers = new List<Renderer>();
        public List<Color> materialColors = new List<Color>();

        // Use this for initialization
        public void Init()
        {
            if (m_renderers.Count == 0)
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    Renderer renderer = renderers[i];
                    m_renderers.Add(renderer);
                    for (int j = 0; j < renderer.materials.Length; j++)
                    {
                        Material material = renderer.materials[j];
                        materialColors.Add(material.color);
                    }
                }
            }
        }

        public void MaterialColorChange(Color color)
        {
            for (int i = 0; i < m_renderers.Count; i++)
            {
                Renderer renderer = m_renderers[i];
                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    Material material = renderer.materials[j];
                    material.color = color;
                }
            }
        }
        public void MaterialColorBack()
        {
            for (int i = 0; i < m_renderers.Count; i++)
            {
                Renderer renderer = m_renderers[i];
                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    Material material = renderer.materials[j];
                    material.color = materialColors[i];
                }
            }
        }
    }
}