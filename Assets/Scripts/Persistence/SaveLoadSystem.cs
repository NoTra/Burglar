using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace burglar.persistence
{
    [Serializable] public class GameData
    {
        public string Name;
        public int CurrentLevelId;
        public List<Item> Items;
        public int Credit;
        public DateTime SavedAt;
    }

    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
    {
        [SerializeField] public GameData gameData;

        IDataService dataService;

        protected override void Awake()
        {
            base.Awake();
            dataService = new FileDataService(new JsonSerializer());
        }

        public void NewGame()
        {
            gameData = new GameData {
                Name = "My save",
                CurrentLevelId = 0,
                Items = new List<Item>(),
                Credit = 0,
                SavedAt = DateTime.Now
            };

            LevelManager.Instance.LoadScene("level1");
        }

        public void SaveGame()
        {
            List<Item> items = new List<Item>();

            foreach (var item in GameManager.Instance.items)
            {
                // Find item by slug
                Debug.Log(item.title);
            }

            // 
            gameData.Items = GameManager.Instance.items;
            gameData.Credit = GameManager.Instance.credit;
            gameData.SavedAt = DateTime.Now;

            dataService.Save(gameData);
        }

        public void LoadGame(string gameName)
        {
            gameData = dataService.Load(gameName);

            if (gameData.CurrentLevelId < 0)
            {
                gameData.CurrentLevelId = 0;
            }

            GameManager.Instance.items = gameData.Items;

            Debug.Log("Items loaded: " + gameData.Items.Count);

            foreach (var item in gameData.Items)
            {
                // Find item by slug
                Debug.Log(item);
            }

            GameManager.Instance.credit = gameData.Credit;

            var buildIndex = LevelManager.Instance.levels[gameData.CurrentLevelId];

            LevelManager.Instance.LoadScene(buildIndex);
        }

        public void DeleteGame(string gameName) => dataService.Delete(gameName);

        public void ReloadGame() => LoadGame(gameData.Name);

        public bool SaveExists() {
            var name = String.IsNullOrEmpty(gameData.Name) ? "My save" : gameData.Name;
            return dataService.SaveExists(name);
        }

        public string GetLastSaveName()
        {
            // Get list of all save names
            var saveNames = dataService.ListSaves().ToList();

            // If there are no saves, return false
            if (saveNames.Count == 0)
            {
                return null;
            }

            // Get the last save name
            var lastSaveName = saveNames.Last();

            return lastSaveName;
        }

        private void OnEnable()
        {
            EventManager.Save += SaveGame;
        }
        private void OnDisable()
        {
               EventManager.Save -= SaveGame;
        }
    }
}
