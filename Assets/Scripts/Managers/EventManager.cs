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

        public static event UnityAction<GameObject> LightChange;
        public static void OnLightChange(GameObject switchGO) => LightChange?.Invoke(switchGO);

        public static event UnityAction<int> CreditCollected;
        public static void OnCreditCollected(int amount) => CreditCollected?.Invoke(amount);

        public static event UnityAction<Safe> OpenSafe;
        public static void OnOpenSafe(Safe safe) => OpenSafe?.Invoke(safe);

        public static event UnityAction<Safe> SuccessSafeCrack;
        public static void OnSuccessSafeCrack(Safe safe) => SuccessSafeCrack?.Invoke(safe);

        public static event UnityAction FailSafeCrack;
        public static void OnFailSafeCrack() => FailSafeCrack?.Invoke();
    }
}
