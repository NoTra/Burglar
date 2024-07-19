using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using burglar.environment;
using burglar.items;

namespace burglar.managers
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        private static GameManager instance = null;

        public static GameManager Instance => instance;

        public PlayerInput playerInput;

        public AudioManager audioManager;

        public enum GameState
        {
            Playing,
            Alert,
            Paused,
            GameOver,
            Menu
        }

        public GameState gameState = GameState.Menu;

        public int credit = 0;

        public List<Item> items = new List<Item>();

        public Item selectedItem;

        public LightSwitch _lightSwitchSelectedByRemote;

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
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicMenu, false, false);
            playerInput = GetComponent<PlayerInput>();
            
            // LevelManager.Instance.LoadScene("main");
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
                case GameState.GameOver:
                    gameState = GameState.GameOver;
                    break;
                case GameState.Playing:
                    gameState = GameState.Playing;
                    break;
                case GameState.Menu:
                    gameState = GameState.Menu;
                    break;
                default:
                    break;
            }
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
            
            gameState = (gameState == GameState.Paused) ? GameState.Playing : GameState.Paused;
            Time.timeScale = (gameState == GameState.Paused) ? 0 : 1;

            EventManager.OnTogglePause();
        }
    }
}
