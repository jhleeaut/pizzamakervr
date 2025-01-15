using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTeamProject01
{
    public enum TeleportName
    {
        Kitchen = 0,
        Counter,
        Oven,
    }
    public class TeleportPoint : MonoBehaviour
    {
        public TeleportName teleportName;
    }
}