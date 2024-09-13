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
        public static void OnPlayerCaught(GameObject agent)
        {
            if (GameManager.Instance.gameState == GameManager.GameState.Caught)
            {
                return;
            }
            
            GameManager.Instance.SetGameState(GameManager.GameState.Caught);
            // Debug.Log("Player caught triggered !");
            PlayerCaught?.Invoke(agent);
        }

        public static event UnityAction<GameObject> LightChange;
        public static void OnLightChange(GameObject switchGO) => LightChange?.Invoke(switchGO);

        #region Credit
        public static event UnityAction<int> CreditCollected;
        public static void OnCreditCollected(int amount)
        {
            Debug.Log("CreditCollected triggered");
            CreditCollected?.Invoke(amount);
        }

        public static event UnityAction CreditChanged;
        public static void OnCreditChanged() => CreditChanged?.Invoke();
        
        #endregion

        #region Safe
        public static event UnityAction<Safe> OpenSafe;
        public static void OnOpenSafe(Safe safe) => OpenSafe?.Invoke(safe);

        public static event UnityAction<Safe> SuccessSafeCrack;
        public static void OnSuccessSafeCrack(Safe safe) => SuccessSafeCrack?.Invoke(safe);

        public static event UnityAction<Safe> FailSafeCrack;
        public static void OnFailSafeCrack(Safe safe) => FailSafeCrack?.Invoke(safe);

        public static event UnityAction CloseSafe;
        public static void OnCloseSafe() => CloseSafe?.Invoke();
        
        #endregion Safe
        
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

        public static event UnityAction Save;
        public static void OnSave() => Save?.Invoke();

        public static event UnityAction<Interactible> Interact;
        public static void OnInteract(Interactible interactible) => Interact?.Invoke(interactible);
        
        public static event UnityAction<Interactible> EnterInteractibleArea;
        public static void OnEnterInteractibleArea(Interactible interactible) => EnterInteractibleArea?.Invoke(interactible);
        
        public static event UnityAction<Interactible> ExitInteractibleArea;
        public static void OnExitInteractibleArea(Interactible interactible) => ExitInteractibleArea?.Invoke(interactible);

        public static event UnityAction TutoSuccess;
        public static void OnTutoSuccess() => TutoSuccess?.Invoke();
        
        public static event UnityAction DialogStart;
        public static void OnDialogStart() => DialogStart?.Invoke();
        
        public static event UnityAction DialogEnd;
        public static void OnDialogEnd() => DialogEnd?.Invoke();
        
        public static event UnityAction<UserWaypoint> EnterUserWaypoint;
        public static void OnEnterUserWaypoint(UserWaypoint userWaypoint) => EnterUserWaypoint?.Invoke(userWaypoint);
        
        public static event UnityAction<UserWaypoint> ExitUserWaypoint;
        public static void OnExitUserWaypoint(UserWaypoint userWaypoint) => ExitUserWaypoint?.Invoke(userWaypoint);
        
        public static event UnityAction<Checkpoint> CheckpointReached;
        public static void OnCheckpointReached(Checkpoint checkpoint) => CheckpointReached?.Invoke(checkpoint);
        
        public static event UnityAction TogglePause;
        public static void OnTogglePause() => TogglePause?.Invoke();

        public static event UnityAction LoadLevelStart;
        public static void OnLoadLevelStart()
        {
            Debug.Log("LoadLevelStart triggered");
            LoadLevelStart?.Invoke();
        }

        public static event UnityAction LoadLevelEnd;
        public static void OnLoadLevelEnd()
        {
            Debug.Log("LoadLevelEnd triggered");
            LoadLevelEnd?.Invoke();
        }

        public static event UnityAction<Objective> ObjectiveCompleted;
        public static void OnObjectiveCompleted(Objective objective) => ObjectiveCompleted?.Invoke(objective);
        
        public static event UnityAction TimeScaleChanged;
        public static void OnTimeScaleChanged() => TimeScaleChanged?.Invoke();

        public static event UnityAction ObjectiveLoaded;
        public static void OnObjectiveLoaded() => ObjectiveLoaded?.Invoke();
        
        public static event UnityAction CinematicStart;

        public static void OnCinematicStart()
        {
            Debug.Log("CinematicStart triggered");
            CinematicStart?.Invoke();
        }
        
        public static event UnityAction CinematicEnd;
        public static void OnCinematicEnd()
        {
            Debug.Log("CinematicEnd triggered");
            CinematicEnd?.Invoke();
        }

        public static event UnityAction LevelSuccess;
        public static void OnLevelSuccess() => LevelSuccess?.Invoke();

        public static event UnityAction UpdateObjectives;
        public static void OnUpdateObjectives() => UpdateObjectives?.Invoke();
    }
}
