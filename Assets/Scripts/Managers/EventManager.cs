using UnityEngine;
using UnityEngine.Events;
using System;


namespace burglar
{
    public static class EventManager
    {
        public static event UnityAction<Vector3> SuspectedPoint;
        public static void OnSuspectedPoint(Vector3 point) => SuspectedPoint?.Invoke(point);

        public static event UnityAction<GameObject> PlayerCaught;
        public static void OnPlayerCaught(GameObject player) => PlayerCaught?.Invoke(player);
    }
}
