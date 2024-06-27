using UnityEngine;
using UnityEngine.Events;
using burglar.environment;
using burglar.items;

namespace burglar.managers
{
    public static class EventManager
    {
        public static event UnityAction<Vector3, float, bool> SoundGenerated;
        public static void OnSoundGenerated(Vector3 point, float strength, bool checkDistance) => SoundGenerated?.Invoke(point, strength, checkDistance);

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

        public static event UnityAction<Safe> FailSafeCrack;
        public static void OnFailSafeCrack(Safe safe) => FailSafeCrack?.Invoke(safe);

        public static event UnityAction<GameManager.GameState> ChangeGameState;
        public static void OnChangeGameState(GameManager.GameState gameState) => ChangeGameState?.Invoke(gameState);

        public static event UnityAction EndOfAlertState;
        public static void OnEndOfAlertState() => EndOfAlertState?.Invoke();

        public static event UnityAction<Item> SelectItem;
        public static void OnSelectItem(Item item) => SelectItem?.Invoke(item);

        public static event UnityAction IsInvisible;
        public static void OnIsInvisible() => IsInvisible?.Invoke();

        public static event UnityAction IsVisible;
        public static void OnIsVisible() => IsVisible?.Invoke();

        public static event UnityAction CreditChanged;
        public static void OnCreditChanged() => CreditChanged?.Invoke();

        public static event UnityAction Save;
        public static void OnSave() => Save?.Invoke();

        public static event UnityAction<Interactible> Interact;
        public static void OnInteract(Interactible interactible) => Interact?.Invoke(interactible);
    }
}
