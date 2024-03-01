using UnityEngine;
using UnityEngine.Events;
using System;


namespace burglar
{
    public static class EventManager
    {
        public static event UnityAction<Vector3, float> SoundHeard;
        public static void OnSoundHeard(Vector3 point, float strength) => SoundHeard?.Invoke(point, strength);

        public static event UnityAction<GameObject> PlayerCaught;
        public static void OnPlayerCaught(GameObject player) => PlayerCaught?.Invoke(player);
    }
}
