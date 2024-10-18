using System;
using Newtonsoft.Json;

namespace TheSTAR.Data
{
    public sealed partial class DataController
    {
        [Serializable]
        public class GameData
        {
            public DataSection[] sections;

            public GameData()
            {
                sections = new DataSection[]
                {
                    new BattleData()
                };
            }

            public T GetSection<T>() where T : DataSection
            {
                for (int i = 0; i < sections.Length; i++)
                {
                    DataSection section = sections[i];
                    if (section is T result) return result;
                }

                return null;
            }

            public void SetSection<T>(T sectionData) where T : DataSection
            {
                for (int i = 0; i < sections.Length; i++)
                {
                    DataSection section = sections[i];
                    if (section is T)
                    {
                        sections[i] = sectionData;
                        return;
                    }
                }
            }
        }
    }

    [Serializable]
    public abstract class DataSection
    {
        [JsonIgnore] public abstract string FileName { get; }
    }

    [Serializable]
    public class BattleData : DataSection
    {
        [JsonIgnore] public override string FileName => "battle_data";

        public bool gameStarted = false;
        public BattleState battleState;
    }
}