using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTeamProject01
{
    public class TeleportManager : MonoBehaviour
    {
        public static TeleportManager Instance;

        public TeleportPoint[] teleportPoints;

        // Use this for initialization
        public void Init()
        {
            Instance = this;
            teleportPoints = FindObjectsOfType<TeleportPoint>();
        }

        public void Teleport(TeleportName teleportName)
        {
            foreach (TeleportPoint teleportPoint in teleportPoints)
            {
                if (teleportName == teleportPoint.teleportName)
                {
                    GameManager.Instance.Teleport(teleportPoint);
                }
            }
        }
    }
}