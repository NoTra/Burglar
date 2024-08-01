using UnityEngine;
using burglar.player;
using Ink.Runtime;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using burglar.tutos;
using Cinemachine;


namespace burglar.managers
{
    public class TutoManager : MonoBehaviour
    {
        public static TutoManager Instance { get; private set; }
        
        public GameObject teleportEffectPrefab;

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
        [HideInInspector] public Tuto _previousTuto;
        [SerializeField] private GameObject _playerPrefab;
        [HideInInspector] public Player _player;
        public CinemachineFreeLook _freeLook;

        private PlayerInput _playerInput;

        public Story story;

        private void OnEnable()
        {
            Debug.Log("OnEnable (TutoManager)");
            EventManager.CinematicEnd += StartTuto;
        }
        
        private void OnDisable()
        {
            EventManager.CinematicEnd -= StartTuto;
        }

        public Story GetStory() { return story; }
        
        public void SetStory(TextAsset inkFile)
        {
            story = new Story(inkFile.text);

            DialogManager.Instance.SetStory(story);
        }

        private void Start()
        {
            Debug.Log("OnStart (TutoManager)");
            _player = GameManager.Instance.player;
            _playerInput = _player._playerInput;
        }

        public void StartTuto()
        {
            Debug.Log("StartTuto");
            
            try
            {
                if (_currentTuto == null) return;
                
                LoadTuto(_currentTuto);
                
                UIManager.Instance.UITitle.SetActive(true);
                // Instantiate(UIManager.Instance.titlePrefab, UIManager.Instance.GetCanvasGO().transform);
                
                if (_player.transform.position != _currentTuto.GetSpawnPoint().transform.position)
                {
                    StartCoroutine(Tuto.TeleportPlayerToNewPosition(_currentTuto.GetSpawnPoint().transform.position));
                    _currentTuto.OnEnter();
                }
                
                UIManager.Instance.ToggleHudVisibility();
                
                StartCoroutine(LaunchTuto(1f));
            } catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
        }

        public IEnumerator LaunchTuto(float delay = 0)
        {
            if (_currentTuto.GetSpawnPoint().transform.position != _player.transform.position)
            {
                // Teleport player
                _player.transform.position = _currentTuto.GetSpawnPoint().transform.position;
            }

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            
            yield return StartCoroutine(DialogManager.Instance.StartDialog());
            
            _currentTuto.OnEnter();
        }

        public void LoadTuto(Tuto currentTuto)
        {
            _currentTuto = currentTuto;

            if (_currentTuto && _currentTuto.inkFileStart is not null)
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
            _previousTuto = _currentTuto;
            
            SetStory(_currentTuto.inkFileEnd);
            
            _currentTuto = null;
        }

        public Tuto GetCurrentTuto()
        {
            return _currentTuto;
        }
    }
}