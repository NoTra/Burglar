using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using burglar.environment;
using burglar.items;
using burglar.player;
using Cinemachine;

namespace burglar.managers
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        private static GameManager instance = null;

        public static GameManager Instance => instance;

        public Player player;
        public PlayerInput playerInput;

        public AudioManager audioManager;

        private List<GameObject> _wallsBlockingView = new (); 

        public enum GameState
        {
            Playing,
            Alert,
            Paused,
            GameOver,
            Menu,
            Caught
        }

        public GameState gameState = GameState.Menu;

        public int credit = 0;

        public List<Item> items = new();

        public Item selectedItem;

        public LightSwitch _lightSwitchSelectedByRemote;
        
        public CinemachineBrain _cinemachineBrain;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            EventManager.ChangeGameState += (state) => OnChangeGameState(state);
        }

        private void OnDisable()
        {
            EventManager.ChangeGameState -= (state) => OnChangeGameState(state);
        }

        private void OnChangeGameState(GameState state)
        {
            if (state == gameState)
            {
                return;
            }

            switch (state)
            {
                case GameState.Alert:
                    gameState = GameState.Alert;
                    StartCoroutine(ResumeToStateIn(GameState.Playing, 5f));
                    break;
                case GameState.Paused:
                    gameState = GameState.Paused;
                    break;
                case GameState.Playing:
                    gameState = GameState.Playing;
                    break;
                case GameState.Menu:
                    gameState = GameState.Menu;
                    break;
                case GameState.Caught:
                    gameState = GameState.Caught;
                    break;
                default:
                    break;
            }
        }
        
        public void SetGameState(GameState state)
        {
            EventManager.OnChangeGameState(state);
        }

        IEnumerator ResumeToStateIn(GameState gameState, float durationInSeconds)
        {
            yield return new WaitForSeconds(durationInSeconds);
            
            EventManager.OnEndOfAlertState();
            EventManager.OnChangeGameState(gameState);
        }

        public string GetSelectedItemSlug()
        {
            return selectedItem?.slug;
        }

        public void TogglePause()
        {
            if (
                gameState != GameState.Playing && 
                gameState != GameState.Alert && 
                gameState != GameState.Paused
            ) { 
                return;
            }
            
            EventManager.OnTogglePause();
            
            if (
                UIManager.Instance.PauseScreen.activeSelf && 
                UIManager.Instance.PauseScreen.GetComponent<PauseScreenManager>().GetSettingsCanvas().gameObject.activeSelf
            ) {
                return;
            }
            
            gameState = (gameState == GameState.Paused) ? GameState.Playing : GameState.Paused;
            Time.timeScale = (gameState == GameState.Paused) ? 0 : 1;

            EventManager.OnTimeScaleChanged();
        }

        public void SwitchCameraToTopRig()
        {
            StartCoroutine(TransitionBetweenRig(GetActiveCamera(), 2f, 1f));
        }
        
        public void SwitchCameraToBottomRig()
        {
            StartCoroutine(TransitionBetweenRig(GetActiveCamera(), 2f, 0f));
        }

        public ICinemachineCamera GetActiveCamera()
        {
            if (_cinemachineBrain == null)
            {
                _cinemachineBrain = FindObjectOfType<CinemachineBrain>();
            }
            
            return _cinemachineBrain.ActiveVirtualCamera;
        }

        private IEnumerator TransitionBetweenRig(ICinemachineCamera cineCamera, float duration, float targetYValue)
        {
            var freeLookCamera = cineCamera as CinemachineFreeLook;
            if (freeLookCamera)
            {
                var elapsedTime = 0f;
                var startValue = freeLookCamera.m_YAxis.Value;
                
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    freeLookCamera.m_YAxis.Value = Mathf.Clamp01(Mathf.Lerp(startValue, targetYValue, elapsedTime / duration));

                    if (targetYValue == 0f)
                    {
                        // Draw a ray from camera to player
                        var ray = new Ray(freeLookCamera.transform.position,
                            player.transform.position - freeLookCamera.transform.position);
                        // If the ray hits something, make it disappear and store it in the variable hit
                        if (Physics.Raycast(ray, out var hit, 100f))
                        {
                            // If the object hit is the player, break the loop
                            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                            {
                                break;
                            }

                            // If the object hit is a wall, make it disappear
                            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                            {
                                hit.collider.gameObject.SetActive(false);
                                _wallsBlockingView.Add(hit.collider.gameObject);
                            }
                        }
                    }
                    else
                    {
                        _wallsBlockingView.ForEach(
                            (wall) =>
                            {
                                StartCoroutine(WaitForSecondsAndDo(1f, () =>
                                {
                                    wall.SetActive(true);
                                }));

                            }
                            );
                        _wallsBlockingView.Clear();
                    }

                    yield return null;
                }
                
                freeLookCamera.m_YAxis.Value = targetYValue;
            }
        }
        
        private IEnumerator WaitForSecondsAndDo(float v, Action action)
        {
            yield return new WaitForSeconds(v);
            action();
        }
    }
}
