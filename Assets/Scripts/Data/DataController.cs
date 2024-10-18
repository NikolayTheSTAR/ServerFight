using System;
using UnityEngine;
using Newtonsoft.Json;

namespace TheSTAR.Data
{
    [Serializable]
    public sealed partial class DataController
    {
        private bool lockData = false; // когда true, перезапись сохранений заблокирована, файлы сохранений не могут быть изменены
        private bool clearData = false;

        public const string LocationKey = "location";

        public GameData gameData = new();

        public void Init(bool lockData, bool clearData)
        {
            this.lockData = lockData;
            this.clearData = clearData;

            Debug.Log("LOAD GAME");

            if (this.clearData) LoadDefault();
            else LoadAll();
        }

        [ContextMenu("Save")]
        private void ForceSave()
        {
            SaveAll(true);
        }

        public void SaveAll(bool force = false)
        {
            Debug.Log("SAVE ALL");
            for (int i = 0; i < gameData.sections.Length; i++)
            {
                var section = gameData.sections[i];
                Save(section, force);
            }
        }

        public void Save<T>(bool force = false) where T : DataSection
        {
            if (lockData && !force) return;

            var sectionData = gameData.GetSection<T>();
            Save(sectionData, force);
        }

        public void Save(DataSection sectionData, bool force = false)
        {
            if (lockData && !force) return;

            JsonSerializerSettings settings = new() { TypeNameHandling = TypeNameHandling.Objects };
            string jsonString = JsonConvert.SerializeObject(sectionData, Formatting.Indented, settings);

            PlayerPrefs.SetString(sectionData.FileName, jsonString);
        }

        public void LoadAll()
        {
            var sectionData = gameData.sections[0];
            if (PlayerPrefs.HasKey(sectionData.FileName))
            {
                for (int i = 0; i < gameData.sections.Length; i++)
                {
                    var section = gameData.sections[i];
                    LoadSection(section);
                }
            }
            else LoadDefault();

            void LoadSection(DataSection section)
            {
                //Debug.Log("Load section " + section);
                string jsonString = PlayerPrefs.GetString(section.FileName);
                DataSection loadedData;

                loadedData = JsonConvert.DeserializeObject<DataSection>(jsonString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                gameData.SetSection(loadedData);
            }
        }

        private void LoadDefault()
        {
            Debug.Log("LOAD DEFAULT");
            gameData = new();
            SaveAll();
        }
    }
}