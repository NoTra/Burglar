using UnityEngine;
using burglar.player;
using Ink.Runtime;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using burglar.tutos;


namespace burglar.managers
{
    public class TutoManager : MonoBehaviour
    {
        public static TutoManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        [SerializeField] private Tuto _currentTuto;
        public Player _player;

        private PlayerInput _playerInput;

        public static Story story;

        public Story GetStory() { return story; }

        private void Start()
        {
            _playerInput = _player._playerInput;

            try
            {
                if (_currentTuto != null)
                {
                    LoadTuto(_currentTuto);

                    StartCoroutine(LaunchTuto());
                }
            } catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
        }

        public IEnumerator LaunchTuto()
        {
            // Teleport player
            _player.transform.position = _currentTuto.GetSpawnPoint().transform.position;

            Debug.Log("Start Dialog new zone");
            yield return StartCoroutine(DialogManager.Instance.StartDialog());
        }

        public IEnumerator EndTuto()
        {
            var nextTuto = _currentTuto.GetNextTuto();
            UnloadTuto();

            Debug.Log("Start end dialog");
            yield return StartCoroutine(DialogManager.Instance.StartDialog());

            LoadTuto(nextTuto);
            Debug.Log("Launch new zone");
            yield return StartCoroutine(LaunchTuto());
        }

        public void LoadTuto(Tuto currentTuto)
        {
            _currentTuto = currentTuto;

            if (_currentTuto.inkFileStart != null)
            {
                SetStory(_currentTuto.inkFileStart);
            }
            else
            {
                throw new Exception("No inkFileStart for current tuto.");
            }
            
        }

        public void UnloadTuto()
        {
            SetStory(_currentTuto.inkFileEnd);

            _currentTuto = null;
        }

        public void SetStory(TextAsset inkFile)
        {
            story = new Story(inkFile.text);

            DialogManager.Instance.SetStory(story);
        }

        /*public void TeleportPlayerToNextTuto()
        {
            Debug.Log("Teleport player to next position");
            _player.transform.position = _currentTuto.GetNextTuto().GetSpawnPoint().transform.position;
        }*/
    }
}