using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;

namespace burglar.persistence
{
    public class FileDataService : IDataService
    {
        ISerializer serializer;
        string dataPath;
        string fileExtension;

        public FileDataService(ISerializer serializer)
        {
            this.serializer = serializer;
            this.dataPath = Application.persistentDataPath;
            this.fileExtension = ".json";
        }

        private string GetFilePath(string name)
        {
            return $"{dataPath}/{name}{fileExtension}";
        }

        public void Save(GameData data, bool overwrite = true)
        {
            string fileLocation = GetFilePath(data.Name);

            if (!overwrite && File.Exists(fileLocation))
            {
                throw new System.Exception($"File already exists at {fileLocation} and cannot be overwritten.");
            }
            
            File.WriteAllText(fileLocation, serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string fileLocation = GetFilePath(name);

            if (!File.Exists(fileLocation))
            {
                throw new System.Exception($"File does not exist at {fileLocation}.");
            }

            return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        public void Delete(string name)
        {
            string fileLocation = GetFilePath(name);

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
        }

        public void DeleteAll()
        {
            foreach (string filePath in Directory.GetFiles(dataPath))
            {
                if (Path.HasExtension(filePath) && Path.GetExtension(filePath) == fileExtension)
                {
                    File.Delete(filePath);
                }
            }
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string filePath in Directory.GetFiles(dataPath))
            {
                if (Path.HasExtension(filePath) && Path.GetExtension(filePath) == fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(filePath);
                }
            }
        }

        public bool SaveExists(string name)
        {
            string fileLocation = GetFilePath(name);
            Debug.Log(fileLocation);
            return File.Exists(GetFilePath(name));
        }
    }
}
