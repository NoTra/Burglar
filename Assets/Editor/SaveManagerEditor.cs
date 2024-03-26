using burglar.persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar.editor
{
    [UnityEditor.CustomEditor(typeof(SaveLoadSystem))]
    public class SaveManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaveLoadSystem saveLoadSystem = (SaveLoadSystem) target;
            string gameName = saveLoadSystem.gameData.Name;

            DrawDefaultInspector();

            if (GUILayout.Button("Save Game"))
            {
                saveLoadSystem.SaveGame();
            }

            if (GUILayout.Button("Load Game"))
            {
                saveLoadSystem.LoadGame(gameName);
            }

            if (GUILayout.Button("Delete Game"))
            {
                saveLoadSystem.DeleteGame(gameName);
            }
        }
    }
}
