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

        public Story GetStory() { return story; }
        
        public void SetStory(TextAsset inkFile)
        {
            story = new Story(inkFile.text);

            DialogManager.Instance.SetStory(story);
        }

        private void Start()
        {
            // Spawn player at the start of the game
            var playerGo = Instantiate(_playerPrefab, _currentTuto.GetSpawnPoint().transform.position,
                Quaternion.identity);
            _player = playerGo.GetComponent<Player>();
            _playerInput = _player._playerInput;
            
            // Make freelook camera look at player
            _freeLook.Follow = _player.transform;
            
            try
            {
                if (_currentTuto == null) return;
                
                LoadTuto(_currentTuto);
                
                UIManager.Instance.HUD.SetActive(true);

                StartCoroutine(LaunchTuto(1f));
            } catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
        }

        public IEnumerator LaunchTuto(float delay = 0)
        {
            // Teleport player
            _player.transform.position = _currentTuto.GetSpawnPoint().transform.position;

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