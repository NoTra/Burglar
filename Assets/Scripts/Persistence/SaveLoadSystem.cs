using System;
using System.Collections;
using System.Collections.Generic;
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

            SceneManager.LoadScene("level1");
        }

        public void SaveGame()
        {
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
            GameManager.Instance.credit = gameData.Credit;

            SceneManager.LoadScene(gameData.CurrentLevelId);
        }

        public void DeleteGame(string gameName) => dataService.Delete(gameName);

        public void ReloadGame() => LoadGame(gameData.Name);

        public bool SaveExists() {
            var name = String.IsNullOrEmpty(gameData.Name) ? "My save" : gameData.Name;
            Debug.Log("SaveExists : " + name);
            return dataService.SaveExists(name);
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
