using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        private static GameManager instance = null;

        public static GameManager Instance => instance;

        public PlayerInput playerInput;

        public enum GameState
        {
            Playing,
            Alert,
            Paused,
            GameOver
        }

        public GameState gameState = GameState.Playing;

        public int credit = 0;

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
                    Debug.Log("Alert starting !");
                    StartCoroutine(ResumeToStateIn(GameState.Playing, 5f));
                    break;
                case GameState.Paused:
                    gameState = GameState.Paused;
                    break;
                case GameState.GameOver:
                    gameState = GameState.GameOver;
                    break;
                case GameState.Playing:
                    Debug.Log("Gamestate = Playing !");
                    gameState = GameState.Playing;
                    break;
                default:
                    break;
            }
        }

        IEnumerator ResumeToStateIn(GameState gameState, float durationInSeconds)
        {
            yield return new WaitForSeconds(durationInSeconds);
            Debug.Log("Alert is over !");
            EventManager.OnEndOfAlertState();

            EventManager.OnChangeGameState(gameState);
        }
    }
}
